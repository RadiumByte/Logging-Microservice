using System.Collections.Generic;
using LoggingService.Models;
using LoggingService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System;

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

        /// <summary>
        /// Gets a LogItem by parameter filtering.
        /// Filter structure: [type]$[user]$[sender]$[time].
        /// There [time] is [hour], or [day], or [month], or [year], or []
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /log/param:telemetry_msg$Robocar$Autopilot$hour
        ///
        /// </remarks>
        /// <param name="token"></param>
        /// <param name="parameters"></param>
        /// <returns>List of LogItem</returns>
        /// <response code="400">If token is invalid</response> 
        [HttpGet]
        [ProducesResponseType(400)]
        [Route("param:{parameters}", Name = "GetByParameters")]
        public ActionResult<List<LogModel>> GetByParameters([FromHeader] long token, string parameters)
        {
            if (token == defaultToken)
                return _logService.GetByParameters(parameters);
            else
            {
                Thread.Sleep(1000);
                return BadRequest();
            }         
        }

        /// <summary>
        /// Gets a LogItem by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /log/id:5c69a7adc8c48004c81c7d40
        ///
        /// </remarks>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>LogItem</returns>
        /// <response code="400">If the token is invalid</response>
        /// <response code="404">If the item is null</response> 
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
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
                return BadRequest();
            }
        }

        /// <summary>
        /// Creates a LogItem in MongoDB.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /log
        ///     {
        ///        "date": "2018-09-15 20:24:48",
        ///        "type": "telemetry_msg",
        ///        "userName": "Robocar",
        ///        "senderApp": "Autopilot v1.2",
        ///        "bInfo": "Car movement",
        ///        "dInfo": "LEFT 15, FORWARD 20, RIGHT 20"
        ///     }
        ///
        /// </remarks>
        /// <param name="token"></param>
        /// <param name="log"></param>
        /// <returns>A newly created LogItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null OR token is invalid</response>       
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<LogModel> Create([FromHeader] long token, LogModel log)
        {
            if (log == null || token != defaultToken)
            {
                Thread.Sleep(1000);
                return BadRequest();
            }
            else
            {
                _logService.Create(log);
                return Created(log.Id.ToString(), log);
            }
        }

        /// <summary>
        /// Deletes a LogItem in MongoDB by item's id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /log/id:5c69a7adc8c48004c81c7d40
        ///
        /// </remarks>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>204 NoContent response</returns>
        /// <response code="204">After deleting of item</response>
        /// <response code="400">If token is invalid</response>    
        [HttpDelete("id:{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Delete([FromHeader] long token, string id)
        {
            if (token == defaultToken)
            {
                _logService.Remove(id);
                return NoContent();
            }
            else
                return BadRequest();
        }      
    }
}