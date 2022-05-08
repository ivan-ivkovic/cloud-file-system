using System;

namespace P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

public class FolderModel : BaseFolderModel
{
    public FolderModel()
    {
        this.FolderName = string.Empty;
    }

    public Guid Id { get; set; }

    public string FolderName { get; set; }

    public override bool IsEmpty()
    {
        return this.Id == Guid.Empty;
    }
}
