using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggingService.Models;
using LoggingService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.Controllers
{
    [Route("api/diag")]
    [ApiController]
    public class DiagnosticController : ControllerBase
    {
        private readonly LogService _logService;

        public DiagnosticController(LogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public ActionResult<DiagModel> Get()
        {
            var diag = new DiagModel();
            diag.isDbRunning = _logService.Ping();



            return new ActionResult<DiagModel>(diag);
        }
    }
}