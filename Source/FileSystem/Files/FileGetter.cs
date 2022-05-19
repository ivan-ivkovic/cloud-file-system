using System;
using System.Collections.Generic;

using CloudFileSystem.FileSystem.Folders;

namespace CloudFileSystem.FileSystem.Files;

public class FileGetter
{
    private readonly IFileRepository fileRepository;
    private readonly IFolderRepository folderRepository;

    public FileGetter(IFileRepository fileRepository, IFolderRepository folderRepository)
    {
        this.fileRepository = fileRepository;
        this.folderRepository = folderRepository;
    }

    public FileResponseModel GetFile(Guid id)
    {
        var file = this.fileRepository.GetFileById(id);
        if (file == null)
        {
            return new EmptyFileResponseModel();
        }

        var ancestorIds = this.folderRepository.GetFolderAncestorIds(file.FolderId);
        return new FileResponseModel
        {
            Id = file.Id,
            Name = file.Name,
            AncestorFolderIds = ancestorIds
        };
    }

    public IEnumerable<FileResponseModel> GetFiles()
    {
        var files = this.fileRepository.GetAllFiles();
        if (files == null)
        {
            return new List<FileResponseModel>();
        }

        var fileResponseModels = new List<FileResponseModel>();
        foreach (var file in files)
        {
            fileResponseModels.Add(new FileResponseModel
            {
                Id = file.Id,
                Name = file.Name,
                AncestorFolderIds = this.folderRepository.GetFileAncestorIds(file.FolderId)
            });
        }

        return fileResponseModels;
    }
}
