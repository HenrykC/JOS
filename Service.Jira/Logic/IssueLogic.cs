using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Jira;
using Service.Jira.Models;
using Service.Jira.Repository;

namespace Service.Jira.Logic
{
    public class IssueLogic : IIssueLogic
    {
        private readonly IIssueRepository _issueRepository;

        public IssueLogic(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }
        public Issue GetIssue(string key)
        {
            return _issueRepository.GetIssue(key);
        }

        public IList<Issue> GetAllIssuesBySprintId(int sprintId)
        {
            return _issueRepository.GetAllIssuesBySprintId(sprintId);
        }
    }
}
