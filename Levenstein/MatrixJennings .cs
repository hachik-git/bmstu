п»їusing System.Data.Common;

namespace Algorithm.Matrix;

public class jMatrix
{
    public int[] Elements { get; }
    public int[] Counter { get; }
    public int this[int row, int col]
    {
        get { return Elements[Counter[row] - row + col - 1]; }
        set { Elements[Counter[row] - row + col - 1] = value; }
    }
    public jMatrix(Matrix M)
    {
        if (M.RowCount != M.ColCount)
            throw new ArgumentException("Jennings compressed matrix can be used only for square matrix");
        if (!M.IsSymmetric)
            throw new ArgumentException("Jennings compressed matrix can be used only for symmetric matrix");

        Counter = new int[M.RowCount];
        var EList = new List<int>();
        if (M.RowCount > 0)
        {
            EList.Add(M[0, 0]);
            Counter[0] = 1;

            for (int i = 1; i < M.RowCount; i++)
            {
                Counter[i] = Counter[i - 1];
                for (int j = 0; j <= i; j++)
                    if ((M[i, j] != 0) || (Counter[i] > Counter[i - 1]))
                    {
                        EList.Add(M[i, j]);
                        Counter[i]++;
                    }
            }
        }
        Elements = EList.ToArray();
    }

    public jMatrix(int[] elements, int[] counter)
    {
        Elements = elements;
        Counter = counter;
    }

    public static explicit operator jMatrix(Matrix matrix)
    {
        return new jMatrix(matrix);
    }

    public static explicit operator Matrix(jMatrix mj)
    {
        var m = new Matrix(mj.Counter.Length, mj.Counter.Length);
            
        m[0,0] = mj.Elements[0];
            
        for (int i = 1; i < mj.Counter.Length; i++)
            for (int j = 0; j < mj.Counter[i] - mj.Counter[i-1]; j++)
                m[i, i-j] = m[i - j, i] = mj.Elements[mj.Counter[i] - j - 1];

        return m;
    }

    public static jMatrix operator +(jMatrix a, jMatrix b)
    {
        if (a?.Counter.Length != b.Counter.Length)
            throw new ArgumentException("Can't sum matrix with different dimentions");
        
        var nc = new int[a.Counter.Length];
        var nl = new List<int>();

        for (int i = 0; i < a.Counter.Length; i++)
        {
            /*Math.Max(
            a.Counter[a.Counter.Length - 1],
            b.Counter[b.Counter.Length - 1])*/

            nl.Add(a.Elements.FirstOrDefault(i) + b.Elements.FirstOrDefault(i));
            nc[i] = Math.Max(a.Counter[i], b.Counter[i]);
        }
        
        return new jMatrix(nl.ToArray(), nc);
    }
}