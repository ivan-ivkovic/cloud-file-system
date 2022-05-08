using System;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public class FileModel : BaseFileModel
{
    public FileModel()
    {
        this.Name = string.Empty;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public override bool IsEmpty()
    {
        return this.Id == Guid.Empty;
    }
}
