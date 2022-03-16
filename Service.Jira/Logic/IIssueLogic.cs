using System.Collections.Generic;
using Global.Models.Jira;
using Service.Jira.Models;

namespace Service.Jira.Logic
{
    public interface IIssueLogic
    {
        Issue GetIssue(string key);
        IList<Issue> GetAllIssuesBySprintId(int sprintId);
        IList<Issue> GetIssuesByJql(string jqlQuery);
    }
}