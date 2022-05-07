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

    public bool FileExists(string fileName)
    {
        if (this.dbContext.Files == null)
        {
            throw new Exception();
        }

        return this.dbContext.Files.Any<FileModel>(x => x.Name == fileName);
    }

    public FileModel? GetFileById(Guid id)
    {
        return this.dbContext.Files?.Where(x => x.Id == id).FirstOrDefault();
    }

    public FileModel CreateFile(CreateOrUpdateFileModel file)
    {
        if (this.dbContext.Files == null)
        {
            throw new Exception();
        }

        var addedFile = this.dbContext.Files.Add(new FileModel()
        {
            Id = Guid.NewGuid(),
            Name = file.Name
        });
        this.dbContext.SaveChanges();
        return addedFile.Entity;
    }
}
