namespace Fintech.Library.Entities.Abstract;
public class BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime? CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDate { get; set; } = DateTime.UtcNow;
    public int CreateUserId { get; set; } = 0;
    public int UpdateUserId { get; set; } = 0;
}
