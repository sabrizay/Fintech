namespace Fintech.Library.Core.Utilities.Results.API;

public class ApiResult : IApiResult
{
    public ApiResult(bool success, string message) : this(success)
    {
        Message = message;
    }
    public ApiResult(bool success)
    {
        Success = success;
    }

    public bool Success { get; }
    public string Message { get; }
}
