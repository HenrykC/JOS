using Service.Jira.Models.Repository;

namespace Service.Jira.Logic
{
    public interface IBoardLogic
    {
        Board GetBoardById(int id);
    }
}