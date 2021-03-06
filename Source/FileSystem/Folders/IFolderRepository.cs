using System;
using System.Collections.Generic;

using CloudFileSystem.FileSystem.Folders.Models;

namespace CloudFileSystem.FileSystem.Folders;

public interface IFolderRepository
{
    bool FolderExists(Guid folderId);

    bool IsChildFolderNameTaken(Guid parentFolderId, string childFolderName);

    FolderModel? GetFolder(Guid folderId);

    IEnumerable<Guid> GetFolderAncestorIds(Guid folderId);

    IEnumerable<Guid> GetFileAncestorIds(Guid folderId);

    FolderModel CreateFolder(Guid parentFolderId, string folderPath);
}
