using System;

namespace P3Mobility.CloudFileSystem.FileSystemRepository.Files;

internal class FileDatabaseModel
{
    internal FileDatabaseModel()
    {
        this.Name = string.Empty;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }
}
