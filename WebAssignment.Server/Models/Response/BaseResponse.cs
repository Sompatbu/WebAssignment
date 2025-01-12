namespace WebAssignment.Server.Models.Response;

public class BaseResponse<T>
{
    public T? Data { get; set; }

    public ErrorResponseData[]? Error { get; set; }

    public bool Success => Data != null && Error == null;

    public BaseResponse()
    {
    }

    public BaseResponse(T data)
    {
        Data = data;
    }

    public BaseResponse(ErrorResponseData[] error)
    {
        Error = error;
    }
}
