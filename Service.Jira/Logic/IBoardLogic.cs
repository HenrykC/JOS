using Service.Jira.Models;

namespace Service.Jira.Logic
{
    public interface IBoardLogic
    {
        Board GetBoardById(int id);
    }
}