using System;

namespace P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

public class CreateFolderModel
{
    public CreateFolderModel()
    {
        this.FolderName = string.Empty;
    }

    public Guid ParentFolderId { get; set; }

    public string FolderName { get; set; }
}