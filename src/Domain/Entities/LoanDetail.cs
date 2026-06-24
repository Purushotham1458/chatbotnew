namespace Domain.Entities;

public class LoanDetail
{
    public int Id { get; set; }
    public string LnNo { get; set; } = string.Empty;
    public string BorrowerName { get; set; } = string.Empty;
    public string LoanType { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public decimal EmiAmount { get; set; }
    public decimal OutstandingBalance { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DisbursementDate { get; set; }
    public DateTime? MaturityDate { get; set; }
    public int EmisPaid { get; set; }
    public int EmisRemaining { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextDueDate { get; set; }
    public string Collateral { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
