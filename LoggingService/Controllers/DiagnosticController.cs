using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggingService.Models;
using LoggingService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;


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

        [NonAction]
        public int CheckDiskSpace(string driveLetter)
        {
            DriveInfo drive = new DriveInfo(driveLetter);

            var totalBytes = drive.TotalSize;
            var freeBytes = drive.AvailableFreeSpace;

            var freePercent = (int)((100 * freeBytes) / totalBytes);

            return freePercent;
        }

        [HttpGet]
        public ActionResult<DiagModel> Get()
        {
            var diag = new DiagModel();
            diag.IsDbRunning = _logService.Ping();

            var proc = Process.GetCurrentProcess();
            diag.MemoryUsage = proc.WorkingSet64 / 1024 / 1024;

            diag.CPU = proc.TotalProcessorTime.Milliseconds;

            diag.FreeHddPercent = CheckDiskSpace(System.AppContext.BaseDirectory);

            diag.MemoryToBeAllocated = GC.GetTotalMemory(true) / 1024 / 1024;
            diag.Date = DateTime.Now;

            diag.AverageRequestTime = _logService.GetRequestAverageTime();

            return new ActionResult<DiagModel>(diag);
        }
    }
}