using System;
using System.Collections.Generic;
using CloudFileSystem.FileSystem.Folders.Models;

namespace CloudFileSystem.FileSystem.Folders;

public class FolderGetter
{
    private readonly IFolderRepository folderRepository;

    public FolderGetter(IFolderRepository folderRepository)
    {
        this.folderRepository = folderRepository;
    }

    public FolderResponseModel GetFolder(Guid folderId)
    {
        FolderModel? folder = this.folderRepository.GetFolder(folderId);
        if (folder == null)
        {
            return new EmptyFolderResponseModel();
        }

        IEnumerable<Guid> ancestors = this.folderRepository.GetFolderAncestorIds(folderId);
        return new FolderResponseModel(folder, ancestors);
    }
}
