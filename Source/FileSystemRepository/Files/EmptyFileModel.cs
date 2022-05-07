using System;

using P3Mobility.CloudFileSystem.FileSystem.Files;

namespace P3Mobility.CloudFileSystem.FileSystemRepository.Files;

internal class EmptyFileModel : FileModel
{
    internal EmptyFileModel()
    {
        this.Id = Guid.Empty;
        this.Name = string.Empty;
    }
}