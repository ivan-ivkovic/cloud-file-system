using System;
using System.Collections.Generic;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public interface IFileRepository
{
    public IEnumerable<FileModel>? GetAllFiles();

    public FileModel? GetFileById(Guid id);
}
