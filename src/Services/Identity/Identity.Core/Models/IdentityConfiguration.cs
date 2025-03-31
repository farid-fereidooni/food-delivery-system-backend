namespace Identity.Core.Models;

public class IdentityConfiguration
{
    public string EncryptionCertificatePath { get; set; } = null!;
    public string SigningCertificatePath { get; set; } = null!;
    public string? IdentityPublicAddress { get; set; }
}
