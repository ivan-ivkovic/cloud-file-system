using System;
using System.Collections.Generic;
using System.Linq;

using P3Mobility.CloudFileSystem.FileSystem.Files;

namespace P3Mobility.CloudFileSystem.FileSystemRepository.Files;

internal class FileRepository : IFileRepository
{
    private List<FileModel> files = new List<FileModel>()
    {
        new FileModel
        {
            Id = Guid.Parse("013b03b8-5787-40c4-889e-adc9e8c605d1"),
            Name = "file1.txt"
        },
        new FileModel
        {
            Id = Guid.Parse("5160cb78-2aef-435e-9cb4-0008bcb9c080"),
            Name = "file2.txt"
        }
    };

    public IEnumerable<FileModel> GetAllFiles()
    {
        return this.files;
    }

    public FileModel GetFileById(Guid id)
    {
        FileModel? file = this.files.Where(x => x.Id == id).FirstOrDefault();
        if (file == null)
        {
            return new EmptyFileModel();
        }

        return file;
    }
}
