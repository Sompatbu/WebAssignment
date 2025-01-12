using WebAssignment.Server.Entities;
using WebAssignment.Server.Enums;

namespace WebAssignment.ServerTests.Repositories;

[TestClass()]
public class TransactionRepositoryTests
{
    [TestMethod()]
    public async Task AddRangeAsyncTest()
    {
        Server.Repositories.AssignmentRepositories repositories = TestServicesFactory.GetAssignmentRepositories();
        TransactionEntity[] transactions = [
            new() {
                TransactionId = "Test-Transaction-1",
                AccountNumber = "12345678",
                Amount = 500,
                CurrencyCode = "THB",
                TransactionDate = DateTimeOffset.UtcNow,
                Status = TransactionStatus.Approved,
            }
            ];

        int result = await repositories.Transaction.AddRangeAsync(transactions);

        Assert.IsTrue(result > 0);
        Assert.IsTrue(transactions[0].Id > 0);
    }

    [TestMethod()]
    public async Task FindTransactionsAsyncTest()
    {
        Server.Repositories.AssignmentRepositories repositories = TestServicesFactory.GetAssignmentRepositories();
        TransactionEntity[] transactions = [
            new() {
                TransactionId = "Test-Find-Transaction",
                AccountNumber = "87654321",
                Amount = 100,
                CurrencyCode = "USD",
                TransactionDate = DateTimeOffset.UtcNow,
                Status = TransactionStatus.Done,
            }
            ];

        int result = await repositories.Transaction.AddRangeAsync(transactions);

        Assert.IsTrue(result > 0);
        Assert.IsTrue(transactions[0].Id > 0);

        Server.Models.Response.TransactionResponseData[] findResult = await repositories.Transaction.FindTransactions(new());
        Assert.IsNotNull(findResult);
        Assert.IsTrue(findResult.Length > 1);
        Assert.AreEqual(transactions[0].TransactionId, findResult[0].Id);
    }
}