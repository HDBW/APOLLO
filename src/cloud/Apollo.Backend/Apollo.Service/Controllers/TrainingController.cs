using Apollo.Service.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Apollo.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        // GET: api/<TrainingController>
        [HttpGet]
        public IEnumerable<Training> Get()
        {
            return new Training[0];
        }

        // GET api/<TrainingController>/5
        [HttpGet("{id}")]
        public Training Get(int id)
        {
            return new Training();
        }

        // POST api/<TrainingController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TrainingController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TrainingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
