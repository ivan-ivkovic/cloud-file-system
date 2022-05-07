using System;
using System.Collections.Generic;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public class FileService
{
    private readonly IFileRepository fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        this.fileRepository = fileRepository;
    }

    public FileModel GetFile(Guid id)
    {
        return this.fileRepository.GetFileById(id);
    }

    public IEnumerable<FileModel> GetFiles()
    {
        return this.fileRepository.GetAllFiles();
    }
}