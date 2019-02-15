using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingService.Models
{
    public class DiagModel
    {
        public bool IsDbRunning { get; set;}

        public long MbAppUsage { get; set;}

        public double CPUtime { get; set;}
    }
}
