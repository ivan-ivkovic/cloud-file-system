using System;
namespace P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

public class HierarchyModel
{
    public Guid Id { get; set; }

    public Guid ParentFolderId { get; set; }

    public Guid ChildFolderId { get; set; }

    public int Depth { get; set; }
}
