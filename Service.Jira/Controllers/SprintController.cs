using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace Service.Jira.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintController : ControllerBase
    {
        // GET: api/<SprintController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SprintController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SprintController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SprintController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SprintController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
