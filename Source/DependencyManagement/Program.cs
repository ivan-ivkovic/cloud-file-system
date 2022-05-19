using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using CloudFileSystem.DependencyManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
{
    var services = builder.Services;
    services.AddCustomServices();
    services.AddControllers();
}

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { }
