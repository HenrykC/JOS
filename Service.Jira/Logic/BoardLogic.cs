using Service.Jira.Models;
using Service.Jira.Repository;

namespace Service.Jira.Logic
{
    public class BoardLogic : IBoardLogic
    {
        private readonly IBoardRepository _boardRepository;

        public BoardLogic(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public Board GetBoardById(int id)
        {
            return _boardRepository.GetBoardById(id);
        }
    }
}
