using System;
namespace CloudFileSystem.FileSystemRepository.Folders.Models;

/// <summary>
/// SQL Closue Tables
/// </summary>
public class HierarchyDatabaseModel
{
    public Guid Id { get; set; }

    public Guid ParentFolderId { get; set; }

    public Guid ChildFolderId { get; set; }

    public int Depth { get; set; }
}
