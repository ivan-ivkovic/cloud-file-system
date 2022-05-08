using System;
using System.Collections.Generic;

namespace P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

public class EmptyFolderResponseModel : FolderResponseModel
{
    public EmptyFolderResponseModel()
        : base(
            new FolderModel
            {
                Id = Guid.Empty,
                ParentFolderId = Guid.Empty,
                FolderName = string.Empty
            },
            new List<Guid>()
        )
    {
    }
}