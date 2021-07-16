using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Produktteam.Capacity;

namespace Service.Produktteam
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    builder.AddUserSecrets<Program>();
                })
                .ConfigureLogging(configureLogging =>
                    configureLogging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information))
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IOutlookRepository, OutlookRepository>();
                    var connectionString = hostContext.Configuration.GetSection("ConnectionString").Value;
                    services.AddDbContext<OutlookDbContext>(options =>
                            options.UseSqlServer(connectionString));

                    services.AddHostedService<OutlookWorker>()
                        .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "Prod.Team Outlook Service";
                            config.SourceName = "Prod.Team Service";
                        });
                })
                .UseWindowsService();
    }
}
