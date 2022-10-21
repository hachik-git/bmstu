п»їusing Microsoft.Extensions.Primitives;
using System.Text;

namespace Matrices
{
    public abstract class BaseMatrix : IMatrix
    {
        public string? Name { get; set; }

        public abstract int this[int r, int c] { get; set; }

        public abstract int RowCount { get; }

        public abstract int ColCount { get; }

        public virtual bool IsSymmetric
        {
            get
            {
                if (RowCount != ColCount)
                    return false;

                for (int i = 0; i < RowCount; i++)
                    for (int j = 0; j < ColCount; j++)
                        if (this[i, j] != this[j, i])
                            return false;

                return true;
            }
        }

        public virtual bool IsSquare { get { return RowCount == ColCount; } }

        public virtual int[,] ToArray()
        {
            var a = new int[RowCount, ColCount];
            for (int r = 0; r < RowCount; r++)
                for (int c = 0; c < ColCount; c++)
                    a[r, c] = this[r, c];

            return a;
        }

        public override string ToString()
        {
            /*var sb = new StringBuilder();
            sb.Append("{");
            for (int r = 0; r < RowCount; r++)
            {
                sb.Append(" {");
                for (int c = 0; c < ColCount; c++)
                {
                    sb.Append(this[r, c] + (c == ColCount - 1 ? "" : ", "));
                }
                sb.Append("} " + (r == RowCount - 1 ? "" : ", "));
            }
            sb.Append("}");

            return sb.ToString();*/
            return $"Matrix{(" " + Name + " ").Trim()}[{RowCount}x{ColCount}]";
        }

        public static Matrix operator *(BaseMatrix A, BaseMatrix B)
        {
            if (A.ColCount != B.RowCount)
                throw new ArgumentException("РљРѕР»РёС‡РµСЃС‚РІРѕ СЃС‚СЂРѕРє РјР°С‚СЂРёС†С‹ A РґРѕР»Р¶РЅРѕ СЂР°РІРЅСЏС‚СЊСЃСЏ РєРѕР»РёС‡РµСЃС‚РІСѓ СЃС‚РѕР»Р±С†РѕРІ РјР°С‚СЂРёС†С‹ B");

            var matrix = new Matrix(A.RowCount, B.ColCount);
            
            for (int m = 0; m < A.RowCount; m++)
                for (int q = 0; q < B.ColCount; q++)
                {
                    matrix[m, q] = 0;
                    for (int n = 0; n < A.ColCount; n++)
                        matrix[m, q] += A[m, n] * B[n, q];
                }

            return matrix;
        }

        public static Matrix operator +(BaseMatrix A, BaseMatrix B)
        {
            if ((A.ColCount != B.ColCount) || (A.RowCount != B.RowCount))
                throw new ArgumentException("РљРѕР»РёС‡РµСЃС‚РІРѕ СЃС‚СЂРѕРє Рё СЃС‚РѕР»Р±С†РѕРІ РјР°С‚СЂРёС†С‹ A РґРѕР»Р¶РЅРѕ СЂР°РІРЅСЏС‚СЊСЃСЏ РєРѕР»РёС‡РµСЃС‚РІСѓ СЃС‚СЂРѕРє Рё СЃС‚РѕР»Р±С†РѕРІ РјР°С‚СЂРёС†С‹ B");

            var matrix = new Matrix(A.RowCount, B.ColCount);

            for (int r = 0; r < A.RowCount; r++)
                for (int c = 0; c < B.ColCount; c++)
                {
                    matrix[r, c] = A[r, c] + B[r, c];
                }

            return matrix;
        }
    }
}
