using System.ComponentModel.DataAnnotations;
using WebAssignment.Server.Enums;

namespace WebAssignment.Server.Entities;

public class TransactionEntity
{
    public int Id { get; set; }
    [Required]
    public required string TransactionId { get; set; }
    [Required]
    public required string AccountNumber { get; set; }
    [Required]
    public required decimal Amount { get; set; }
    [Required]
    public required string CurrencyCode { get; set; }
    [Required]
    public required DateTimeOffset TransactionDate { get; set; }
    [Required]
    public required TransactionStatus Status { get; set; }
}
