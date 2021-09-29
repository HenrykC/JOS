using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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

            // while (true)
            {
                var scheduledTime = DateTime.MinValue;
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var outlookRepository = scope.ServiceProvider.GetRequiredService<IOutlookRepository>();
                    scheduledTime = outlookRepository.LastCapacityMeasure();
                }

                var sleepTime = 0;
                var nextSchedule = new TimeSpan(1, 9, 0, 0);
                var timeToWait = nextSchedule - DateTime.Now.AddDays(1).TimeOfDay;

                if (timeToWait > TimeSpan.Zero)
                {
                    sleepTime = (int)timeToWait.TotalMilliseconds;
                }

                Console.WriteLine("Wait for next cycle: {0:c}", TimeSpan.FromMilliseconds(sleepTime));
                var runAt = DateTime.Now.AddMilliseconds(sleepTime).ToString("dddd, HH:mm");
                Console.WriteLine($"Run next cycle: {runAt} Uhr");

                //Thread.Sleep(sleepTime);
                //await ReadCapacityFromOutlook();
                await WriteCapacityToJiraDashboard();
                await WriteCapacityToJiraDashboard(true);

            }
        }

        private async Task WriteCapacityToJiraDashboard(bool calculateNextSprint = false)
        {
            var teamSettings = new List<TeamSetting>();

            using var scope = _serviceScopeFactory.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            teamSettings = configuration.GetSection("TeamSettings")
                .Get<List<TeamSetting>>()
                .Where(setting => setting.EndDate == null && setting.CreateReport)
                .ToList();


            foreach (var teamSetting in teamSettings)
            {
                string html =
                    "\"<style>.tableone {border-collapse: collapse;font-family: Arial;font-size: 11pt;}.tableone td {text-align: left;padding: 7px;width: 150px;}.tableone tr:nth-child(odd) {background-color: #D9E1F2;}.tableone tr:nth-child(even) {background-color: #FFFFFF;}.tableone th {text-align: left;padding: 7px;font-weight: bold;color: #FFFFFF;background-color: #4472C4;}</style>";


                html += "<table class='tableone'>" +
                              "<tbody>" +
                              "<tr>" +
                              "<th>Team</th><th>Sprint</th><th>HOK h</th><th>HOK %</th>";

                var nextSprintStart = GetNextWeekday(DateTime.Now, teamSetting.StartDate.DayOfWeek);
                if (!calculateNextSprint) nextSprintStart = nextSprintStart.AddDays(-1 * teamSetting.SprintLength);

                var outlookRepository = scope.ServiceProvider.GetRequiredService<IOutlookRepository>();
                var capacity = outlookRepository.GetCapacity(teamSetting.Email, nextSprintStart, nextSprintStart.AddDays(teamSetting.SprintLength - 1));

                html += "<tr>" +
                        $"<td><b>{teamSetting.TeamName}</b></td><td>{nextSprintStart.ToShortDateString()}</td><td>{capacity} h</td><td>{Math.Round(capacity * 100.0 / (40.0 * teamSetting.Email.Count))} %</td></tr>";

                html += "</table>";
                html = AddCapacityDetails(scope, teamSetting, nextSprintStart, html);

                html += "\"";
                var client = new HttpClient();

                try
                {
                    var gadgetId = calculateNextSprint
                        ? teamSetting.NextSprintGadgetId
                        : teamSetting.ActualSprintGadgetId;
                    var result = await client.PostAsync($"https://localhost:6000/api/report/Capacity/{gadgetId}", new StringContent(html, Encoding.UTF8, "application/json"));

                    result.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {


                }


            }
        }

        private string AddCapacityDetails(IServiceScope scope, TeamSetting teamSetting, DateTime nextSprintStart, string html)
        {
            var outlookRepository = scope.ServiceProvider.GetRequiredService<IOutlookRepository>();
            html += "<br>";
            html += "<table class='tableone'>" +
                        "<tbody>" +
                          "<tr>" +
                          "<th>Name</th><th>HOK h</th><th>%</th>";

            var generateHeader = true;
            foreach (var user in teamSetting.Email)
            {
                var dailyCapacities = outlookRepository.GetDailyCapacity(user, nextSprintStart,
                    nextSprintStart.AddDays(teamSetting.SprintLength - 1));

                if (generateHeader)
                {
                    html = dailyCapacities.Select(s => s.Date).Aggregate(html, (current, date) => current + $"<th>{date.Day}.{date.Month}</th>");
                    html += "</tr>";
                    generateHeader = false;
                }

                html += $"<tr><td>{dailyCapacities.FirstOrDefault().UserName.Split('@')[0] ?? ""}</td><td>{dailyCapacities.Sum(s => s.Capacity)}</td><td>{Math.Round((dailyCapacities.Sum(s => s.Capacity)) * 100.0 / 40.0)}%</td>";
                foreach (var entry in dailyCapacities)
                {
                    if (entry.Capacity > 2)
                        html += $"<td>{entry.Capacity}h</td>";
                    else
                        html += $"<td bgcolor='pink''>{entry.Capacity}h</td>";
                }
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }


        private static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            var daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            if (daysToAdd == 0) daysToAdd += 7; //sprintWechsel
            return start.AddDays(daysToAdd);
        }

        private async Task ReadCapacityFromOutlook()
        {
            var produktteam = new List<string>();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var teamSettings = configuration.GetSection("TeamSettings").Get<List<TeamSetting>>();

                produktteam = teamSettings
                    .Where(setting => setting.EndDate == null)
                    .SelectMany(s => s.Email)
                    .Distinct()
                    .ToList();
            }

            var startDate = DateTime.Now.Date.AddDays(-7).ToString("yyyy-MM-dd");
            var endDate = DateTime.Now.Date.AddDays(15).ToString("yyyy-MM-dd") + "T23:59:59.000Z";

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
