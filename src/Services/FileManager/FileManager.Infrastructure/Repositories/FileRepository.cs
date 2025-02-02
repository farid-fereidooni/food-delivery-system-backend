using FileManager.Core.Contracts;
using FileManager.Infrastructure.Database;
using File = FileManager.Core.Models.File;

namespace FileManager.Infrastructure.Repositories;

public class FileRepository : BaseRepository<File>, IFileRepository
{
    private readonly AppDbContext _dbContext;

    public FileRepository(AppDbContext dbContext) : base(dbContext.Files)
    {
        _dbContext = dbContext;
    }

    public override async Task<File?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Files.FindAsync(id, cancellationToken);
    }
}
