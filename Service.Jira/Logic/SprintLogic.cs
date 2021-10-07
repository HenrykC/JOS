using System.Collections.Generic;
using Global.Models.Jira;
using Service.Jira.Models;
using Service.Jira.Repository;

namespace Service.Jira.Logic
{
    public class SprintLogic : ISprintLogic
    {
        private readonly ISprintRepository _sprintRepository;

        public SprintLogic(ISprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public Sprint GetSprint(int id)
        {
            return _sprintRepository.GetSprint(id);
        }

        public IList<Sprint> GetAllSprints(int boardId)
        {
            return _sprintRepository.GetAllSprints(boardId);
        }
    }
}
