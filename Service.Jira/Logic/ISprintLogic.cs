using System.Collections.Generic;
using Global.Models.Jira;
using Service.Jira.Models;

namespace Service.Jira.Logic
{
    public interface ISprintLogic
    {
        Sprint GetSprint(int id);
        IList<Sprint> GetAllSprints(int boardId);
    }
}