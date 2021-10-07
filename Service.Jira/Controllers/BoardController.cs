using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Jira;
using Service.Jira.Logic;
using Service.Jira.Models;
using Service.Jira.Models.Repository;

namespace Service.Jira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardLogic _boardLogic;
        private readonly ISprintLogic _sprintLogic;

        public BoardController(IBoardLogic boardLogic, ISprintLogic sprintLogic)
        {
            this._boardLogic = boardLogic;
            _sprintLogic = sprintLogic;
        }

        [HttpGet("{id}")]
        public Board GetBoardById(int id)
        {
            return _boardLogic.GetBoardById(id);
        }

        [HttpGet("{id}/sprint")]
        public IList<Sprint> GetAllSprintsByBoardId(int id)
        {
            return _sprintLogic.GetAllSprints(id);
        }
    }
}
