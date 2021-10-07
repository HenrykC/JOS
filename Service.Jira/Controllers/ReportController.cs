using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Jira;
using Service.Jira.Logic;
using Service.Jira.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Service.Jira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportLogic _reportLogic;

        public ReportController(IReportLogic reportLogic)
        {
            _reportLogic = reportLogic;
        }
        [HttpGet("{boardId}/Velocity")]
        public List<SprintReport> GetSprintReports(int boardId, DateTime? startDate, DateTime? endDate)
        {
            return _reportLogic.GetSprintReports(boardId, startDate, endDate);
        }

        [HttpPost("{boardId}/Velocity")]
        public List<SprintReport> GenerateVelocity(int boardId)
        {
            return _reportLogic.GenerateVelocity(boardId);
        }

        [HttpPost("Capacity/{gadgetId}")]
        public bool GenerateCapacity(int gadgetId, [FromBody] string htmlText)
        {
            return _reportLogic.GenerateCapacity(gadgetId, htmlText);
        }
    }
}
