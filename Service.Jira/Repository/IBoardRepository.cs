using Service.Jira.Models.Repository;

namespace Service.Jira.Repository
{
    public interface IBoardRepository
    {
        Board GetBoardById(int id);
    }
}