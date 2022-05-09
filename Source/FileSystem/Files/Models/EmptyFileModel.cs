using System;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

internal sealed class EmptyFileResponseModel : FileResponseModel
{
    internal EmptyFileResponseModel()
    {
        this.Id = Guid.Empty;
    }
}
