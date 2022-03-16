using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Global.Models;
using Global.Models.Outlook;
using Service.Outlook.Logic;

namespace Service.Outlook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentLogic _appointmentLogic;

        public AppointmentController(IAppointmentLogic appointmentLogic)
        {
            _appointmentLogic = appointmentLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments(DateTime startDate, DateTime endDate)
        {
            var result = await _appointmentLogic.GetAppointments(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("Capacity")]
        public async Task<IActionResult> GetAppointmentDuration(DateTime startDate, DateTime endDate, string userMail)
        {
            var result = await _appointmentLogic.GetAppointmentDuration(startDate, endDate, userMail);
            return Ok(result);
        }

        [HttpPost("Capacity")]
        public async Task<IActionResult> GenerateCapacity(DateTime startDate, DateTime endDate, List<string> emailAddresses)
        {
            var teamCapacity = new List<PersonalCapacity>();

            foreach (var member in emailAddresses)
            {
                var dailyCapacities = await _appointmentLogic.GetAppointmentDuration(startDate, endDate, member);
                var personalCapacity = new PersonalCapacity();
                
                personalCapacity.Capacity = dailyCapacities;
                personalCapacity.SprintCapacity = 8 * personalCapacity.Capacity.Count;
                personalCapacity.HandsOnKeyboard = personalCapacity.Capacity.Sum(s => s.Capacity);
                personalCapacity.UserName = member.Contains('@') ? member.Split("@")[0] : member;

                teamCapacity.Add(personalCapacity);
            }

            string html = "\"<table border=1'>" +
                          "<tbody>" +
                          "<tr>" +
                          "<td>Name</td><td>HOK h</td><td>HOK %</td>";
            if (teamCapacity.Count == 0)
            {
                return BadRequest();
            }

            foreach (var date in teamCapacity.FirstOrDefault()?.Capacity.Select(s => s.Date))
            {
                html += $"<td>{date.Day}.{date.Month}</td>";
            }

            html += "</tr>";
            foreach (var personalCapacity in teamCapacity)
            {
                html += GenerateLine(personalCapacity);
            }
            html += "</table>\"";

            var client = new HttpClient();
            var result = await client.PostAsync(Endpoints.Jira.Capacity, new StringContent(html, Encoding.UTF8, "application/json"));

            result.EnsureSuccessStatusCode();

            return Ok();
        }

        private string GenerateLine(PersonalCapacity capacity)
        {
            var html = $"<tr><td>{capacity.UserName}</td><td>{capacity.HandsOnKeyboard}</td><td>{Math.Round(capacity.HandsOnKeyboard * 100.0 / 40.0)}%</td>";
            foreach (var entry in capacity.Capacity)
            {
                html += $"<td>{entry.Capacity}h</td>";
            }
            html += "</tr>";
            return html;
        }
    }
}
