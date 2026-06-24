using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class LoanRepository : ILoanRepository
{
    private readonly AppDbContext _db;

    public LoanRepository(AppDbContext db) => _db = db;

    public async Task<LoanDetail?> GetByLoanNumberAsync(string lnNo)
    {
        return await _db.LoanDetails
            .FirstOrDefaultAsync(l => l.LnNo.ToUpper() == lnNo.ToUpper());
    }

    public async Task<List<LoanDetail>> SearchLoansAsync(string keyword)
    {
        return await _db.LoanDetails
            .Where(l => l.LnNo.Contains(keyword) ||
                        l.BorrowerName.Contains(keyword) ||
                        l.Status.Contains(keyword))
            .Take(10)
            .ToListAsync();
    }
}
