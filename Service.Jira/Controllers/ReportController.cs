using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        // GET: api/<ReportController>
        [HttpGet("{boardId}")]
        public List<SprintReport> Get(int boardId)
        {
            return _reportLogic.GetReport(boardId);
        }
    }
}
