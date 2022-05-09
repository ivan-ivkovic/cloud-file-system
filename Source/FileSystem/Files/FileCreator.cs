using P3Mobility.CloudFileSystem.FileSystem.Exceptions;
using P3Mobility.CloudFileSystem.FileSystem.Folders;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public class FileCreator
{
    private readonly IFileRepository fileRepository;
    private readonly IFolderRepository folderRepository;

    public FileCreator(IFileRepository fileRepository, IFolderRepository folderRepository)
    {
        this.fileRepository = fileRepository;
        this.folderRepository = folderRepository;
    }

    public FileModel CreateFile(CreateOrUpdateFileModel file)
    {
        if (!this.folderRepository.FolderExists(file.FolderId))
        {
            throw new FileSystemException("Folder does not exist.");
        }

        if (this.fileRepository.FileExists(file.FolderId, file.Name))
        {
            throw new FileSystemException("File already exists.");
        }

        return this.fileRepository.CreateFile(file);
    }
}
