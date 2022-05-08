using System;
using P3Mobility.CloudFileSystem.FileSystem.Exceptions;
using P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

namespace P3Mobility.CloudFileSystem.FileSystem.Folders;

public class FolderCreator
{
    private readonly IFolderRepository folderRepository;

    public FolderCreator(IFolderRepository folderRepository)
    {
        this.folderRepository = folderRepository;
    }

    public FolderResponseModel CreateFolder(Guid parentFolderId, string folderName)
    {
        if (!this.folderRepository.FolderExists(parentFolderId))
        {
            throw new FileSystemException($"Parent folder could not be found.");
        }

        if (this.folderRepository.IsChildFolderNameTaken(parentFolderId, folderName))
        {
            throw new FileSystemException($"Folder with name {folderName} already exists.");
        }

        var createdFolder = this.folderRepository.CreateFolder(parentFolderId, folderName);
        var ancestors = this.folderRepository.GetFolderAncestorIds(createdFolder.Id);
        return new FolderResponseModel(createdFolder, ancestors);
    }
}
