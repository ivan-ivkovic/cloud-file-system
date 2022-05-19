using System;

namespace CloudFileSystem.FileSystemRepository.Files;

internal class FileDatabaseModel
{
    internal FileDatabaseModel()
    {
        this.Name = string.Empty;
    }

    public Guid Id { get; set; }

    public Guid ParentFolderId { get; set; }

    public string Name { get; set; }
}
