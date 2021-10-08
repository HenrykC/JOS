using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Global.Models.Jira;
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

            //while (true)
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

                // Thread.Sleep(sleepTime);
                await ReadCapacityFromOutlook();
                await WriteCapacityToJiraDashboard();
                await WriteCapacityToJiraDashboard(true);

                try
                {
                    await WriteSprintSummaryToJiraDashboard();
                    await WriteVelocityToJiraDashboard();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private async Task WriteVelocityToJiraDashboard()
        {
            var teamSettings = GetTeamSettings().Where(setting => setting.EndDate == null && setting.CreateReport).ToList();

            foreach (var teamSetting in teamSettings)
            {
                var client = new HttpClient();
                var startDate = teamSetting.StartDate.ToString("s");
                var result = await client.GetFromJsonAsync<List<SprintReport>>($"https://localhost:6000/api/report/57/Velocity?startDate={startDate}");
                result = result.Where(w => w.Name.ToLower().Contains(teamSetting.JiraSettings.FilterSprintName.ToLower()))
                                .ToList();

                var sprintSuccess = result.Count(s => s.Success == true);
                var sprintFailed = result.Count(s => s.Success == false);
                var velocityFull = Math.Round(result.Sum(s => s.Velocity) / result.Count, 2);
                var velocity4Sprints = Math.Round(
                    result.Where(w =>
                            w.StartDate.CompareTo(DateTime.Now.AddDays(-1 * teamSetting.SprintLength * 5)) >= 0)
                        .Where(w => w.Name.ToLower().Contains(teamSetting.JiraSettings.FilterSprintName.ToLower()))
                        .Sum(s => s.Velocity)
                    / (result.Count < 4 ? result.Count : 4), 2);

                var velocity12Sprints = Math.Round(
                    result.Where(w =>
                            w.StartDate.CompareTo(DateTime.Now.AddDays(-1 * teamSetting.SprintLength * 13)) >= 0)
                        .Where(w => w.Name.ToLower().Contains(teamSetting.JiraSettings.FilterSprintName.ToLower()))
                        .Sum(s => s.Velocity)
                    / (result.Count < 12 ? result.Count : 4), 2);

                var html = $@"
                                <meta http-equiv=""Content-Type"" content=""text/html; charset=ISO-8859-1""> 

                                <svg id=""statSvg"" xmlns=""http://www.w3.org/2000/svg"" width=""350"" height=""50""> 
                                    <text x=""10"" y=""15"" font-size=""14"" font-family=""Arial"" fill=""#404040"">Sprintziel erfüllt</text> 
                                    <rect x=""120"" y=""0"" width=""{2 * (sprintSuccess * 100 / result.Count)}"" height=""20"" rx=""3"" ry=""3"" fill=""#90EE90""></rect> 
                                    <rect x=""{120 + 2 * (sprintSuccess * 100 / result.Count)}"" y=""0"" width=""{2 * (sprintFailed * 100 / result.Count)}"" height=""20"" rx=""3"" ry=""3"" fill=""#FA8072""></rect>";
                if (sprintSuccess > 0)
                    html += $@"<text x=""{120 + (sprintSuccess * 100 / result.Count)}"" y=""15"" font-size=""12"" font-family=""Arial"" fill=""#404040"">{sprintSuccess}</text>";
                if (sprintFailed > 0)
                    html += $@"<text x=""{120 + 2 * (sprintSuccess * 100 / result.Count) + sprintFailed * 100 / result.Count}"" y=""15"" font-size=""12"" font-family=""Arial"" fill=""#404040"">{sprintFailed}</text>";

                html +=
                    $@"</svg><br>
                                Velocity 4 Wochen : {velocity4Sprints} <br>
                                Velocity 12 Wochen :  {velocity12Sprints}<br>
                                Velocity Gesamt : {velocityFull}<br>";

                var queryResult = await client.PostAsync($"https://localhost:6000/api/report/gadget/{teamSetting.JiraSettings.DashboardId}/{teamSetting.JiraSettings.VelocityGadgetId}",
                    new StringContent(JsonConvert.SerializeObject(html), Encoding.UTF8, "application/json"));

                queryResult.EnsureSuccessStatusCode();
            }
        }

        private async Task WriteSprintSummaryToJiraDashboard()
        {
            var teamSettings = GetTeamSettings().Where(setting => setting.EndDate == null && setting.CreateReport).ToList();

            foreach (var teamSetting in teamSettings)
            {
                var client = new HttpClient();
                var startDate = teamSetting.StartDate.ToString("s");
                var result = await client.PostAsync($"https://localhost:6000/api/report/SprintHistory?startDate={startDate}",
                    new StringContent(JsonConvert.SerializeObject(teamSetting.JiraSettings), Encoding.UTF8, "application/json"));
            }
        }

        private List<TeamSetting> GetTeamSettings()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var teamSettings = configuration.GetSection("TeamSettings")
                .Get<List<TeamSetting>>()
                .ToList();

            return teamSettings;
        }

        private async Task WriteCapacityToJiraDashboard(bool calculateNextSprint = false)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var teamSettings = GetTeamSettings().Where(setting => setting.EndDate == null && setting.CreateReport).ToList();


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
                var gadgetId = calculateNextSprint
                                        ? teamSetting.JiraSettings.NextSprintGadgetId
                                        : teamSetting.JiraSettings.ActualSprintGadgetId;
                var result = await client.PostAsync($"https://localhost:6000/api/report/Gadget/{teamSetting.JiraSettings.DashboardId}/{gadgetId}", new StringContent(html, Encoding.UTF8, "application/json"));

                result.EnsureSuccessStatusCode();
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
            var teamSettings = GetTeamSettings();

            produktteam = teamSettings
                .Where(setting => setting.EndDate == null)
                .SelectMany(s => s.Email)
                .Distinct()
                .ToList();


            var startDate = DateTime.Now.Date.AddDays(-7).ToString("yyyy-MM-dd");
            var endDate = DateTime.Now.Date.AddDays(15).ToString("yyyy-MM-dd") + "T23:59:59.000Z";

            var client = new HttpClient();

            var measureTimeStamp = DateTime.Now.Date
                                        .AddHours(DateTime.Now.Hour)
                                        .AddMinutes(DateTime.Now.Minute)
                                        .AddSeconds(DateTime.Now.Second);
            foreach (var member in produktteam)
            {
                try
                {
                    var result = await client.GetFromJsonAsync<List<DailyCapacity>>($"https://localhost:5000/api/Appointment/Capacity/?startDate={startDate}&endDate={endDate}&usermail={member}");
                    result.ForEach(r => r.MeasureTimeStamp = measureTimeStamp);
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
