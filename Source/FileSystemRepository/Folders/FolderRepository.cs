using System.Runtime.CompilerServices;
using System;
using System.Linq;

using P3Mobility.CloudFileSystem.FileSystem.Folders;
using P3Mobility.CloudFileSystem.FileSystem.Folders.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace P3Mobility.CloudFileSystem.FileSystemRepository.Folders;

internal class FolderRepository : IFolderRepository
{
    private readonly DatabaseContext dbContext;
    private readonly Guid rootFolderId;

    public FolderRepository(DatabaseContext dbContext, IConfiguration configuration)
    {
        this.dbContext = dbContext;
        this.rootFolderId = Guid.Parse(configuration["RootFolderId"]);
    }

    public bool FolderExists(Guid folderId)
    {
        if (this.dbContext.Folders == null)
        {
            return false;
        }

        if (folderId == this.rootFolderId)
        {
            return true;
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

    public IEnumerable<Guid> GetFolderAncestorIds(Guid folderId)
    {
        if (this.dbContext.Hierarchies == null)
        {
            throw new Exception();
        }

        return this.dbContext.Hierarchies
            .Where(x => x.ChildFolderId == folderId && x.ParentFolderId != x.ChildFolderId)
            .OrderBy(x => x.Depth)
            .Select(x => x.ParentFolderId)
            .ToList();
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
