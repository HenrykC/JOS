﻿using System.Collections.Generic;
using Service.Jira.Models;

namespace Service.Jira.Logic
{
    public interface IIssueLogic
    {
        Issue GetIssue(string key);
        IList<Issue> GetAllIssuesBySprintId(int sprintId);
    }
}