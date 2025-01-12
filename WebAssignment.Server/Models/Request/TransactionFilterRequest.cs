using WebAssignment.Server.Enums;

namespace WebAssignment.Server.Models.Request;

public class TransactionFilterRequest
{
    public string? CurrencyCode { get; set; }
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public TransactionStatus? Status { get; set; }
}
