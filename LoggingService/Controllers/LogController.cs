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

        public LogController(LogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public ActionResult<List<LogModel>> Get()
        {
            return _logService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetLog")]
        public ActionResult<LogModel> Get(string id)
        {
            var log = _logService.Get(id);

            if (log == null)
            {
                return NotFound();
            }

            return log;
        }

        [HttpPost]
        public ActionResult<LogModel> Create(LogModel log)
        {
            _logService.Create(log);

            return CreatedAtRoute("GetLog", new { id = log.Id.ToString() }, log);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, LogModel logIn)
        {
            var log = _logService.Get(id);

            if (log == null)
            {
                return NotFound();
            }

            _logService.Update(id, logIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var log = _logService.Get(id);

            if (log == null)
            {
                return NotFound();
            }

            _logService.Remove(log.Id);

            return NoContent();
        }
    }
}