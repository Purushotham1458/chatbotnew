using System.Text;
using System.Text.RegularExpressions;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ChatService : IChatService
{
    private readonly IAiProvider _aiProvider;
    private readonly IChatRepository _chatRepo;
    private readonly ILoanRepository _loanRepo;

    public ChatService(IAiProvider aiProvider, IChatRepository chatRepo, ILoanRepository loanRepo)
    {
        _aiProvider = aiProvider;
        _chatRepo = chatRepo;
        _loanRepo = loanRepo;
    }

    public async Task<ChatResponse> SendMessageAsync(string clientId, ChatRequest request)
    {
        var context = await BuildLoanContextAsync(request.Message);
        var reply = await _aiProvider.GetResponseAsync(request.Message, context);
        await _chatRepo.SaveMessageAsync(clientId, request.Message, reply);
        return new ChatResponse(reply, DateTime.UtcNow);
    }

    public async Task<List<ChatResponse>> GetHistoryAsync(string clientId)
    {
        return await _chatRepo.GetHistoryAsync(clientId);
    }

    private async Task<string?> BuildLoanContextAsync(string message)
    {
        // Extract loan number pattern (e.g., A000001, LN001, L-12345)
        var loanNoMatch = Regex.Match(message, @"[A-Za-z]{1,3}[-]?\d{3,}", RegexOptions.IgnoreCase);

        LoanDetail? loan = null;

        if (loanNoMatch.Success)
        {
            loan = await _loanRepo.GetByLoanNumberAsync(loanNoMatch.Value);
        }

        if (loan == null)
        {
            // Try keyword search if no direct match
            var keywords = new[] { "loan", "emi", "status", "balance", "payment", "disbursement", "maturity", "overdue" };
            var isLoanQuery = keywords.Any(k => message.Contains(k, StringComparison.OrdinalIgnoreCase));

            if (!isLoanQuery) return null;

            // If asking about loan but no specific number found
            if (!loanNoMatch.Success)
                return "The user is asking about a loan but did not provide a loan number. Ask them to provide the loan number (e.g., A000001) so you can look up the details.";
        }

        if (loan == null) return null;

        var sb = new StringBuilder();
        sb.AppendLine("Here is the loan data from our database. Use this to answer the user's question with a clear explanation:");
        sb.AppendLine();
        sb.AppendLine($"| Field | Value |");
        sb.AppendLine($"|-------|-------|");
        sb.AppendLine($"| Loan Number | {loan.LnNo} |");
        sb.AppendLine($"| Borrower Name | {loan.BorrowerName} |");
        sb.AppendLine($"| Loan Type | {loan.LoanType} |");
        sb.AppendLine($"| Loan Amount | ₹{loan.LoanAmount:N2} |");
        sb.AppendLine($"| Interest Rate | {loan.InterestRate}% per annum |");
        sb.AppendLine($"| Tenure | {loan.TenureMonths} months |");
        sb.AppendLine($"| EMI Amount | ₹{loan.EmiAmount:N2} |");
        sb.AppendLine($"| Outstanding Balance | ₹{loan.OutstandingBalance:N2} |");
        sb.AppendLine($"| Status | {loan.Status} |");
        sb.AppendLine($"| Disbursement Date | {loan.DisbursementDate:dd-MMM-yyyy} |");
        sb.AppendLine($"| Maturity Date | {loan.MaturityDate?.ToString("dd-MMM-yyyy") ?? "N/A"} |");
        sb.AppendLine($"| EMIs Paid | {loan.EmisPaid} |");
        sb.AppendLine($"| EMIs Remaining | {loan.EmisRemaining} |");
        sb.AppendLine($"| Last Payment Date | {loan.LastPaymentDate?.ToString("dd-MMM-yyyy") ?? "N/A"} |");
        sb.AppendLine($"| Next Due Date | {loan.NextDueDate?.ToString("dd-MMM-yyyy") ?? "N/A"} |");
        sb.AppendLine($"| Collateral | {loan.Collateral} |");
        sb.AppendLine($"| Branch | {loan.Branch} |");

        return sb.ToString();
    }
}
