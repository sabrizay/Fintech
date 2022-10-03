namespace Fintech.Library.Entities.Models;

public class BaseResponse<T>
{
    public bool Success { get; set; } = false;
    public Error error { get; set; }
    public T Data { get; set; }
    public T First { get; set; }
    public T Last { get; set; }
    public int RowCount { get; set; }
    public int CurrentPageIndex { get; set; }

    public BaseResponse()
    {

    }
    public BaseResponse(T data, bool success)
    {
        this.Data = data;
        this.Success = success;

    }
    public BaseResponse(T data, bool success, int rowCount)
    {
        this.Data = data;
        this.Success = success;
        this.RowCount = rowCount;
    }
    public BaseResponse(T data, bool success, int currentPageIndex, int rowCount)
    {
        this.Data = data;
        this.RowCount = rowCount;
        this.Success = success;
        this.CurrentPageIndex = currentPageIndex;
    }
    public BaseResponse(T data, bool success, Error error)
    {
        this.Data = data;
        this.error = error;
        this.Success = success;

    }

}

public class BaseResponse
{
    public bool Success { get; set; } = false;
    public Error error { get; set; }

    public BaseResponse()
    {
        Success = true;
    }
    public BaseResponse(bool success)
    {
        this.Success = success;
    }
    public BaseResponse(bool success, Error error)
    {
        this.Success = success;
        this.error = error;
    }
}

public class Error
{
    public Guid errorLogId { get; set; }
    /// <summary>
    ///     1 TRACE. 2 DEBUG.3 WARN.4 ERROR.5 FATAL
    /// </summary>
    public int errorLevel { get; set; }
    public Exception exception { get; set; }
    public string message { get; set; }
}
