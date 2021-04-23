using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Service.Jira.Logic;
using Service.Jira.Models;


namespace Service.Jira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintController : ControllerBase
    {
        private readonly ISprintLogic _sprintLogic;

        public SprintController(ISprintLogic sprintLogic)
        {
            _sprintLogic = sprintLogic;
        }
        
        [HttpGet("{id}")]
        public Sprint Get(int id)
        {
            return _sprintLogic.GetSprint(id);
        }
    }
}
