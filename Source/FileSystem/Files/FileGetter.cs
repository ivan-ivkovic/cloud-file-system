using System;
using System.Collections.Generic;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public class FileGetter
{
    private readonly IFileRepository fileRepository;

    public FileGetter(IFileRepository fileRepository)
    {
        this.fileRepository = fileRepository;
    }

    public FileModel GetFile(Guid id)
    {
        var file = this.fileRepository.GetFileById(id);
        if (file == null)
        {
            return new EmptyFileModel();
        }

        return file;
    }

    public IEnumerable<FileModel> GetFiles()
    {
        var files = this.fileRepository.GetAllFiles();
        if (files == null)
        {
            return new List<FileModel>();
        }

        return files;
    }
}
