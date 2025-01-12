using WebAssignment.Server.Extension;
using WebAssignment.Server.Models.DTOs;
using WebAssignment.Server.Models.Request;
using WebAssignment.Server.Models.Response;
using WebAssignment.Server.Repositories;

namespace WebAssignment.Server.Services;

public class TransactionService(AssignmentRepositories repositories)
{
    public async Task<bool> InsertTransactionsAsync(IEnumerable<TransactionDto> transaction)
    {
        int response = await repositories.Transaction.AddRangeAsync(transaction.Select(item => item.ToTransactionEntity()));
        return response > 0;
    }

    public async Task<BaseResponse<TransactionResponseData[]>> FindTransactionsAsync(TransactionFilterRequest filter)
    {
        TransactionResponseData[] response = await repositories.Transaction.FindTransactions(filter);
        return new(response);
    }
}
