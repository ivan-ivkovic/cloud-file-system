using Microsoft.Extensions.DependencyInjection;

using P3Mobility.CloudFileSystem.FileSystem.Files;
using P3Mobility.CloudFileSystem.FileSystem.Folders;
using P3Mobility.CloudFileSystem.FileSystemRepository;
using P3Mobility.CloudFileSystem.FileSystemRepository.Files;
using P3Mobility.CloudFileSystem.FileSystemRepository.Folders;

namespace P3Mobility.CloudFileSystem.DependencyManagement;

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
