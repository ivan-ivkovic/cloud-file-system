using System;

namespace CloudFileSystem.FileSystem.Files;

public class CreateOrUpdateFileModel
{
    public CreateOrUpdateFileModel()
    {
        this.Name = string.Empty;
    }

    public Guid FolderId { get; set; }

    public string Name { get; set; }
}
