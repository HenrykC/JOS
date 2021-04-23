using Service.Jira.Models;

namespace Service.Jira.Repository
{
    public interface IBoardRepository
    {
        Board GetBoardById(int id);
    }
}