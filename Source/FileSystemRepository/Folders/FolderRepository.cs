using System;
using System.Linq;

using P3Mobility.CloudFileSystem.FileSystem.Folders;
using P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

namespace P3Mobility.CloudFileSystem.FileSystemRepository.Folders;

internal class FolderRepository : IFolderRepository
{
    private readonly DatabaseContext dbContext;

    public FolderRepository(DatabaseContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public bool FolderExists(Guid folderId)
    {
        if (this.dbContext.Folders == null)
        {
            return false;
        }

        return this.dbContext.Folders.Any(x => x.Id == folderId);
    }

    public bool IsChildFolderNameTaken(Guid parentFolderId, string childFolderName)
    {
        if (this.dbContext.Folders == null || this.dbContext.Hierarchies == null)
        {
            throw new Exception();
        }

        return this.dbContext.Folders.Any(
            x => x.ParentFolderId == parentFolderId && x.FolderName == childFolderName
        );
    }

    public FolderModel? GetFolder(Guid folderId)
    {
        return this.dbContext.Folders?.FirstOrDefault(x => x.Id == folderId);
    }

    public FolderModel CreateFolder(Guid parentFolderId, string folderName)
    {
        if (this.dbContext.Folders == null || this.dbContext.Hierarchies == null)
        {
            throw new Exception();
        }

        var folderModel = new FolderModel
        {
            Id = Guid.NewGuid(),
            ParentFolderId = parentFolderId,
            FolderName = folderName
        };
        this.dbContext.Folders.Add(folderModel);
        this.dbContext.SaveChanges();

        var zeroDepthHierarchyModel = new HierarchyModel
        {
            Id = Guid.NewGuid(),
            ParentFolderId = folderModel.Id,
            ChildFolderId = folderModel.Id,
            Depth = 0
        };
        this.dbContext.Add(zeroDepthHierarchyModel);
        this.dbContext.SaveChanges();

        var query = from p in this.dbContext.Hierarchies
                    from c in this.dbContext.Hierarchies
                    where p.ChildFolderId == parentFolderId && c.ParentFolderId == folderModel.Id
                    select new HierarchyModel
                    {
                        Id = Guid.NewGuid(),
                        ParentFolderId = p.ParentFolderId,
                        ChildFolderId = c.ChildFolderId,
                        Depth = p.Depth + c.Depth + 1
                    };

        this.dbContext.AddRange(query.ToList());
        this.dbContext.SaveChanges();

        return folderModel;
    }
}
