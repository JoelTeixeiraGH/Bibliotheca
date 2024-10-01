using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GroupLibraryManagment.Webjobs
{
    class Program
    {
        static async Task Main()
        {
            var builder = new HostBuilder();
            builder.ConfigureWebJobs(b =>
            {
                //b.AddAzureStorageCoreServices();
                b.AddAzureStorageQueues();
                b.AddTimers();
            });
            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
            });
            builder.ConfigureHostConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                // Add other configuration sources as needed
            });
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<GroupLibraryManagmentDbContext>(options =>
                {
                    options.UseSqlServer(context.Configuration.GetConnectionString("DatabaseCs"));
                });
                services.AddTransient<Functions>();
            });
            //builder.UseEnvironment(EnvironmentName.Development);
            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }

        }
    }
}