using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Jira.Logic;
using Service.Jira.Models.Repository;

namespace Service.Jira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardLogic boardLogic;

        public BoardController(IBoardLogic boardLogic)
        {
            this.boardLogic = boardLogic;
        }


        [HttpGet("{id}")]
        public Board GetBoardById(int id)
        {
            return boardLogic.GetBoardById(id);
        }

    }
}
