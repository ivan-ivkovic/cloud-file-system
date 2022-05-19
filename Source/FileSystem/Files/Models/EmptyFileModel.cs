using System;

namespace CloudFileSystem.FileSystem.Files;

internal sealed class EmptyFileResponseModel : FileResponseModel
{
    internal EmptyFileResponseModel()
    {
        this.Id = Guid.Empty;
    }
}
