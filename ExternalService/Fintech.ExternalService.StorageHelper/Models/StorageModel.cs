namespace Fintech.ExternalService.StorageHelper.Models;

public class StorageModel
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string BaseUrl { get; set; }
    public string FileId { get; set; }
    public string FileName { get; set; }
    public string FullPath { get; set; } //=> Path.Combine(BaseUrl, FileName);
}
