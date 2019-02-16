using System.Collections.Generic;
using LoggingService.Models;
using LoggingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.Controllers
{
    [Route("api/log")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;

        private long defaultToken = 199719741969;

        public LogController(LogService logService)
        {
            _logService = logService;
        }

        // Get all DB
        [HttpGet]
        public ActionResult<List<LogModel>> Get([FromHeader] long token)
        {
            if (token == defaultToken)
                return _logService.Get();
            else
                return NoContent();
        }

        // Get by id
        [HttpGet("{id:length(24)}", Name = "GetLog")]
        public ActionResult<LogModel> Get([FromHeader] long token, string id)
        {
            if (token == defaultToken)
            {
                var log = _logService.Get(id);

                if (log == null)
                {
                    return NotFound();
                }

                return log;
            }
            else
                return NoContent();
        }

        // Post item
        [HttpPost]
        public ActionResult<LogModel> Create([FromHeader] long token, LogModel log)
        {
            if (token == defaultToken)
            {
                _logService.Create(log);

                return CreatedAtRoute("GetLog", new { id = log.Id.ToString() }, log);
            }
            else
                return NoContent();     
        }

        // Update item by id
        [HttpPut("{id:length(24)}")]
        public IActionResult Update([FromHeader] long token, string id, LogModel logIn)
        {
            if (token == defaultToken)
            {
                var log = _logService.Get(id);

                if (log == null)
                {
                    return NotFound();
                }

                _logService.Update(id, logIn);   
            }
            return NoContent();
        }

        // Delete item by id
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete([FromHeader] long token, string id)
        {
            if (token == defaultToken)
            {
                var log = _logService.Get(id);

                if (log == null)
                {
                    return NotFound();
                }

                _logService.Remove(log.Id);   
            }
            return NoContent();
        }
    }
}