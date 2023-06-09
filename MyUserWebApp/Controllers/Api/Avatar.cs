using Microsoft.AspNetCore.Mvc;
using MyUserWebApp.MyRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyUserWebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class Avatar : ControllerBase
    {
        private readonly CRUD_Repository cRUD;
        public Avatar(CRUD_Repository _cRUD)
        {
            cRUD= _cRUD;
        }
        // GET: api/<Avatar>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Avatar>/5
        [HttpGet("{name}")]
        public string Get(string name)
        {
            return cRUD.Avatar(name);
        }

        // POST api/<Avatar>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Avatar>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Avatar>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
