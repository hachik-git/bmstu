п»їnamespace Algorithm.Levenstein;

using System;
using System.Diagnostics;

public enum CalcMethod
{
    Matrix,
    Recursive,
    RecursiveCashed,
    Domerau,
}

public class Levenstein
{
    private int Min(params int[] values)
    {
        int min = values[0];
        foreach (var i in values)
            if (i < min)
                min = i;
        return min;
    }
    public string s1 { get; private set; }
    public string s2 { get; private set; }
    public int l1 { get; private set; }
    public int l2 { get; private set; }
    
    public Levenstein(string s1, string s2)
    {
        this.s1 = s1;
        this.s2 = s2;
        l1 = s1?.Length ?? 0;
        l2 = s2?.Length ?? 0;
    }
    
    public int[,] Matrix
    {
        get
        {
            var m = new int[l1 + 1, l2 + 1];

            for (int i = 0; i <= l1; i++)
                for (int j = 0; j <= l2; j++)
                {
                    if (i * j == 0)
                        m[i, j] = i + j;
                    else
                        m[i, j] =
                            Min(m[i, j - 1] + 1,
                                m[i - 1, j] + 1,
                                m[i - 1, j - 1] + (s1?[i - 1] == s2?[j - 1] ? 0 : 1));
                }       

            return m;
        }
    }

    public char[] Path
    {
        get
        {
            var m = Matrix;
            (int r, int c) = (s1?.Length ?? 0, s2?.Length ?? 0);

            List<char> o = new List<char>();

            int d, i, rm;

            while (r + c > 0)
            {
                if (c == 0)
                {
                    o.Add('D');
                    r--;
                }
                else if (r == 0)
                {
                    o.Add('I');
                    c--;
                }
                else
                {
                    rm = m[r - 1, c - 1];
                    i = m[r, c - 1];
                    d = m[r - 1, c];

                    if (rm <= Math.Min(i, d))
                    {
                        if (rm < m[r, c])
                            o.Add('R');
                        else
                            o.Add('M');

                        c--;
                        r--;
                    }
                    else if (i <= d)
                    {
                        o.Add('I');
                        c--;
                    }
                    else
                    {
                        o.Add('D');
                        r--;
                    }
                }
            }

            o.Reverse();
            return o.ToArray();
        }
    }
    private int DM()
    {
        if (l1+l2 == 0)
            return l1+l2;

        (int rc, int cc) = (l1 + 1, l2 + 1);
        (var prev_row, var cur_row) = (new int[cc], new int[cc]);

        for (int r = 0; r < rc; r++)
        {
            for (int c = 0; c < cc; c++)
            {
                if (r * c == 0)
                    cur_row[c] = r+c;
                else
                    cur_row[c] = Min(prev_row[c] + 1,
                                     cur_row[c - 1] + 1,
                                     prev_row[c - 1] + (s1[r - 1] == s2[c - 1] ? 0 : 1));
            }

            Array.Copy(cur_row, prev_row, cc);
        }
        return cur_row[cc - 1];
    }
    public int DDL()
    {
        var m = new int[l1 + 1, l2 + 1];

        for (int i = 0; i <= l1; i++)
            for (int j = 0; j <= l2; j++)
            {
                if (i * j == 0)
                    m[i, j] = i + j;
                else if (i > 1 && j > 1 && s1[i-2] == s2[j-1] && s1[i-1] == s2[j-2])
                    m[i, j] =
                        Min(m[i, j - 1] + 1,
                            m[i - 1, j] + 1,
                            m[i - 1, j - 1] + (s1?[i - 1] == s2?[j - 1] ? 0 : 1),
                            m[i - 2, j - 2] + 1);
                else
                    m[i, j] =
                        Min(m[i, j - 1] + 1,
                            m[i - 1, j] + 1,
                            m[i - 1, j - 1] + (s1?[i - 1] == s2?[j - 1] ? 0 : 1));
            }

        return m[l1, l2];
    }
    private int DR(int p1, int p2, int[,]? cash)
    {
        if (p1 * p2 == 0)
            return p1 + p2;

        if ((cash != null) && (cash[p1 - 1, p2 - 1] != -1))
            return cash[p1 - 1, p2 - 1];
        else
        {
            int result = Min(DR(p1 - 1, p2,     cash) + 1,
                             DR(p1,     p2 - 1, cash) + 1,
                             DR(p1 - 1, p2 - 1, cash) + (s1[p1 - 1] == s2[p2 - 1] ? 0 : 1));
            
            if (cash != null)
                cash[p1 - 1, p2 - 1] = result;

            return result;
        }
    }

    public int GetDistance(CalcMethod method)
    {
        if (l1 * l2 == 0)
            return l1 + l2;

        int[,] InitCache(int m, int n)
        {
            var cache = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    cache[i, j] = -1;

            return cache;
        }

        return method switch
        {
            CalcMethod.Matrix => DM(),
            CalcMethod.Recursive => DR(l1, l2, null),
            CalcMethod.RecursiveCashed => DR(l1, l2, InitCache(l1, l2)),
            CalcMethod.Domerau => DDL(),
            _ => -1
        };
    }

    public int Distance { get { return GetDistance(CalcMethod.Matrix); } }
    public TimeSpan MeasureTime(CalcMethod method = CalcMethod.Matrix, byte MeasureCount = 10)
    {
        var sw = Stopwatch.StartNew();        

        for (int i = 0; i < MeasureCount; i++)
            GetDistance(method);

        sw.Stop ();

        return sw.Elapsed.Divide(MeasureCount);
    }
}