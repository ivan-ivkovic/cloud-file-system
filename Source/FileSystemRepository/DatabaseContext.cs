using Microsoft.EntityFrameworkCore;

using CloudFileSystem.FileSystemRepository.Files;
using CloudFileSystem.FileSystemRepository.Folders.Models;

namespace CloudFileSystem.FileSystemRepository;

internal class DatabaseContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("TestDb");
    }

    internal DbSet<FileDatabaseModel>? Files { get; set; }
    internal DbSet<FolderDatabaseModel>? Folders { get; set; }
    internal DbSet<HierarchyDatabaseModel>? Hierarchies { get; set; } 
}