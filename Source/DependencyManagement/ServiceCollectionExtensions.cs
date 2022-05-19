using Microsoft.Extensions.DependencyInjection;

using CloudFileSystem.FileSystem.Files;
using CloudFileSystem.FileSystem.Folders;
using CloudFileSystem.FileSystemRepository;
using CloudFileSystem.FileSystemRepository.Files;
using CloudFileSystem.FileSystemRepository.Folders;

namespace CloudFileSystem.DependencyManagement;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<FileGetter>();
        services.AddScoped<FileCreator>();
        services.AddScoped<FolderGetter>();
        services.AddScoped<FolderCreator>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFolderRepository, FolderRepository>();
        services.AddDbContext<DatabaseContext>(ServiceLifetime.Scoped);

        return services;
    }
}
