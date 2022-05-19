using CloudFileSystem.FileSystem.Exceptions;
using CloudFileSystem.FileSystem.Folders;

namespace CloudFileSystem.FileSystem.Files;

public class FileCreator
{
    private readonly IFileRepository fileRepository;
    private readonly IFolderRepository folderRepository;

    public FileCreator(IFileRepository fileRepository, IFolderRepository folderRepository)
    {
        this.fileRepository = fileRepository;
        this.folderRepository = folderRepository;
    }

    public FileResponseModel CreateFile(CreateOrUpdateFileModel file)
    {
        if (!this.folderRepository.FolderExists(file.FolderId))
        {
            throw new FileSystemException("Folder does not exist.");
        }

        if (this.fileRepository.FileExists(file.FolderId, file.Name))
        {
            throw new FileSystemException("File already exists.");
        }

        var createdFile = this.fileRepository.CreateFile(file);
        var ancestorIds = this.folderRepository.GetFileAncestorIds(createdFile.FolderId);
        return new FileResponseModel
        {
            Id = createdFile.Id,
            Name = createdFile.Name,
            AncestorFolderIds = ancestorIds
        };
    }
}
