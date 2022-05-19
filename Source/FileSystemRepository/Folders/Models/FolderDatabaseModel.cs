using System;

namespace CloudFileSystem.FileSystemRepository.Folders.Models;

internal class FolderDatabaseModel
{
    internal FolderDatabaseModel()
    {
        this.FolderName = string.Empty;
    }

    public Guid Id { get; set; }

    public Guid ParentFolderId { get; set; }

    public string FolderName { get; set; }
}
