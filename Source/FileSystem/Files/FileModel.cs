using System;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public class FileModel : BaseModel
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public override bool IsEmpty()
    {
        return this.Id == Guid.Empty;
    }
}
