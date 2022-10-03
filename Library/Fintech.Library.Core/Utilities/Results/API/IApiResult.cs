namespace Fintech.Library.Core.Utilities.Results.API;

public interface IApiResult
{
    bool Success { get; }
    string Message { get; }
}
