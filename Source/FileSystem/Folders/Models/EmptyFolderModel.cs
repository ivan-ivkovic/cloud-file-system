using System;
namespace P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

internal sealed class EmptyFolderModel : FolderModel
{
    internal EmptyFolderModel()
    {
        this.Id = Guid.Empty;
    }
}
