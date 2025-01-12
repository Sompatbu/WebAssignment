using WebAssignment.Server.Enums;

namespace WebAssignment.Server.Models.Request;

public class TransactionFilterRequest
{
    public string? CurrencyCode { get; set; }
    public DateTimeOffset? From { get; set; }
    public DateTimeOffset? To { get; set; }
    public TransactionStatus? Status { get; set; }
}
