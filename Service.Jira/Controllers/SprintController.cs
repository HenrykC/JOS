using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Global.Models.Jira;
using Service.Jira.Models;
using Service.Jira.Logic;


namespace Service.Jira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintController : ControllerBase
    {
        private readonly ISprintLogic _sprintLogic;
        private readonly IIssueLogic _issueLogic;

        public SprintController(ISprintLogic sprintLogic, IIssueLogic issueLogic)
        {
            _sprintLogic = sprintLogic;
            _issueLogic = issueLogic;
        }
        
        [HttpGet("{id}")]
        public Sprint Get(int id)
        {
            return _sprintLogic.GetSprint(id);
        }

        [HttpGet("{id}/issue")]
        public IList<Issue> GetAllIssuesBySprintId(int id)
        {
            return _issueLogic.GetAllIssuesBySprintId(id);
        }
    }
}
