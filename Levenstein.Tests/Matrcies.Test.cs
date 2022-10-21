using Matrices;
using System.Xml.Linq;

namespace Matrices.Tests.MSTest;

[TestClass]
public class MatrixMulTest
{
    private Matrix tm2x3()
    {
        return new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
    }

    private Matrix tm3x2()
    {
        var B = new Matrix(3, 2);

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 2; j++)
                B[i, j] = (i + 1) * (j + 1);

        return B;
    }

    [TestMethod]
    public void Test_mul_TripleCycle_2x3_3x2()
    {
        var A = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
        var B = new Matrix(3, 2);

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 2; j++)
                B[i, j] = (i + 1) * (j + 1);

        var Control = new Matrix(new int[,] { { 14, 28 }, { 32, 64 } });

        var C = A * B;
        CollectionAssert.AreEqual(C.Data, Control.Data);
    }

    [TestMethod]
    public void Test_mul_Vinograd_2x3_3x2()
    {
        var A = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
        var B = new Matrix(3, 2);

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 2; j++)
                B[i, j] = (i + 1) * (j + 1);

        var Control = new Matrix(new int[,] { { 14, 28 }, { 32, 64 } });

        Matrix.MulAlg = MultiplyAlgorithm.Vinograd;
        var C = A * B;
        CollectionAssert.AreEqual(C.Data, Control.Data);
    }

    [TestMethod]
    public void aaa()
    {
        var A = Matrix.GenMatrix(50, 50);
        var B = Matrix.GenMatrix(50, 50);
        var C = A + B;
        var D = new rmMatrix(C);
        var E = new rmMatrix(A) + new rmMatrix(B);

        CollectionAssert.AreEqual(D.Elements, E.Elements);

        A = new Matrix(new int[,] { { 1, 2, 3 }, { 0, 0, 0 }, { 3, 4, 5 }, });
        B = new Matrix(new int[,] { { 10, 20, 30 }, { 0, 0, 0 }, { 30, 40, 50 }, });
        C = A + B;
        D = new rmMatrix(C);
        E = new rmMatrix(A) + new rmMatrix(B);

        CollectionAssert.AreEqual(D.Elements, E.Elements);

        CollectionAssert.AreEqual(D.Elements, E.Elements);

        A = new Matrix(new int[,] { { 1, 2, 3 }, { 0, 0, 0 }, { 3, 4, 5 }, });
        B = new Matrix(new int[,] { { 10, 20, 30 }, { 30, 40, 50 }, { 60, 70, 80 }, });
        C = A + B;
        D = new rmMatrix(C);
        E = new rmMatrix(A) + new rmMatrix(B);

        CollectionAssert.AreEqual(D.Elements, E.Elements);
    }
    
    [TestMethod]
    public void ggg()
    {
        var A = new rmMatrix(new Matrix(new int[,] {
            {  0,   2,   0,   0,   0,   0 },
            {  0,   0,  10,  20,   0,   0 },
            { 30,   0,   0,  80,   0,  90 },
            {  0,   0,  50,   0,   0,  60 },
            { 40,   0,   0,  70,   0,   0 },
        }));

        var B = new rmMatrix(new Matrix(new int[,] {
            { 10,   0,   4,   0,   0,   0 },
            { 40,  10,  50,  20,   0,   0 },
            { 30,   0,   0,  80,   0,  90 },
            {  0,   0,  50,   0,   0,  60 },
            { 40,   0,  50,  70,   0,   0 },
        }));

        if ((A.ColCount != B.ColCount) || (A.RowCount != B.RowCount))
            throw new ArgumentException("Количество строк и столбцов матрицы A должно равняться количеству строк и столбцов матрицы B");

        List<int> elements = new List<int>();
        List<int> nr = new List<int>();
        List<int> nc = new List<int>();
        var jr = new int[A.RowCount];
        var jc = new int[A.ColCount];

        jr = Enumerable.Repeat(-1, A.RowCount).ToArray();
        jc = Enumerable.Repeat(-1, A.ColCount).ToArray();

        int ai, bi, aj, bj;
        int r, c;
        int idx;
        int value;

        for (int i = 0, j = 0; (i < A.Elements.Length) && (j < B.Elements.Length); )
        {
            (ai, aj, bi, bj) = (A.GetElementRow(i), A.GetElementCol(i),
                                B.GetElementRow(j), B.GetElementCol(j));

            (r, c) = (ai < bi ? ai : bi, (ai <= bi && aj < bj) ? aj : bj);

            if (ai * A.ColCount + aj < bi * B.ColCount + bj)
                value = A.Elements[i++];                
            else if (ai * A.ColCount + aj > bi * B.ColCount + bj)
                value = B.Elements[j++];
            else
                value = A.Elements[i++] + B.Elements[j++];

            if (value == 0)
                continue;

            elements.Add(value);
            idx = elements.Count - 1;
            
            nr.Add(idx);            

            if (jr[r] == -1)
                jr[r] = idx;
            else
                (nr[idx], nr[idx - 1]) = (nr[idx - 1], nr[idx]);


            nc.Add(idx);

            if (jc[c] == -1)
                jc[c] = idx;
            else
            {
                int next = jc[c];
                int a;
                do
                {
                    a = next;
                }
                while ((next = nc[next]) != jc[c]);

                (nc[idx], nc[a]) = (nc[a], nc[idx]);
            }
        }

        #region old
        /* for (int r = 0; r < A.RowCount; r++)
        {
            var a_cur = A.JR[r];
            var b_cur = B.JR[r];

            int a_col, b_col, col;
            do
            {
                a_col = A.GetElementCol(a_cur);
                b_col = B.GetElementCol(b_cur);

                if (a_col < b_col)
                {
                    col = a_col;
                    elements.Add(A.Elements[a_cur]);

                    if ((a_cur = A.NR[a_cur]) == A.JR[r])
                        a_cur = -1;
                }
                else if (a_col > b_col)
                {
                    col = b_col;
                    elements.Add(B.Elements[b_cur]);

                    if ((b_cur = B.NR[b_cur]) == B.JR[r])
                        b_cur = -1;
                }
                else
                {
                    col = a_col;
                    elements.Add(A.Elements[a_cur] + B.Elements[b_cur]);

                    if ((a_cur = A.NR[a_cur]) == A.JR[r])
                        a_cur = -1;
                    if ((b_cur = B.NR[b_cur]) == B.JR[r])
                        b_cur = -1;
                }

                if (jr[r] == -1)
                    jr[r] = elements.Count - 1;

                if (jc[col] == -1)
                    jc[col] = elements.Count - 1;
            }
            while ((a_cur != -1) && (b_cur != -1));

            if (b_cur != -1)
            {
                do
                {
                    elements.Add(B.Elements[b_cur]);
                    
                    b_col = B.GetElementCol(b_cur);
                    if (jc[b_col] == -1)
                        jc[b_col] = elements.Count - 1;

                } while ((b_cur = B.NR[b_cur]) != B.JR[r]);

            }
            else if (a_cur != -1)
            {
                do
                {
                    elements.Add(A.Elements[a_cur]);

                    a_col = A.GetElementCol(a_cur);
                    if (jc[a_col] == -1)
                        jc[a_col] = elements.Count - 1;

                } while ((a_cur = A.NR[a_cur]) != A.JR[r]);
            }
        }*/
        #endregion
    }

    [TestMethod]
    public void Test_mul_TripleCycle_3x2_2x3()
    {
        var A = new Matrix(new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } });
        var B = new Matrix(new int[,] { { 7, 8, 9 }, { 10, 11, 12 } });

        var Control = new Matrix(new int[,] { { 27, 30, 33 }, { 61, 68, 75 }, { 95, 106, 117 } });

        var C = A * B;
        CollectionAssert.AreEqual(C.Data, Control.Data);
    }

    [TestMethod]
    public void Test_mul_Vinograd_3x2_2x3()
    {
        var A = new Matrix(new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } });
        var B = new Matrix(new int[,] { { 7, 8, 9 }, { 10, 11, 12 } });

        var Control = new Matrix(new int[,] { { 27, 30, 33 }, { 61, 68, 75 }, { 95, 106, 117 } });

        Matrix.MulAlg = MultiplyAlgorithm.Vinograd;
        var C = A * B;
        CollectionAssert.AreEqual(C.Data, Control.Data);
    }

    [TestMethod]
    public void Test_mul_TripleCycle_2x2_2x2()
    {
        var A = new Matrix(new int[,] { { 11, 22 }, { 33, 44 } });
        var B = new Matrix(new int[,] { { 55, 66 }, { 77, 88 } });

        var Control = new Matrix(new int[,] { { 2299, 2662 }, { 5203, 6050 } });

        var C = A * B;
        CollectionAssert.AreEqual(C.Data, Control.Data);
    }

    [TestMethod]
    public void Test_mul_Vinograd_2x2_2x2()
    {
        var A = new Matrix(new int[,] { { 11, 22 }, { 33, 44 } });
        var B = new Matrix(new int[,] { { 55, 66 }, { 77, 88 } });

        var Control = new Matrix(new int[,] { { 2299, 2662 }, { 5203 , 6050 } });

        Matrix.MulAlg = MultiplyAlgorithm.Vinograd;
        var C = A * B;
        CollectionAssert.AreEqual(C.Data, Control.Data);
    }

    [TestMethod]
    public void Test_mul_TripleCycle_1x3_3x2()
    {
        var A = new Matrix(new int[,] { { 100, 200, 300 } });
        var B = new Matrix(new int[,] { { 10, 20 }, { 30, 40 }, { 50, 60 } });

        var Control = new Matrix(new int[,] { { 22000, 28000 } });

        var C = A * B;
        CollectionAssert.AreEqual(C.Data, Control.Data);
    }

    [TestMethod]
    public void Test_mul_Vinograd_1x3_3x2()
    {
        var A = new Matrix(new int[,] { { 100, 200, 300 } });
        var B = new Matrix(new int[,] { { 10, 20 }, { 30, 40 }, { 50, 60 } });

        var Control = new Matrix(new int[,] { { 22000, 28000 } });

        Matrix.MulAlg = MultiplyAlgorithm.Vinograd;
        var C = A * B;
        CollectionAssert.AreEqual(C.Data, Control.Data);
    }

    [TestMethod]
    public void Test_mul_TripleCycle_3x1_1x5()
    {
        var A = new Matrix(new int[,] { { 100 }, { 200 }, {300 } });
        var B = new Matrix(new int[,] { { 10, 20, 30, 40,  50 } });

        var Control = new Matrix(new int[,] { { 1000,2000,3000,4000,5000 },
                                              { 2000,4000,6000,8000,10000 },
                                              { 3000,6000,9000,12000,15000 } });

        var C = A * B;
        CollectionAssert.AreEqual(C.Data, Control.Data);
    }

    [TestMethod]
    public void Test_mul_Vinograd_3x1_1x5()
    {
        var A = new Matrix(new int[,] { { 100 }, { 200 }, { 300 } });
        var B = new Matrix(new int[,] { { 10, 20, 30, 40, 50 } });

        var Control = new Matrix(new int[,] { { 1000,2000,3000,4000,5000 },
                                              { 2000,4000,6000,8000,10000 },
                                              { 3000,6000,9000,12000,15000 } });

        Matrix.MulAlg = MultiplyAlgorithm.Vinograd;
        var C = A * B;
        CollectionAssert.AreEqual(C.Data, Control.Data);
    }

    [TestMethod]
    public void Test_mul_TripleCycle_3x1_2x2_throw_exception()
    {
        var A = new Matrix(new int[,] { { 100 }, { 200 }, { 300 } });
        var B = new Matrix(new int[,] { { 10, 20 }, {30, 40 } });

        Assert.ThrowsException<ArgumentException>(() => { var C = A * B; } );
    }

    [TestMethod]
    public void Test_mul_Vinograd_3x1_2x2_throw_exception()
    {
        var A = new Matrix(new int[,] { { 100 }, { 200 }, { 300 } });
        var B = new Matrix(new int[,] { { 10, 20 }, { 30, 40 } });

        Matrix.MulAlg = MultiplyAlgorithm.Vinograd;

        Assert.ThrowsException<ArgumentException>(() => { var C = A * B; });
    }
}

[TestClass]
public class MatrixSumTest
{
    [TestMethod]
    public void test_sum()
    {
        var A = new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 }, { 3, 5, 9 } });
        var S = A+A;
        var T = new Matrix(new int[,] { { 2, 4, 6}, { 4, 8, 10 }, { 6, 10, 18 } });

        CollectionAssert.AreEqual(T.Data, S.Data);
    }

    [TestMethod]
    public void summ_3x3_2x2_throw()
    {
        var A = new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 }, { 3, 5, 9 } });
        var B = new Matrix(new int[,] { { 1, 2 }, { 2, 4 } });

        Assert.ThrowsException<ArgumentException>(() => { var C = A + B; });
    }

    [TestMethod]
    public void summ_2x3_3x2_throw()
    {
        var A = new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 } });
        var B = new Matrix(new int[,] { { 1, 2 }, { 2, 4 }, { 4, 5 } });

        Assert.ThrowsException<ArgumentException>(() => { var C = A + B; });
    }

    [TestMethod]
    public void summ_3x2_2x3_throw()
    {
        var A = new Matrix(new int[,] { { 1, 2 }, { 2, 4 }, { 4, 5 } });
        var B = new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 } });        

        Assert.ThrowsException<ArgumentException>(() => { var C = A + B; });
    }
}

[TestClass]
public class jMatrixTest
{

    [TestMethod]
    public void not_square_and_symmetric_throw_arg_exception()
    {
        var A = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
        Assert.ThrowsException<ArgumentException>(() => { var C = A * A; });
    }

    [TestMethod]
    public void empty_jmatrix_convertation()
    {
        var jm = new jMatrix(new Matrix(0, 0));
        var m = (Matrix)jm;
        var Expected = new Matrix(0, 0);
        CollectionAssert.AreEqual(m.Data, Expected.Data);
    }

    [TestMethod]
    public void random_matrix_convertation()
    {
        var m = new Matrix(100, 100);
        var r = new Random(unchecked((int)DateTime.Now.Ticks));

        for (int i = 0; i < 50; i++)
            for (int j = 0; j <= i; j++)
                m[i, j] = m[j, i] = r.Next();

        var t = (Matrix)((jMatrix)m);

        CollectionAssert.AreEqual(m.Data, t.Data );
    }

    [TestMethod]
    public void random_jmatrix_check_indexator()
    {
        var m = new Matrix(101,101);
        var r = new Random(unchecked((int)DateTime.Now.Ticks));

        for (int i = 0; i < m.RowCount; i++)
            for (int j = 0; j <= i; j++)
                m[i, j] = m[j, i] = r.Next();

        var jm = new jMatrix(m);

        for (int i = 0; i < m.RowCount; i++)
            for (int j = 0; j < m.ColCount; j++)
                Assert.AreEqual(m[i, j], jm[i, j]);
    }

    [TestMethod]
    public void summ_3x3_2x2_throw()
    {
        var A = new jMatrix(new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 }, { 3, 5, 9 } }));
        var B = new jMatrix(new Matrix(new int[,] { { 1, 2 }, { 2, 4 } }));

        Assert.ThrowsException<ArgumentException>(() => { var C = A + B; });
    }

    [TestMethod]
    public void summ_2x2_3x3_throw()
    {
        var A = new jMatrix(new Matrix(new int[,] { { 1, 2 }, { 2, 4 } }));
        var B = new jMatrix(new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 }, { 3, 5, 9 } }));

        Assert.ThrowsException<ArgumentException>(() => { var C = A + B; });
    }

    [TestMethod]
    public void test_summ_empty_matrices()
    {
        var A = new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 }, { 3, 5, 9 } });
        var AS = A + A;
        var J = (jMatrix)(A);
        var JS = J + J;

        CollectionAssert.AreEqual(AS.Data, ((Matrix)JS).Data);
    }

    [TestMethod]
    public void empty_matrix_summ()
    {
        var A = new Matrix(0, 0);        
        var J = (jMatrix)(A);
        var JS = J + J;

        CollectionAssert.AreEqual(A.Data, ((Matrix)JS).Data);
    }

    [TestMethod]
    public void test_summ()
    {
        var A = new Matrix(new int[,] { { 1,2,3 }, { 2, 4, 5 }, { 3, 5, 9 } });
        var AS = A + A;
        var J = (jMatrix)(A);
        var JS = J + J;

        CollectionAssert.AreEqual(AS.Data, ((Matrix)JS).Data);
    }
}

[TestClass]
public class rmMatrixTest
{
    [TestMethod]
    public void random_rmmatrix_check_indexator()
    {
        var m = new Matrix(100, 100);
        var r = new Random(unchecked((int)DateTime.Now.Ticks));

        for (int i = 0; i < 50; i++)
            for (int j = 0; j <= i; j++)
                m[i, j] = m[j, i] = r.Next();

        var jm = new rmMatrix(m);

        for (int i = 0; i < m.RowCount; i++)
            for (int j = 0; j < m.ColCount; j++)
                Assert.AreEqual(m[i, j], jm[i, j]);
    }

    [TestMethod]
    public void empty_rmmatrix_convertation()
    {
        var rmm = new rmMatrix(new Matrix(0, 0));
        var m = (Matrix)rmm;
        var Expected = new Matrix(0, 0);
        CollectionAssert.AreEqual(m.Data, Expected.Data);
    }

    [TestMethod]
    public void random_matrix_4x3_convertation()
    {
        var m = new Matrix(4, 3);
        var r = new Random(unchecked((int)DateTime.Now.Ticks));

        for (int i = 0; i < m.RowCount; i++)
            for (int j = 0; j < m.ColCount; j++)
                m[i, j] = r.Next();

        var t = (Matrix)((rmMatrix)m);

        CollectionAssert.AreEqual(m.Data, t.Data);
    }

    [TestMethod]
    public void random_matrix_3x4_convertation()
    {
        var m = new Matrix(4, 3);
        var r = new Random(unchecked((int)DateTime.Now.Ticks));

        for (int i = 0; i < m.RowCount; i++)
            for (int j = 0; j < m.ColCount; j++)
                m[i, j] = r.Next();

        var t = (Matrix)((rmMatrix)m);

        CollectionAssert.AreEqual(m.Data, t.Data);
    }

    [TestMethod]
    public void random_matrix_4x4_convertation()
    {
        var m = new Matrix(4, 4);
        var r = new Random(unchecked((int)DateTime.Now.Ticks));

        for (int i = 0; i < m.RowCount; i++)
            for (int j = 0; j < m.ColCount; j++)
                m[i, j] = r.Next();

        var t = (Matrix)((rmMatrix)m);

        CollectionAssert.AreEqual(m.Data, t.Data);
    }

    [TestMethod]
    public void random_matrix_100x100_convertation()
    {
        var m = new Matrix(100, 100);
        var r = new Random(unchecked((int)DateTime.Now.Ticks));

        for (int i = 0; i < m.RowCount; i++)
            for (int j = 0; j < m.ColCount; j++)
                m[i, j] = r.Next();

        var t = (Matrix)((rmMatrix)m);

        CollectionAssert.AreEqual(m.Data, t.Data);
    }

    [TestMethod]
    public void random_matrix_4x3_empty_last_row_convertation()
    {
        var m = new Matrix(4, 3);
        var r = new Random(unchecked((int)DateTime.Now.Ticks));

        for (int i = 0; i < m.RowCount-1; i++)
            for (int j = 0; j < m.ColCount; j++)
                m[i, j] = r.Next();

        for (int j = 0; j < m.ColCount; j++)
            m[3, j] = 0;

        var rm = (rmMatrix)m;
        for (int i = 0; i < rm.NR.Length; i++)
            Assert.IsTrue(rm.NR[i] >= 0 && rm.NR[i] < rm.Elements.Length);

        var t = (Matrix)rm;

        CollectionAssert.AreEqual(m.Data, t.Data);
    }

    /*[TestMethod]
    public void summ_3x3_2x2_throw()
    {
        var A = new rmMatrix(new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 }, { 3, 5, 9 } }));
        var B = new rmMatrix(new Matrix(new int[,] { { 1, 2 }, { 2, 4 } }));

        Assert.ThrowsException<ArgumentException>(() => { var C = A + B; });
    }

    [TestMethod]
    public void summ_2x2_3x3_throw()
    {
        var A = new rmMatrix(new Matrix(new int[,] { { 1, 2 }, { 2, 4 } }));
        var B = new rmMatrix(new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 }, { 3, 5, 9 } }));

        Assert.ThrowsException<ArgumentException>(() => { var C = A + B; });
    }

    [TestMethod]
    public void test_summ_empty_matrices()
    {
        var A = new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 }, { 3, 5, 9 } });
        var AS = A + A;
        var J = (rmMatrix)(A);
        var JS = J + J;

        CollectionAssert.AreEqual(AS.Data, ((Matrix)JS).Data);
    }

    [TestMethod]
    public void empty_matrix_summ()
    {
        var A = new Matrix(0, 0);
        var J = (rmMatrix)(A);
        var JS = J + J;

        CollectionAssert.AreEqual(A.Data, ((Matrix)JS).Data);
    }

    [TestMethod]
    public void test_summ()
    {
        var A = new Matrix(new int[,] { { 1, 2, 3 }, { 2, 4, 5 }, { 3, 5, 9 } });
        var AS = A + A;
        var J = (rmMatrix)(A);
        var JS = J + J;

        CollectionAssert.AreEqual(AS.Data, ((Matrix)JS).Data);
    }*/
}