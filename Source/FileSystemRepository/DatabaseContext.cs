using Microsoft.EntityFrameworkCore;
using P3Mobility.CloudFileSystem.FileSystem.Files;
using P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

namespace P3Mobility.CloudFileSystem.FileSystemRepository;

internal class DatabaseContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("TestDb");
    }

    internal DbSet<FileModel>? Files { get; set; }
    internal DbSet<FolderModel>? Folders { get; set; }
    internal DbSet<HierarchyModel>? Hierarchies { get; set; } 
}