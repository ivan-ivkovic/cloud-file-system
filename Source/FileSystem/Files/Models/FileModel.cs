using System;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public class FileModel
{
    public FileModel()
    {
        this.Name = string.Empty;
    }

    public Guid Id { get; set; }

    public Guid FolderId { get; set; }

    public string Name { get; set; }
}
