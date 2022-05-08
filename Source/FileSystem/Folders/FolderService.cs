using System;
using P3Mobility.CloudFileSystem.FileSystem.Exceptions;
using P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

namespace P3Mobility.CloudFileSystem.FileSystem.Folders;

public class FolderService
{
    private readonly IFolderRepository folderRepository;

    public FolderService(IFolderRepository folderRepository)
    {
        this.folderRepository = folderRepository;
    }

    public FolderModel GetFolder(Guid folderId)
    {
        FolderModel? folder = this.folderRepository.GetFolder(folderId);
        if (folder == null)
        {
            return new EmptyFolderModel();
        }

        return folder;
    }

    public FolderModel CreateFolder(Guid parentFolderId, string folderName)
    {
        if (this.folderRepository.FolderExists(parentFolderId))
        {
            throw new FileSystemException($"Parent folder could not be found.");
        }

        if (this.folderRepository.IsChildFolderNameTaken(parentFolderId, folderName))
        {
            throw new FileSystemException($"Folder with name {folderName} already exists.");
        }

        return this.folderRepository.CreateFolder(parentFolderId, folderName);
    }
}
