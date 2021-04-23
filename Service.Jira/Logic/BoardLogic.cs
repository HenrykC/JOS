using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Jira.Models.Repository;
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
