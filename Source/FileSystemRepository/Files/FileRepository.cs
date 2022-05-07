using System;
using System.Collections.Generic;
using System.Linq;

using P3Mobility.CloudFileSystem.FileSystem.Files;

namespace P3Mobility.CloudFileSystem.FileSystemRepository.Files;

internal class FileRepository : IFileRepository
{
    private readonly DatabaseContext dbContext;

    public FileRepository(DatabaseContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IEnumerable<FileModel>? GetAllFiles()
    {
        return this.dbContext.Files;
    }

    public FileModel? GetFileById(Guid id)
    {
        return this.dbContext.Files?.Where(x => x.Id == id).FirstOrDefault();
    }
}
