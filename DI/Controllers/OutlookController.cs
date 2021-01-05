using DI.Outlook.Logic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OutlookController : ControllerBase
    {
        private readonly IOutlookLogic outlookLogic;

        public OutlookController(IOutlookLogic outlookLogic)
        {
            this.outlookLogic = outlookLogic;
        }

        [HttpPatch]
        public async Task<IActionResult> GetLastWeek()
        {
            var result = await outlookLogic.GetLastWeek();

            return Ok(result);

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = outlookLogic.GetAllAppointments();

            return Ok(result);

        }

    }
}
