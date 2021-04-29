using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("report")]
        public async Task<IActionResult> GetAppointmentDuration(DateTime startDate, DateTime endDate, string userMail)
        {
            var result = await _appointmentLogic.GetAppointmentDuration(startDate, endDate, userMail);
            return Ok(result);
        }
    }
}
