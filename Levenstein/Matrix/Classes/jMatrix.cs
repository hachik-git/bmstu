п»їusing Common.Interfaces;

namespace Matrices;

public class jMatrix : BaseMatrix, IMatrix, ICompressed<Matrix>
{
    public List<int> Elements { get; }

    public int[] Counter { get; }

    public override int RowCount => Counter.Length;

    public override int ColCount => Counter.Length;

    public override bool IsSquare => true;

    public override bool IsSymmetric => true;

    public override int this[int row, int col]
    {
        get
        {
            if (col > row)
                (row, col) = (col, row);
            
            return Elements[Counter[row] - row + col];
        }
        set
        {
            if (col > row)
                (row, col) = (col, row);

            Elements[Counter[row] - row + col] = value;
        }
    }

    public override int[,] ToArray()
    {
        var a = new int[RowCount, ColCount];
        if (Elements.Count == 0)
            return a;
        
        a[0, 0] = Elements[0];

        for (int i = 1; i < Counter.Length; i++)
            for (int j = 0; j < Counter[i] - Counter[i - 1]; j++)
                a[i, i - j] = a[i - j, i] = Elements[Counter[i] - j];

        return a;
    }

    public jMatrix(List<int> elements, int[] counter)
    {
        Elements = elements;
        Counter = counter;
    }

    public jMatrix(Matrix M)
    {
        if (M.RowCount != M.ColCount)
            throw new ArgumentException("Jennings compressed matrix can be used only for square matrix");
        if (!M.IsSymmetric)
            throw new ArgumentException("Jennings compressed matrix can be used only for symmetric matrix");

        Counter = new int[M.RowCount];
        Elements = new List<int>();

        if (M.RowCount > 0)
        {
            Elements.Add(M[0, 0]);
            Counter[0] = 0;

            for (int i = 1; i < M.RowCount; i++)
            {
                Counter[i] = Counter[i - 1];
                for (int j = 0; j <= i; j++)
                    if ((M[i, j] != 0) || i==j || (Counter[i] > Counter[i - 1]))
                    {
                        Elements.Add(M[i, j]);
                        Counter[i]++;
                    }
            }
        }
    }

    protected jMatrix(int[] elements, int[] counter)
    {
        Elements = elements.ToList();
        Counter = counter;
    }

    public static explicit operator jMatrix(Matrix matrix)
    {
        return new jMatrix(matrix);
    }

    public static explicit operator Matrix(jMatrix jm)
    {
        return new Matrix(jm.ToArray());
    }

    public static jMatrix operator +(jMatrix a, jMatrix b)
    {
        if (a.Counter.Length != b.Counter.Length)
            throw new ArgumentException("Can't sum matrix with different dimentions");

        var nc = new int[a.Counter.Length];
        var nl = new List<int>();

        int na, nb;

        if (a.Counter.Length > 0)
        {
            nc[0] = 0;
            nl.Add(a[0, 0] + b[0, 0]);
            for (int i = 1; i < a.Counter.Length; i++)
            {
                na = a.Counter[i] - a.Counter[i - 1];
                nb = b.Counter[i] - b.Counter[i - 1];
                
                nc[i] = nc[i - 1] + Math.Max(na, nb);
                
                for (int j = nc[i] - nc[i - 1] - 1; j >= 0; j--)
                {
                    int value =
                        (j < na ? a.Elements[a.Counter[i] - j] : 0)
                        +
                        (j < nb ? b.Elements[b.Counter[i] - j] : 0);

                    if (value != 0 || (na == 1 && nb == 1))
                        nl.Add(value);
                    else
                        nc[i]--;
                }
            }
        }

        return new jMatrix(nl.ToArray(), nc);
    }

    public float Compression
    {
        get
        {
            int ms = sizeof(int) * RowCount * ColCount;
            int js = sizeof(int) * Counter.Length + sizeof(int) * Elements.Count;
            return 1 - (float)js / ms;
        }
    }

    public Matrix Decompress()
    {
        return (Matrix)this;
    }
}