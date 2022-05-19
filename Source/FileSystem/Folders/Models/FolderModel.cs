using System;

namespace CloudFileSystem.FileSystem.Folders.Models;

public class FolderModel
{
    public FolderModel()
    {
        this.FolderName = string.Empty;
    }

    public Guid Id { get; set; }

    public string FolderName { get; set; }
}
