using System.Collections.Generic;
using Global.Models.Jira;
using Service.Jira.Models;

namespace Service.Jira.Repository
{
    public interface IIssueRepository
    {
        Issue GetIssue(string key);
        IList<Issue> GetAllIssuesBySprintId(int sprintId);

        IList<Issue> GetIssuesByJql(string jqlQuery);
    }
}