using System.Collections.Generic;
using Global.Models.Jira;
using Service.Jira.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Jira.Logic;

namespace Service.Jira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IIssueLogic _issueLogic;

        public IssueController(IIssueLogic issueLogic)
        {
            _issueLogic = issueLogic;
        }

        [HttpGet("{key}")]
        public Issue Get(string key)
        {
            return _issueLogic.GetIssue(key);
        }
        
        [HttpGet]
        public IList<Issue> GetIssueByQuery(string jqlQuery)
        {
            return _issueLogic.GetIssuesByJql(jqlQuery);
        }
    }
}
