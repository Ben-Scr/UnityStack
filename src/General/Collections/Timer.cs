using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenScr.UnityStack
{
    public class Timer
    {
        private Stopwatch stopwatch;

        public Timer()
        {
           stopwatch = Stopwatch.StartNew();
        }

        public string ToString(string preText)
        {
            return $"{preText}{stopwatch.ElapsedMilliseconds} ms";
        }
    }
}
