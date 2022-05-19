using System;
using System.Collections.Generic;

namespace CloudFileSystem.FileSystem.Files;

public interface IFileRepository
{
    public IEnumerable<FileModel>? GetAllFiles();

    public bool FileExists(Guid folderId, string fileName);

    public FileModel? GetFileById(Guid id);

    public FileModel CreateFile(CreateOrUpdateFileModel file);
}
