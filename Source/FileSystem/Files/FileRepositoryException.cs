using System;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public class FileRepositoryException : Exception
{
    public FileRepositoryException(string message, int statusCode = 400) : base(message)
    {
    }

    public FileRepositoryException(string message, Exception e, int statusCode = 400)
        : base(message, e)
    {
    }
}