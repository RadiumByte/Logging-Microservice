using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggingService.Models;
using LoggingService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Diagnostics;

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
            diag.IsDbRunning = _logService.Ping();

            var proc = Process.GetCurrentProcess();
            diag.MbAppUsage = proc.WorkingSet64 / 1024 / 1024;

            diag.CPUtime = proc.TotalProcessorTime.Milliseconds;

            return new ActionResult<DiagModel>(diag);
        }
    }
}