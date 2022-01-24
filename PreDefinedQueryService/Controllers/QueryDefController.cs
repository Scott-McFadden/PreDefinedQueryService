using DomainOps;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PreDefinedQueryService.Controllers
{
    [Route("api/QueryDef")]
    [ApiController]
    public class QueryDefController : ControllerBase
    {
        ExecuteQuery data;
        public QueryDefController()
        {
            data = new(); 
        }
        // GET: api/<QueryDefController>
        [HttpGet]
        public string Get()
        {
             
            return data.Get("QueryDef").ToString();
        }

        // GET api/<QueryDefController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<QueryDefController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<QueryDefController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QueryDefController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
