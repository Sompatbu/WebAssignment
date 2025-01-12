using WebAssignment.Server.Context;
using WebAssignment.Server.Entities;

namespace WebAssignment.Server.Repositories;

public class TransactionRepository(TransactionContext context)
{
    public async Task<int> AddRangeAsync(IEnumerable<TransactionEntity> items, CancellationToken cancellationToken = default)
    {
        await context.Transactions.AddRangeAsync(items, cancellationToken);

        return await context.SaveChangesAsync(cancellationToken);
    }
}
