using P3Mobility.CloudFileSystem.FileSystem.Exceptions;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public class FileCreator
{
    private readonly IFileRepository fileRepository;

    public FileCreator(IFileRepository fileRepository)
    {
        this.fileRepository = fileRepository;
    }

    public FileModel CreateFile(CreateOrUpdateFileModel file)
    {
        if (this.fileRepository.FileExists(file.FolderId, file.Name))
        {
            throw new FileSystemException("File already exists.");
        }

        return this.fileRepository.CreateFile(file);
    }
}
