using WebAssignment.Server.Context;

namespace WebAssignment.Server.Repositories;

public class AssignmentRepositories(TransactionContext context)
{
    public TransactionRepository Transaction
    {
        get
        {
            _transactionRepository ??= new(context);

            return _transactionRepository;
        }
    }

    private TransactionRepository? _transactionRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
