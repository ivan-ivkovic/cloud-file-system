using Microsoft.Extensions.DependencyInjection;
using P3Mobility.CloudFileSystem.FileSystem.Files;
using P3Mobility.CloudFileSystem.FileSystemRepository;
using P3Mobility.CloudFileSystem.FileSystemRepository.Files;

namespace P3Mobility.CloudFileSystem.DependencyManagement;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<FileService>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddDbContext<DatabaseContext>(ServiceLifetime.Scoped);
        return services;
    }
}
