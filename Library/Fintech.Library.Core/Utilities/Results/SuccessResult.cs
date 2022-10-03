namespace Fintech.Library.Core.Utilities.Results;

public class SuccessApiResult : Result
{
    public SuccessApiResult() : base(true)
    {
    }

    public SuccessApiResult(string message) : base(true, message)
    {
    }
}
