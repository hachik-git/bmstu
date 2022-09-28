п»їusing System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMult
{
    public enum MultiplyAlgorithm
    {
        TripleCycle,
        Vinograd,
    }
    public class Matrix
    {
        public int[,] Data { get; }
        public static MultiplyAlgorithm MulAlg { get; set; } = MultiplyAlgorithm.TripleCycle;
        public int this[int r, int c]
        {
            get { return Data[r, c]; }
            set { Data[r, c] = value; }
        }
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }
        public Matrix(int RowCount, int ColCount)
        {
            (this.RowCount, this.ColCount) = (RowCount, ColCount);
            Data = new int[RowCount, ColCount];
        }
        public Matrix(int[,] a)
        {
            (RowCount, ColCount) = (a.GetLength(0), a.Length / a.GetLength(0));
            Data = a;
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
            
            Matrix C = new Matrix(B.ColCount, A.RowCount);

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
            else
            {

                int M = A.RowCount;
                int N = A.ColCount;
                int Q = B.ColCount;

                var MulH = new int[M];
                var MulV = new int[Q];

                for (int i = 0; i < M; i++)
                {
                    for (int k = 0; k < (N/ 2); k++)
                        MulH[i] += A[i, 2*k] * A[i, 2*k+1];
                }
                for (int i = 0; i < Q; i++)
                {
                    for (int k = 0; k < (N / 2); k++)
                        MulV[i] += B[2*k, i] * B[2*k+1, i];
                }

                for (int i = 0; i < M; i++) 
                    for (int j = 0; j < Q; j++)
                    {
                        C[i, j] = -MulH[i] - MulV[j];
                        for (int k = 0; k < (N / 2); k++)
                            C[i, j] = C[i, j] + (A[i, 2*k] + B[2*k + 1, j]) * (A[i, 2*k + 1] + B[2*k, j]);
                    }

                if (N % 2 != 0)
                {
                    for (int i = 0; i < M; i++)
                        for (int j = 0; j < Q; j++)
                            C[i, j] += A[i, N-1] * B[N-1, j];
                }
            }

            return C;
        }
    }
}
