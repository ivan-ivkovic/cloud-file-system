using System;
using System.Collections.Generic;

using P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

namespace P3Mobility.CloudFileSystem.FileSystem.Folders;

public interface IFolderRepository
{
    bool FolderExists(Guid folderId);

    bool IsChildFolderNameTaken(Guid parentFolderId, string childFolderName);

    FolderModel? GetFolder(Guid folderId);

    IEnumerable<Guid> GetFolderAncestorIds(Guid folderId);

    FolderModel CreateFolder(Guid parentFolderId, string folderPath);
}
