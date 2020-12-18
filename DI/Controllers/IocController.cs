using DI.LogicNs;
using DI.Model;
using Microsoft.AspNetCore.Mvc;

namespace DI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IocController : ControllerBase
    {
        private readonly ILogic logic;

        public IocController(ILogic logic)
        {
            this.logic = logic;
        }


        [HttpGet]
        public IActionResult Get()
        {

            var result = logic.Get();

            return Ok(result);
        }


        [HttpPost]
        public IActionResult Post([FromBody] Ticket ticket)
        {
            if (ticket is null)
            {
                throw new System.ArgumentNullException(nameof(ticket));
            }

            var result = logic.Add(ticket);

            return Ok(result);
        }
    }
}
