using System;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

internal sealed class EmptyFileModel : FileModel
{
    internal EmptyFileModel()
    {
        this.Id = Guid.Empty;
        this.Name = string.Empty;
    }
}
