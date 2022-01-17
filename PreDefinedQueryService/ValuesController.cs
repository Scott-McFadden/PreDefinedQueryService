using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreDefinedQueryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreDefinedQueryService
{
    [Route("api/admin")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

       [HttpGet]
       public   IActionResult Index()
        {
            Counters.Inc();

            return Ok(Counters.Results());

        }
    }
}
