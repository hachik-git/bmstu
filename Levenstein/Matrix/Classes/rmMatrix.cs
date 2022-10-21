п»їusing Common.Interfaces;
using System;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace Matrices;

public class rmMatrix : BaseMatrix, IMatrix, ICompressed<Matrix>
{
    public int[] Elements { get; }

    public int[] NR { get; }

    public int[] NC { get; }

    public int[] JR { get; }

    public int[] JC { get; }

    private rmMatrix(int[] elements,int[] jr, int[] jc, int[] nr, int[] nc)
    {
        Elements = elements;
        NR = nr;
        NC = nc;
        JR = jr;
        JC = jc;
    }


    public rmMatrix(Matrix M)
    {
        var elements = new List<int>();
        var nr = new List<int>();
        var nc = new List<int>();
        int idx;
        JR = Enumerable.Repeat(-1, M.RowCount).ToArray();
        JC = Enumerable.Repeat(-1, M.ColCount).ToArray();

        if (M.ColCount * M.RowCount > 0)
            for (int r = 0; r < M.RowCount; r++)
                for (int c = 0; c < M.ColCount; c++)
                    if (M[r, c] != 0)
                    {
                        elements.Add(M[r, c]);

                        nr.Add(idx = elements.Count - 1);

                        if (JR[r] == -1)
                            JR[r] = idx;
                        else
                            (nr[idx], nr[idx - 1]) = (nr[idx - 1], nr[idx]);
                        

                        nc.Add(idx);

                        if (JC[c] == -1)
                            JC[c] = idx;
                        else
                        {
                            int cur = JC[c], prev = JC[c];

                            while ((cur = nc[cur]) != JC[c])
                                prev = cur;

                            (nc[idx], nc[prev]) = (nc[prev], nc[idx]);
                        }
                    }
        #region old
        /*e_list.Add(M[r, c]);

        if (JR[r] == -1)
        {
            JR[r] = e_list.Count - 1;
            nr_list.Add(JR[r]);
        }
        else
        {
            nr_list.Add(JR[r]);
            nr_list[e_list.Count - 2] = e_list.Count - 1;
        }

        if (JC[c] == -1)
        {
            JC[c] = e_list.Count - 1;
            nc_list.Add(JC[c]);
        }
        else
        {
            int next = JC[c];
            int j;
            do
            {
                j = next;
            }
            while ((next = nc_list[next]) != JC[c]);

            nc_list[j] = e_list.Count - 1;
            nc_list.Add(JC[c]);
        }*/
        #endregion

        Elements = elements.ToArray();
        NR = nr.ToArray();
        NC = nc.ToArray();
    }

    public override int RowCount => JR.Length;

    public override int ColCount => JC.Length;


    public override int this[int row, int col]
    {
        get
        {
            if ((JR[row] == -1) || (JC[col] == -1))
                return 0;

            (int next_r, int next_c) = (JR[row], JC[col]);

            do
            {
                do
                {
                    if (next_r == next_c)
                        return Elements[next_r];
                }
                while ((next_c = NC[next_c]) != JC[col]);
            }
            while ((next_r = NR[next_r]) != JR[row]);

            return 0;
        }
        set => throw new NotImplementedException();
    }

    public int[] GetColumnIndexes(int col)
    {
        var res = new List<int>();
        if (JC[col] != -1)
        {
            int next = JC[col];
            do
            {
                res.Add(next);
            }
            while ((next < NC.Length) && ((next = NC[next]) != JC[col]));
        }   

        return res.ToArray();
    }

    public int[] GetRowIndexes(int row)
    {
        var res = new List<int>();
        if (JR[row] != -1)
        {
            int next = JR[row];
            do
            {
                res.Add(next);
            }
            while ((next < NR.Length) && ((next = NR[next]) != JR[row]));
        }

        return res.ToArray();
    }

    public float Compression
    {
        get
        {
            int ms = sizeof(int) * RowCount * ColCount;
            int rmms = sizeof(int) *
                (Elements.Length + NR.Length + NC.Length + JR.Length + JC.Length);
            return 1 - (float)rmms / ms;
        }
    }

    public Matrix Decompress()
    {
        return new Matrix(ToArray());
    }

    public static explicit operator rmMatrix(Matrix matrix)
    {
        return new rmMatrix(matrix);
    }

    public static explicit operator Matrix(rmMatrix jm)
    {
        return new Matrix(jm.ToArray());
    }

    public int GetElementCol(int ElementIndex)
    {
        for (int i = 0; i < JC.Length; i++)
        {
            int idx = JC[i];
            if (idx == -1)
                continue;

            do
            {
                if (idx == ElementIndex)
                    return i;
            }
            while ((idx = NC[idx]) != JC[i]) ;
        }

        return -1;
    }

    public int GetElementRow(int ElementIndex)
    {
        for (int i = 0; i < JR.Length; i++)
        {
            int idx = JR[i];
            if (idx == -1)
                continue;
            do
            {
                if (idx == ElementIndex)
                    return i;
            }
            while ((idx = NR[idx]) != JR[i]);
        }

        return -1;
    }

    public static rmMatrix operator +(rmMatrix A, rmMatrix B)
    {
        if ((A.ColCount != B.ColCount) || (A.RowCount != B.RowCount))
            throw new ArgumentException("РљРѕР»РёС‡РµСЃС‚РІРѕ СЃС‚СЂРѕРє Рё СЃС‚РѕР»Р±С†РѕРІ РјР°С‚СЂРёС†С‹ A РґРѕР»Р¶РЅРѕ СЂР°РІРЅСЏС‚СЊСЃСЏ РєРѕР»РёС‡РµСЃС‚РІСѓ СЃС‚СЂРѕРє Рё СЃС‚РѕР»Р±С†РѕРІ РјР°С‚СЂРёС†С‹ B");

        List<int> elements = new List<int>();
        List<int> nr = new List<int>();
        List<int> nc = new List<int>();

        var jr = Enumerable.Repeat(-1, A.RowCount).ToArray();
        var jc = Enumerable.Repeat(-1, A.ColCount).ToArray();

        int ai, aj, bi, bj;
        int r, c;
        int idx;
        int value;

        int i = 0, j = 0;

        while ((i < A.Elements.Length) || (j < B.Elements.Length))
        {
            (ai, aj, bi, bj) = (A.GetElementRow(i), A.GetElementCol(i),
                                B.GetElementRow(j), B.GetElementCol(j));

            if (ai < 0)
                (ai, aj) = (bi + 1, bj + 1);
            if (bi < 0)
                (bi, bj) = (ai + 1, aj + 1);

            (r, c) = (ai < bi ? ai : bi, (ai < bi  || (ai == bi && aj < bj)) ? aj : bj);

            if (ai * A.ColCount + aj < bi * B.ColCount + bj)
                value = A.Elements[i++];
            else if (ai * A.ColCount + aj > bi * B.ColCount + bj)
                value = B.Elements[j++];
            else
                value = A.Elements[i++] + B.Elements[j++];

            if (value == 0)
                continue;

            elements.Add(value);

            nr.Add(idx = elements.Count - 1);

            if (jr[r] == -1)
                jr[r] = idx;
            else
                (nr[idx], nr[idx - 1]) = (nr[idx - 1], nr[idx]);


            nc.Add(idx);

            if (jc[c] == -1)
                jc[c] = idx;
            else
            {
                int cur = jc[c], prev = jc[c];
                
                while ((cur = nc[cur]) != jc[c])
                    prev = cur;

                (nc[idx], nc[prev]) = (nc[prev], nc[idx]);
            }
        }

        return new rmMatrix(elements.ToArray(), jr, jc, nr.ToArray(), nc.ToArray());

        //return new rmMatrix((BaseMatrix)A + (BaseMatrix)B);
    }

    /*public static rmMatrix operator *(rmMatrix A, rmMatrix B)
    {
        return new rmMatrix((BaseMatrix)A * (BaseMatrix)B);
    }*/

    #region mul_new
    public static rmMatrix operator *(rmMatrix A, rmMatrix B)
    {
        if (A.ColCount != B.RowCount)
            throw new ArgumentException("РљРѕР»РёС‡РµСЃС‚РІРѕ СЃС‚СЂРѕРє РјР°С‚СЂРёС†С‹ A РґРѕР»Р¶РЅРѕ СЂР°РІРЅСЏС‚СЊСЃСЏ РєРѕР»РёС‡РµСЃС‚РІСѓ СЃС‚РѕР»Р±С†РѕРІ РјР°С‚СЂРёС†С‹ B");

        List<int> elements = new List<int>();
        List<int> nr = new List<int>();
        List<int> nc = new List<int>();

        var jr = Enumerable.Repeat(-1, A.RowCount).ToArray();
        var jc = Enumerable.Repeat(-1, B.ColCount).ToArray();

        int ai, aj, bi, bj;
        int a_idx, idx;
        int value;

        for (ai = 0; ai < A.RowCount; ai++)
        {
            a_idx = A.JR[ai];
            if (a_idx == -1)
                continue;

            for (bj = 0; bj < B.ColCount; bj++)
            {
                /*int b_idx = B.JC[bj];
                if (b_idx == -1)
                    continue;
                */
                value = 0;

                do
                {
                    aj = A.GetElementCol(a_idx);
                    value += A.Elements[a_idx] * B[aj, bj];
                }
                while ((a_idx = A.NR[a_idx]) != A.JR[ai]);

                if (value == 0)
                    continue;
                elements.Add(value);

                nr.Add(idx = elements.Count - 1);

                if (jr[ai] == -1)
                    jr[ai] = idx;
                else
                    (nr[idx], nr[idx - 1]) = (nr[idx - 1], nr[idx]);

                nc.Add(idx);

                if (jc[bj] == -1)
                    jc[bj] = idx;
                else
                {
                    int cur = jc[bj], prev = jc[bj];

                    while ((cur = nc[cur]) != jc[bj])
                        prev = cur;

                    (nc[idx], nc[prev]) = (nc[prev], nc[idx]);
                }

            };
        }

        return new rmMatrix(elements.ToArray(), jr, jc, nr.ToArray(), nc.ToArray());
    }

    #endregion
}
