namespace FileManager.Core.Models;

public struct UnixPermission
{
    public UnixPermission(FilePermission owner, FilePermission group, FilePermission other)
    {
        var ownerNum = (short) owner;
        var groupNum = (short) group;
        var otherNum = (short) other;

        Permission = (short)(otherNum + groupNum * 10 + ownerNum * 100);
    }

    public short Permission { get; set; }
}

public enum FilePermission
{
    Write = 2,
    Read = 4,
}
