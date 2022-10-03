namespace Fintech.ExternalService.StorageHelper.Models;

internal class CloudflareResponseModel
{
    public CloudflareResponseResultModel Result { get; set; }
    public object Result_Info { get; set; }
    public bool Success { get; set; }
    public string[] Errors { get; set; }
    public string[] Messages { get; set; }
}
