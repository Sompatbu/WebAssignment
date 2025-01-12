using WebAssignment.Server.Enums;
using WebAssignment.Server.Models.Response;

namespace WebAssignment.Server.Models.DTOs;

public class TransactionDto
{
    public string? TransactionId { get; set; }
    public string? AccountNumber { get; set; }
    public decimal? Amount { get; set; }
    public string? CurrencyCode { get; set; }
    public DateTimeOffset? TransactionDate { get; set; }
    public TransactionStatus? Status { get; set; }
    public ErrorResponseData[]? Error { get; set; }
}
