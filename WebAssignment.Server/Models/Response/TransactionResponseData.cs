namespace WebAssignment.Server.Models.Response;

public class TransactionResponseData
{
    public required string TransactionId { get; set; }
    public required string Payment { get; set; }
    public required string Status { get; set; }
}
