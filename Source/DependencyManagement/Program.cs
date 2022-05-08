using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using P3Mobility.CloudFileSystem.DependencyManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
{
    var services = builder.Services;
    services.AddCustomServices();
    services.AddControllers();
}

var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
//     if (dbContext.Folders == null || dbContext.Hierarchies == null)
//     {
//         throw new Exception();
//     }

//     var rootFolderId = Guid.NewGuid();
//     dbContext.Folders.Add(new FolderModel
//     {
//         Id = rootFolderId,
//         FolderName = "/",
//         ParentFolderId = Guid.Empty
//     });
//     dbContext.Hierarchies.Add(new HierarchyModel
//     {
//         Id = Guid.NewGuid(),
//         ParentFolderId = rootFolderId,
//         ChildFolderId = rootFolderId,
//         Depth = 0
//     });
//     dbContext.SaveChanges();
// }

app.MapControllers();

app.Run();

public partial class Program { }
