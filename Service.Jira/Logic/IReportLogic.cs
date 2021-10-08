using System;
using System.Collections.Generic;
using Global.Models.Jira;
using Service.Jira.Models;

namespace Service.Jira.Logic
{
    public interface IReportLogic
    {
        List<SprintReport> GetSprintReports(int boardId, DateTime? startDate = null, DateTime? endDate = null);
        List<SprintReport> GenerateSprintHistory(JiraSettings jiraSettings, DateTime? startDate, DateTime? endDate);
        bool GenerateCapacity(int gadgetId, string htmlText);
        bool UpdateGadget(int dashboardId, int gadgetId, string htmlText);
    }
}