using System;

namespace P3Mobility.CloudFileSystem.FileSystem.Files;

public class FileRepositoryException : Exception
{
    public FileRepositoryException(string message, int statusCode = 400) : base(message)
    {
        this.StatusCode = statusCode;
    }

    public FileRepositoryException(string message, Exception e, int statusCode = 400)
        : base(message, e)
    {
        this.StatusCode = statusCode;
    }

    public int StatusCode { get; set; }
}