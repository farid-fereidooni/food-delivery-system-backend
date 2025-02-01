namespace FileManager.Core.Models;

public abstract class Entity
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
