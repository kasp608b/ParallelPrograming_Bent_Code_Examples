using System.Diagnostics;

namespace TimeMeasurer
{
    public class TimeMeasurer
    {
        public static double MeasureTimeInSeconds(Action ac)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ac.Invoke();
            sw.Stop();
            return sw.Elapsed.TotalSeconds;
        }
    }
}