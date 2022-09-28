п»їnamespace TimeMeter;

using System;
using System.Diagnostics;

public class TimeMeter
{
    Stopwatch sw = new Stopwatch();

    public long Measure(Delegate method, int MeasuresQnt, params object[] args)
    {
        sw.Reset();
        sw.Start();

        while (MeasuresQnt-- > 0)
            method.DynamicInvoke(args);

        sw.Stop();        

        return sw.Elapsed.Ticks;
    }
}
