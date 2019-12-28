using System;
using System.Diagnostics;

namespace ExcelWorkerApp.Utility
{
    class ConsoleWatch
    {
        Stopwatch diffWatch;
        Stopwatch stampWatch;
        public string ProgramId { get; set; }

        public ConsoleWatch(string programId)
        {
            diffWatch = new Stopwatch();
            stampWatch = new Stopwatch();
            this.ProgramId = programId;
        }

        public void StartAll()
        {
            this.diffWatch.Reset();
            this.stampWatch.Reset();
            this.diffWatch.Start();
            this.stampWatch.Start();
        }

        public void StopAll()
        {
            this.diffWatch.Stop();
            this.stampWatch.Stop();
        }

        public void PrintTime(string message)
        {
            TimeSpan ts = stampWatch.Elapsed;
            var elapsedTime = String.Format("o [{0:00}:{1:00}:{2:00}.{3:00}] [{4}] {5}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10, this.ProgramId, message);
            Console.WriteLine(elapsedTime);
        }

        public void PrintDiff(string message)
        {
            diffWatch.Stop();
            TimeSpan ts = diffWatch.Elapsed;
            var elapsedTime = String.Format("- [{0:00}:{1:00}:{2:00}.{3:00}] [{4}] {5}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10, this.ProgramId, message);
            Console.WriteLine(elapsedTime);
            diffWatch.Restart();
        }

        public void PrintAll(string message)
        {
            this.PrintTime(message);
            this.PrintDiff(message);
        }
    }
}
