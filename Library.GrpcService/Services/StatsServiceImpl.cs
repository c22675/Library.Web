using Grpc.Core;
using Library.Data;
using Library.Grpc;
using Microsoft.EntityFrameworkCore;

namespace Library.GrpcService.Services;

public class StatsServiceImpl : StatsService.StatsServiceBase
{
    private readonly AppDbContext _db;

    public StatsServiceImpl(AppDbContext db)
    {
        _db = db;
    }

    public override async Task<StatsReply> GetDashboardStats(StatsRequest request, ServerCallContext context)
    {
        var totalBooks = await _db.Books.CountAsync();
        var totalCustomers = await _db.Customers.CountAsync();
        var activeLoans = await _db.Loans.CountAsync(l => l.ReturnDate == null);

        return new StatsReply
        {
            TotalBooks = totalBooks,
            TotalCustomers = totalCustomers,
            ActiveLoans = activeLoans
        };
    }
}