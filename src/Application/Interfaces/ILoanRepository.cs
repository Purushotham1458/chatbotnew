using Domain.Entities;

namespace Application.Interfaces;

public interface ILoanRepository
{
    Task<LoanDetail?> GetByLoanNumberAsync(string lnNo);
    Task<List<LoanDetail>> SearchLoansAsync(string keyword);
}
