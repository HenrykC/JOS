using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Global.Models.Outlook;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Service.Produktteam.Models;

namespace Service.Produktteam.Capacity
{
    public class OutlookWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public OutlookWorker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Thread.Sleep(5000);

            while (true)
            {
                var scheduledTime = DateTime.MinValue;
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var outlookRepository = scope.ServiceProvider.GetRequiredService<IOutlookRepository>();
                    scheduledTime = outlookRepository.LastCapacityMeasure();
                }

                var sleepTime = 0;
                if (DateTime.Now < scheduledTime.AddDays(1))
                {
                    sleepTime = (int)scheduledTime.AddDays(1).Subtract(DateTime.Now).TotalMilliseconds;
                }

                Console.WriteLine("Wait for next cycle: {0:c}", TimeSpan.FromMilliseconds(sleepTime));

                Thread.Sleep(sleepTime);
                await ReadCapacityFromOutlook();
            }
        }

        

        private async Task ReadCapacityFromOutlook()
        {
            var produktteam = new List<string>();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var teamSettings = configuration.GetSection("TeamSettings").Get<List<TeamSetting>>();

                produktteam = teamSettings.SelectMany(s => s.Email).Distinct().ToList();
            }

            var startDate = DateTime.Now.Date.AddDays(-7).ToString("yyyy-MM-dd");
            var endDate = DateTime.Now.Date.AddDays(7).ToString("yyyy-MM-dd") + "T23:59:59.000Z";

            var client = new HttpClient();
            foreach (var member in produktteam)
            {
                try
                {
                    var result = await client.GetFromJsonAsync<List<DailyCapacity>>($"https://localhost:5000/api/Appointment/Capacity/?startDate={startDate}&endDate={endDate}&usermail={member}");

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var outlookRepository = scope.ServiceProvider.GetRequiredService<IOutlookRepository>();
                        outlookRepository.Add(result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
