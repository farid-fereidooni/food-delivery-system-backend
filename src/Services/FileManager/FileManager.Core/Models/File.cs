namespace FileManager.Core.Models;

public class File : Entity
{
    public required string FileName { get; set; }
    public long Size { get; set; }
    public required string MimeType { get; set; }
    public string Extension => Path.GetExtension(FileName);
    public Guid OwnerId { get; set; }
    public string? Group { get; set; }
    public bool IsTemporary { get; set; }

    public FilePermission OwnerPermission { get; set; }
    public FilePermission GroupPermission { get; set; }
    public FilePermission OtherPermission { get; set; }

    public short UnixPermission =>
        (short)((short)OtherPermission + (short)GroupPermission * 10 + (short)OwnerPermission * 100);

    public void SetDefaultPermission()
    {
        OwnerPermission = FilePermission.Read | FilePermission.Write;
        GroupPermission = FilePermission.None;
        OtherPermission = FilePermission.None;
    }
}

[Flags]
public enum FilePermission : byte
{
    None = 0,
    Write = 2,
    Read = 4
}
