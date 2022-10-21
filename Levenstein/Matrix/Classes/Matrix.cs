п»їnamespace Matrices;

using System;

public enum MultiplyAlgorithm
{
    TripleCycle,
    Vinograd,
    VinogradOptimized,
}

public class Matrix : BaseMatrix, IMatrix
{
    private int rowCount = 0;

    private int colCount = 0;

    public int[,] Data { get; }

    public static MultiplyAlgorithm MulAlg { get; set; } = MultiplyAlgorithm.TripleCycle;

    public override int RowCount { get { return rowCount; } }

    public override int ColCount { get { return colCount; } }

    public int? ElementAtOrNull(int index)
    {
        if (index < RowCount * ColCount)
            return Data[index / ColCount, index - (index / ColCount) * ColCount];
        else
            return null;
    }
    
    public override int this[int r, int c]
    {
        get { return Data[r, c]; }
        set { Data[r, c] = value; } 
    }

    public override int[,] ToArray()
    {
        var a = new int[RowCount, ColCount];
        Array.Copy(Data, a, Data.Length);
        return a;
    }

    public Matrix(int[,] a)
    {   
        Data = a;
        (rowCount, colCount) = (a.GetLength(0), a.GetLength(0) == 0 ? 0 : a.Length / a.GetLength(0));
    }

    public Matrix(int RowCount, int ColCount) : this(new int[RowCount, ColCount])
    {
        ;
    }

    public Matrix(int RowCount, int ColCount, int[] A) : this(RowCount, ColCount)
    {
        for (int r = 0; r < RowCount; r++)
            for (int c = 0; c < ColCount; c++)
                if (A.Length > r * ColCount + c)
                    this[r, c] = A[r*ColCount + c];
    }

    public static Matrix GenMatrix(int m, int n)
    {
        var matrix = new Matrix(m, n);
        var rnd = new Random(unchecked((int)DateTime.Now.Ticks));

        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
                matrix[i, j] = rnd.Next(1000);
            
        return matrix;
    }

    public static Matrix operator *(Matrix A, Matrix B)
    {
        if (A.ColCount != B.RowCount)
            throw new ArgumentException("РљРѕР»РёС‡РµСЃС‚РІРѕ СЃС‚СЂРѕРє РјР°С‚СЂРёС†С‹ A РґРѕР»Р¶РЅРѕ СЂР°РІРЅСЏС‚СЊСЃСЏ РєРѕР»РёС‡РµСЃС‚РІСѓ СЃС‚РѕР»Р±С†РѕРІ РјР°С‚СЂРёС†С‹ B");
            
        Matrix C = new Matrix(A.RowCount, B.ColCount);

        if (MulAlg == MultiplyAlgorithm.TripleCycle)
        {
            for (int m = 0; m < A.RowCount; m++)
                for (int q = 0; q < B.ColCount; q++)
                {
                    C[m, q] = 0;
                    for (int n = 0; n < A.ColCount; n++)
                        C[m, q] += A[m, n] * B[n, q];
                }
        }
        else if (MulAlg == MultiplyAlgorithm.Vinograd)
        {

            (int M, int N, int Q) = (A.RowCount, A.ColCount, B.ColCount);
            (var MulH, var MulV) = (new int[M], new int[Q]);

            for (int i = 0; i < M; i++)
                for (int k = 0; k < (N/ 2); k++)
                    MulH[i] = MulH[i] + A[i, 2*k] * A[i, 2*k+1];

            for (int i = 0; i < Q; i++)
                for (int k = 0; k < (N / 2); k++)
                    MulV[i] = MulV[i] + B[2*k, i] * B[2*k+1, i];

            for (int i = 0; i < M; i++) 
                for (int j = 0; j < Q; j++)
                {
                    C[i, j] = -MulH[i] - MulV[j];
                    for (int k = 0; k < (N / 2); k++)
                        C[i, j] = C[i, j] + (A[i, 2*k] + B[2*k + 1, j]) * (A[i, 2*k + 1] + B[2*k, j]);
                }

            if (N % 2 != 0)
                for (int i = 0; i < M; i++)
                    for (int j = 0; j < Q; j++)
                        C[i, j] = C[i, j] + A[i, N-1] * B[N-1, j];
        }
        else if (MulAlg == MultiplyAlgorithm.VinogradOptimized)
        {

            (int M, int N, int Q) = (A.RowCount, A.ColCount, B.ColCount);
            (var MulH, var MulV) = (new int[M], new int[Q]);

            int d = (N / 2) * 2;
            for (int i = 0; i < M; i++)
                for (int k = 0; k < d; k += 2)
                    MulH[i] -= A[i, k] * A[i, k + 1];

            for (int i = 0; i < Q; i++)
                for (int k = 0; k < d; k += 2)
                    MulV[i] -= B[k, i] * B[k + 1, i];

            for (int i = 0; i < M; i++)
                for (int j = 0; j < Q; j++)
                {
                    int buf = 0;
                    for (int k = 0; k < d; k += 2)
                        buf += (A[i, k] + B[k + 1, j]) * (A[i, k + 1] + B[k, j]);
                    C[i, j] = MulH[i] + MulV[j] + buf;
                }

            if (N % 2 != 0)
                for (int i = 0; i < M; i++)
                    for (int j = 0; j < Q; j++)
                        C[i, j] += A[i, N - 1] * B[N - 1, j];
        }

        return C;
    }

    public static Matrix operator +(Matrix A, Matrix B)
    {
        if ((A.ColCount != B.ColCount) || (A.RowCount != B.RowCount))
            throw new ArgumentException("РљРѕР»РёС‡РµСЃС‚РІРѕ СЃС‚СЂРѕРє Рё СЃС‚РѕР»Р±С†РѕРІ РјР°С‚СЂРёС†С‹ A РґРѕР»Р¶РЅРѕ СЂР°РІРЅСЏС‚СЊСЃСЏ РєРѕР»РёС‡РµСЃС‚РІСѓ СЃС‚СЂРѕРє Рё СЃС‚РѕР»Р±С†РѕРІ РјР°С‚СЂРёС†С‹ B");

        Matrix C = new Matrix(A.RowCount, A.ColCount);

        for (int r = 0; r < A.RowCount; r++)
            for (int c = 0; c < A.ColCount; c++)
                C[r, c] = A[r, c] + B[r, c];

        return C;
    }
}