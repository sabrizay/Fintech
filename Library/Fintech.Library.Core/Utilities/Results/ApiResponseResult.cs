using Fintech.Library.Entities.Abstract;

namespace Fintech.Library.Core.Utilities.Results;

public class ApiResponseResult : IDto
{
    public ApiResponseResult(string code, bool isSuccess)
    {
        Code = code;
        IsSuccess = isSuccess;
    }
    public ApiResponseResult(string code, bool isSuccess, string message)
    {
        Code = code;
        IsSuccess = isSuccess;
        Message = message;
    }
    public string Code { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}
