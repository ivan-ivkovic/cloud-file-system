using System;
using System.Collections.Generic;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public class FileResponseModel : BaseFileModel
{
    public FileResponseModel()
    {
        this.Name = string.Empty;
        this.AncestorFolderIds = new List<Guid>();
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<Guid> AncestorFolderIds { get; set; }

    public override bool IsEmpty()
    {
        return this.Id == Guid.Empty;
    }
}
