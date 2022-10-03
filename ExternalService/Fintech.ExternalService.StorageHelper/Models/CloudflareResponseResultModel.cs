namespace Fintech.ExternalService.StorageHelper.Models;

internal class CloudflareResponseResultModel
{
    public string Id { get; set; }
    public List<string> Variants { get; set; }
    public string FileName { get; set; }
    public DateTime Uploaded { get; set; }
    public bool RequireSignedURLs { get; set; }
}
