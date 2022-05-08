using Microsoft.EntityFrameworkCore;

using P3Mobility.CloudFileSystem.FileSystemRepository.Files;
using P3Mobility.CloudFileSystem.FileSystemRepository.Folders.Models;

namespace P3Mobility.CloudFileSystem.FileSystemRepository;

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