using System.Collections.Generic;
using LoggingService.Models;
using LoggingService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace LoggingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;

        private readonly long defaultToken = 199719741969;

        public LogController(LogService logService)
        {
            _logService = logService;
        }

        // Get by parameter filtering:
        // Route - /log/<type>&<user>&<sender>&<time>
        // <time> is <hour>, <day>, <month>, <year>, or <>

        [HttpGet]
        [Route("param:{parameters}", Name = "GetByParameters")]
        public ActionResult<List<LogModel>> GetByParameters([FromHeader] long token, string parameters)
        {
            if (token == defaultToken)
                return _logService.GetByParameters(parameters);
            else
            {
                Thread.Sleep(1000);
                return NoContent();
            }         
        }

        // Get by id
        [HttpGet]
        [Route("id:{id:length(24)}", Name = "GetById")]
        public ActionResult<LogModel> GetById([FromHeader] long token, string id)
        {
            if (token == defaultToken)
            {
                var log = _logService.GetById(id);

                if (log == null)
                    return NotFound();

                return log;
            }
            else
            {
                Thread.Sleep(1000);
                return NoContent();
            }         
        }

        // Post item
        [HttpPost]
        public ActionResult<LogModel> Create([FromHeader] long token, LogModel log)
        {
            if (token == defaultToken)
            {
                _logService.Create(log);
                return Created(log.Id.ToString(), log);
            }
            else
            {
                Thread.Sleep(1000);
                return NoContent();
            }
        }

        // Delete item by id
        [HttpDelete("id:{id:length(24)}")]
        public IActionResult Delete([FromHeader] long token, string id)
        {
            if (token == defaultToken)
            {
                _logService.Remove(id);   
            }
            return NoContent();
        }      
    }
}