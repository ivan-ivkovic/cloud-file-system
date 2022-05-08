using System;
using System.Collections.Generic;
using System.Linq;

namespace P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

public class FolderResponseModel : BaseFolderModel
{
    public FolderResponseModel()
    {
        this.FolderName = string.Empty;
        this.ParentFolderIds = new List<Guid>();
    }

    public FolderResponseModel(FolderModel folderModel, IEnumerable<Guid> parentFolderIds)
    {
        this.Id = folderModel.Id;
        this.ParentFolderId = folderModel.ParentFolderId;
        this.FolderName = folderModel.FolderName;
        this.ParentFolderIds = parentFolderIds;
    }

    public Guid Id { get; set; }

    public Guid ParentFolderId { get; set; }

    public string FolderName { get; set; }

    public IEnumerable<Guid> ParentFolderIds { get; set; }

    public override bool IsEmpty()
    {
        return
            this.Id == Guid.Empty &&
            this.ParentFolderId == Guid.Empty &&
            this.ParentFolderIds.Count() == 0;
    }
}
