using System;

namespace P3Mobility.CloudFileSystem.FileSystem.Exceptions;

public class FileSystemException : Exception
{
    public FileSystemException(string message) : base(message)
    {
    }

    public FileSystemException(string message, Exception e)
        : base(message, e)
    {
    }
}
