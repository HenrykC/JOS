using System.Collections.Generic;
using Service.Jira.Models;

namespace Service.Jira.Repository
{
    public interface ISprintRepository
    {
        Sprint GetSprint(int id);
        IList<Sprint> GetAllSprints(int boardId);
    }
}