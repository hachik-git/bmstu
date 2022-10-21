п»їnamespace TimeMeter;

using System;
using System.Diagnostics;
using System.Drawing;

public class TimeMeter
{
    Stopwatch sw = new Stopwatch();    
    
    public long Measure(Delegate method, int MeasuresQnt, params object[] args)
    {
        sw.Reset();
        sw.Start();
        int i = MeasuresQnt;

        while (i-- > 0)
            method.DynamicInvoke(args);

        sw.Stop();        

        return sw.Elapsed.Ticks / MeasuresQnt;
    }
}
