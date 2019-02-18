using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingService.Models
{
    public class DiagModel
    {
        public bool IsDbRunning { get; set;}

        public long MemoryUsage { get; set;}

        public long MemoryToBeAllocated { get; set; }

        public double CPU { get; set;}

        public int FreeHddPercent { get; set; }

        public DateTime Date { get; set; }
    }
}
