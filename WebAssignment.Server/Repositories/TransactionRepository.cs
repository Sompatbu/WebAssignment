using LinqKit;
using Microsoft.EntityFrameworkCore;
using WebAssignment.Server.Context;
using WebAssignment.Server.Entities;
using WebAssignment.Server.Extension;
using WebAssignment.Server.Models.Request;
using WebAssignment.Server.Models.Response;

namespace WebAssignment.Server.Repositories;

public class TransactionRepository(TransactionContext context)
{
    public async Task<int> AddRangeAsync(IEnumerable<TransactionEntity> items, CancellationToken cancellationToken = default)
    {
        await context.Transactions.AddRangeAsync(items, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TransactionResponseData[]> FindTransactions(TransactionFilterRequest filter, CancellationToken cancellationToken = default)
    {
        var predicate = PredicateBuilder.New<TransactionEntity>(true);

        if (!string.IsNullOrEmpty(filter.CurrencyCode))
            predicate = predicate.And(entity => entity.CurrencyCode == filter.CurrencyCode);

        if (filter.From.HasValue)
        {
            var fromDateTime = filter.From.Value.ToDateTime(new TimeOnly(), DateTimeKind.Utc);
            var fromDateTimeOffset = new DateTimeOffset(fromDateTime);
            predicate = predicate.And(entity => entity.TransactionDate >= fromDateTimeOffset);
        }

        if (filter.To.HasValue)
        {
            var toDateTime = filter.To.Value.ToDateTime(new TimeOnly(), DateTimeKind.Utc);
            var toDateTimeOffset = new DateTimeOffset(toDateTime);
            predicate = predicate.And(entity => entity.TransactionDate < toDateTimeOffset);
        }

        if (filter.Status is not null)
            predicate = predicate.And(entity => entity.Status == filter.Status);

        return await context.Transactions.AsNoTracking()
            .Where(predicate)
            .OrderByDescending(item => item.TransactionDate)
            .Select(item => item.ToTransactionResponseData())
            .ToArrayAsync(cancellationToken);
    }
}
