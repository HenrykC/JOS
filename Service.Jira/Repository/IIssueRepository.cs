using System.Collections.Generic;
using Service.Jira.Models;

namespace Service.Jira.Repository
{
    public interface IIssueRepository
    {
        Issue GetIssue(string key);
        IList<Issue> GetAllIssuesBySprintId(int sprintId);
    }
}