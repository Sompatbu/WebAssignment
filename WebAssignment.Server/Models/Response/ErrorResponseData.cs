namespace WebAssignment.Server.Models.Response;

public class ErrorResponseData(int statusCode, string message)
{
    public int StatusCode { get; set; } = statusCode;

    public string? Message { get; set; } = message;
}