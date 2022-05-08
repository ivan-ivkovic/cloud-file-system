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
        return this.dbContext.Files?.Select(x => new FileModel
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    public bool FileExists(Guid folderId, string fileName)
    {
        if (this.dbContext.Files == null)
        {
            throw new Exception();
        }

        return this.dbContext.Files.Any<FileDatabaseModel>(
            x => x.ParentFolderId == folderId && x.Name == fileName
        );
    }

    public FileModel? GetFileById(Guid id)
    {
        return this.dbContext.Files?
            .Where(x => x.Id == id)
            .Select(x => new FileModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .FirstOrDefault();
    }

    public FileModel CreateFile(CreateOrUpdateFileModel file)
    {
        if (this.dbContext.Files == null)
        {
            throw new Exception();
        }

        var addedFile = this.dbContext.Files.Add(new FileDatabaseModel
        {
            Id = Guid.NewGuid(),
            ParentFolderId = file.FolderId,
            Name = file.Name
        });
        this.dbContext.SaveChanges();
        return new FileModel
        {
            Id = addedFile.Entity.Id,
            Name = addedFile.Entity.Name
        };
    }
}
