using WebAssignment.Server.Entities;
using WebAssignment.Server.Repositories;

namespace WebAssignment.Server.Services;

public class TransactionService(AssignmentRepositories repositories)
{
    public async Task<int> InsertAsync(IEnumerable<TransactionEntity> transaction)
    {
        var response = await repositories.Transaction.AddRangeAsync(transaction);
        return response;
    }
}
