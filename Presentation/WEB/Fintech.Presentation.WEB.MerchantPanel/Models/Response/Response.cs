namespace Fintech.Presentation.WEB.MerchantPanel.Models.Response;

public class Response<T>
{
    public T Data { get; set; }
    public bool Success { get; set; } = false;
    public string Message { get; set; }


}
