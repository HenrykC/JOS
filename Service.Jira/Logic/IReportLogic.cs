using System.Collections.Generic;
using Service.Jira.Models;

namespace Service.Jira.Logic
{
    public interface IReportLogic
    {
        List<SprintReport> GetReport(int boardId);
    }
}