namespace FileManager.Core.Models;

public class File : Entity
{
    public required string FileName { get; set; }
    public int Size { get; set; }
    public required string MimeType { get; set; }
    public string Extension => Path.GetExtension(FileName);
    public Guid? FolderId { get; set; }
    public Guid OwnerId { get; set; }
    public string? Group { get; set; }
    public UnixPermission UnixPermission { get; set; }
}
