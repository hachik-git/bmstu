п»їnamespace Algorithm.Matrix;

public class MatrixJennings
{
    public int[] Elements { get; }
    public int[] Counter { get; }
    public int this[int row, int col]
    {
        get { return Elements[Counter[row] - row + col - 1]; }
        set { Elements[Counter[row] - row + col - 1] = value; }
    }
    public MatrixJennings(Matrix M)
    {
        if (M.RowCount != M.ColCount)
            throw new ArgumentException("Jennings compressed matrix can be used ony for square matrix");

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

    public static explicit operator MatrixJennings(Matrix matrix)
    {
        return new MatrixJennings(matrix);
    }

    public static explicit operator Matrix(MatrixJennings mj)
    {
        var m = new Matrix(mj.Counter.Length, mj.Counter.Length);
            
        m[0,0] = mj.Elements[0];
            
        for (int i = 1; i < mj.Counter.Length; i++)
            for (int j = 0; j < mj.Counter[i] - mj.Counter[i-1]; j++)
                m[i, i-j] = m[i - j, i] = mj.Elements[mj.Counter[i] - j - 1];

        return m;
    }
}