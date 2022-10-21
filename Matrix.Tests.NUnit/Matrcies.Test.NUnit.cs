п»їusing Allure.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;

namespace Matrices.Tests.NUnit;
class TestMatrices
{
    public Matrix Empty { get; } = new Matrix(0, 0);
    public Matrix Random(int r, int c, bool symmetric)
    {
        var m = new Matrix(r, c);
        var rnd = new Random(unchecked((int)DateTime.Now.Ticks));

        if (symmetric)
        {
            for (int i = 0; i < r; i++)
                for (int j = 0; j <= i; j++)
                    m[i, j] = m[j, i] = rnd.Next();
        }
        else
        {
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    m[i, j] = rnd.Next();
        }

        return m;
    }
    public Matrix m2x3 { get; } = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
    public Matrix m3x2 { get; } = new Matrix(new int[,] { { 1, 2 }, { 2, 4 }, { 3, 6 } });

    public Matrix m2x2 { get; } = new Matrix(new int[,] { { 1, 2 }, { 3, 4 } });
    public Matrix m2x2_A { get; } = new Matrix(new int[,] { { 11, 22 }, { 33, 44 } });
    public Matrix m2x2_B { get; } = new Matrix(new int[,] { { 55, 66 }, { 77, 88 } });
    public Matrix m1x3 { get; } = new Matrix(new int[,] { { 101, 202, 303 } });
    public Matrix m3x1 { get; } = new Matrix(new int[,] { { 101 }, { 202 }, { 303 } });
    public Matrix m1x5 { get; } = new Matrix(new int[,] { { 10, 20, 30, 40, 50 } });
    
    public Matrix m4x3_empty_last_row { get; } = new Matrix(
        new int[,]
        {
            { 10, 11, 12 },
            { 14, 15, 16 },
            { 18, 19, 20 },
            {  0,  0,  0 },
        });

    public Matrix m4x3_empty_last_col { get; } = new Matrix(
        new int[,]
        {
            { 10, 11, 0 },
            { 12, 13, 0 },
            { 14, 14, 0 },
            { 16, 17, 0 },
        });

    public Matrix m4x3_empty_last_row_and_col { get; } = new Matrix(
        new int[,]
        {
            { 10, 11, 0 },
            { 12, 13, 0 },
            { 14, 14, 0 },
            {  0,  0, 0 },
        });

    public Matrix m2x3_mul_m3x2 { get; } = new Matrix(new int[,] { { 14, 28 }, { 32, 64 } });
    public Matrix m3x2_mul_m2x3 { get; } = new Matrix(new int[,] { { 9, 12, 15 }, { 18, 24, 30 }, { 27, 36, 45 } });
    public Matrix m2x2_A_mul_m2x2_B { get; } = new Matrix(new int[,] { { 2299, 2662 }, { 5203, 6050 } });
    public Matrix m1x3_mul_m3x2 { get; } = new Matrix(new int[,] { { 1414, 2828 } });
    public Matrix m3x1_mul_m1x5 { get; } = new Matrix(new int[,]
        {
                { 1010, 2020, 3030, 4040, 5050 },
                { 2020, 4040, 6060, 8080, 10100 },
                { 3030, 6060, 9090, 12120, 15150 }
        });
    public Matrix m2x3_sum_m2x3 { get; } = new Matrix(new int[,] { { 2, 4, 6 }, { 8, 10, 12 } });
    public Matrix m1x5_sum_m1x5 { get; } = new Matrix(new int[,] { { 20, 40, 60, 80, 100 } });
}

[AllureNUnit]
[TestFixture(TestOf = typeof(Matrix), Description = "Matrix multiply tests")]
public class MatrixMulTest
{   
    TestMatrices tm = new TestMatrices();

    [AllureStep("{0} x {1} = {3}. {2} algorithm")]
    public static void CheckMultiply(Matrix A, Matrix B, MultiplyAlgorithm algorithm, Matrix ExpectedResult)
    {
        Matrix.MulAlg = algorithm;
        CollectionAssert.AreEqual(ExpectedResult.Data, (A * B).Data, "Р РµР·СѓР»СЊС‚Р°С‚ СѓРјРЅРѕР¶РµРЅРёСЏ РЅРµ СЃРѕРѕС‚РІРµС‚СЃС‚РІСѓРµС‚ РѕР¶РёРґР°РµРјРѕРјСѓ Р·РЅР°С‡РµРЅРёСЋ");
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestMultiply()
    {
        foreach (MultiplyAlgorithm alg in Enum.GetValues(typeof(MultiplyAlgorithm)))
        {
            CheckMultiply(tm.m2x3, tm.m3x2, alg, tm.m2x3_mul_m3x2);
            CheckMultiply(tm.m3x2, tm.m2x3, alg, tm.m3x2_mul_m2x3);
            CheckMultiply(tm.m1x3, tm.m3x2, alg, tm.m1x3_mul_m3x2);
            CheckMultiply(tm.m3x1, tm.m1x5, alg, tm.m3x1_mul_m1x5);
        };
    }

    [AllureStep("Check {0} x {1} throw ArgumentException")]
    public static void CheckWrongDimentionsMulRaise(Matrix A, Matrix B)
    {
        Assert.Catch<ArgumentException>(() => { var C = A * B; });
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestWrongDimentionsMul()
    {
        CheckWrongDimentionsMulRaise(tm.m1x3, tm.m2x3);
        CheckWrongDimentionsMulRaise(tm.m3x1, tm.m2x2_A);
    }
}

[AllureNUnit]
[TestFixture(TestOf = typeof(Matrix), Description = "Matrix sum tests")]
public class MatrixSumTest
{
    TestMatrices tm = new TestMatrices();

    [AllureStep("{0} + {1} = {3}")]
    public static void CheckSum(Matrix A, Matrix B, Matrix ExpectedResult)
    {
        CollectionAssert.AreEqual(ExpectedResult.Data, (A + B).Data, "Р РµР·СѓР»СЊС‚Р°С‚ СЃР»РѕР¶РµРЅРёСЏ РЅРµ СЃРѕРѕС‚РІРµС‚СЃС‚РІСѓРµС‚ РѕР¶РёРґР°РµРјРѕРјСѓ Р·РЅР°С‡РµРЅРёСЋ");
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestSum()
    {
        CheckSum(tm.m2x3, tm.m2x3, tm.m2x3_sum_m2x3);
        CheckSum(tm.m1x5, tm.m1x5, tm.m1x5_sum_m1x5);
    }

    [AllureStep("Check {0} + {1} throw ArgumentException")]
    public static void CheckWrongDimentionsSumRaise(Matrix A, Matrix B)
    {
        Assert.Catch<ArgumentException>(() => { var C = A + B; });
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestWrongDimentionsSum()
    {
        CheckWrongDimentionsSumRaise(tm.m1x3, tm.m2x3);
        CheckWrongDimentionsSumRaise(tm.m3x1, tm.m2x2_A);
    }
}

[TestFixture(TestOf = typeof(jMatrix), Description = "jMatrix Tests")]
[AllureNUnit]
public class jMatrixTest
{
    TestMatrices tm = new TestMatrices();

    [AllureStep("{0} convertation")]
    public static void CheckConvertation(Matrix M)
    {
        var converted = (Matrix)(new jMatrix(M));
        CollectionAssert.AreEqual(M.Data, converted.Data, "Matrix в†’ jMatrix в†’ Matrix convertation is not correct. Source and Target are not equal");
    }

    [Test(Description = "Matrix в†’ jMatrix в†’ Matrix convertation")]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestConvertation()
    {
        AllureLifecycle.Instance.WrapInStep(() =>
        {
            var A = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            Assert.Catch<ArgumentException>(() => { var J = new jMatrix(A); });
        }, "CheckNotSymmetricRaise");

        CheckConvertation(tm.Empty);
        CheckConvertation(tm.Random(100, 100, true));
        CheckConvertation(tm.Random(99, 99, true));
    }

    [Test(Description = "Check if indexator jMatrix[i,j] return correct matrix element")]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestIndexator()
    {
        var m = tm.Random(101, 101, true);
        var jm = new jMatrix(m);

        for (int i = 0; i < m.RowCount; i++)
            for (int j = 0; j < m.ColCount; j++)
                Assert.That(m[i, j] == jm[i, j]);
    }

    [AllureStep("Sum {0} + {1}")]
    public static void CheckSum(jMatrix A, jMatrix B)
    {
        var m_summ = (Matrix)A + (Matrix)B;
        var j_summ = A + B;

        CollectionAssert.AreEqual(m_summ.Data, ((Matrix)j_summ).Data);
    }

    [Test(Description = "Test jMatrix sum algorithm")]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestSum()
    {
        AllureLifecycle.Instance.WrapInStep(() =>
        {
            Assert.Catch<ArgumentException>(() => { var C = tm.m1x3 + tm.m1x5; });
        }, "CheckWrongDimentionsRaise");

        CheckSum((jMatrix)(tm.Empty), (jMatrix)(tm.Empty));

        CheckSum((jMatrix)(tm.Random(20, 20, true)), (jMatrix)(tm.Random(20, 20, true)));
        CheckSum((jMatrix)(tm.Random(99, 99, true)), (jMatrix)(tm.Random(99, 99, true)));
    }
}

[TestFixture(TestOf = typeof(rmMatrix), Description = "rmMatrix Tests")]
[AllureNUnit]
public class rmMatrixTest
{
    TestMatrices tm = new TestMatrices();

    [AllureStep("{0} convertation")]
    public static void CheckConvertation(Matrix M)
    {
        var converted = (Matrix)(new rmMatrix(M));
        CollectionAssert.AreEqual(M.Data, converted.Data, "Matrix в†’ rmMatrix в†’ Matrix convertation is not correct. Source and Target are not equal");
    }

    [Test(Description = "Matrix в†’ rmMatrix в†’ Matrix convertation")]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestConvertation()
    {
        CheckConvertation(tm.Empty);
        CheckConvertation(tm.Random(100, 100, false));
        CheckConvertation(tm.Random(99, 99, false));
        CheckConvertation(tm.Random(4, 3, false));
        CheckConvertation(tm.Random(3, 4, false));
        CheckConvertation(tm.m4x3_empty_last_row);
    }

    [Test(Description = "Check if indexator rmMatrix[i,j] return correct matrix element")]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestIndexator()
    {
        var m = tm.Random(101, 101, true);
        var jm = new rmMatrix(m);

        for (int i = 0; i < m.RowCount; i++)
            for (int j = 0; j < m.ColCount; j++)
                Assert.That(m[i, j] == jm[i, j]);
    }

    [AllureStep("Sum {0} + {1}")]
    public static void CheckSum(rmMatrix A, rmMatrix B)
    {
        var m_summ = (Matrix)A + (Matrix)B;
        var rm_summ = A + B;

        CollectionAssert.AreEqual(m_summ.Data, ((Matrix)rm_summ).Data);
    }

    [Test(Description = "Test rmMatrix sum algorithm")]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestSum()
    {
        AllureLifecycle.Instance.WrapInStep(() =>
        {
            Assert.Catch<ArgumentException>(() => { var C = (rmMatrix)tm.m1x3 + (rmMatrix)tm.m1x5; });
        }, "CheckWrongDimentionsRaise");

        CheckSum((rmMatrix)(tm.Empty), (rmMatrix)(tm.Empty));
        CheckSum((rmMatrix)(tm.Random(20, 20, false)), (rmMatrix)(tm.Random(20, 20, false)));
        CheckSum((rmMatrix)(tm.Random(99, 99, false)), (rmMatrix)(tm.Random(99, 99, false)));
        CheckSum((rmMatrix)(tm.Random(3, 4, false)), (rmMatrix)(tm.Random(3, 4, false)));
        CheckSum((rmMatrix)(tm.Random(4, 3, false)), (rmMatrix)(tm.Random(4, 3, false)));
        CheckSum((rmMatrix)(tm.m4x3_empty_last_row), (rmMatrix)(tm.m4x3_empty_last_row));        
        CheckSum((rmMatrix)(tm.m4x3_empty_last_row), (rmMatrix)(tm.Random(4, 3, false)));
        CheckSum((rmMatrix)(tm.Random(4, 3, false)), (rmMatrix)(tm.m4x3_empty_last_row));

        CheckSum((rmMatrix)(tm.m4x3_empty_last_col), (rmMatrix)(tm.m4x3_empty_last_col));
        CheckSum((rmMatrix)(tm.m4x3_empty_last_col), (rmMatrix)(tm.Random(4, 3, false)));
        CheckSum((rmMatrix)(tm.Random(4, 3, false)), (rmMatrix)(tm.m4x3_empty_last_col));

        CheckSum((rmMatrix)(tm.m4x3_empty_last_row_and_col), (rmMatrix)(tm.m4x3_empty_last_row_and_col));
        CheckSum((rmMatrix)(tm.m4x3_empty_last_row_and_col), (rmMatrix)(tm.Random(4, 3, false)));
        CheckSum((rmMatrix)(tm.Random(4, 3, false)), (rmMatrix)(tm.m4x3_empty_last_row_and_col));
    }

    [AllureStep("Multiply {0} * {1}")]
    public static void CheckMul(rmMatrix A, rmMatrix B)
    {
        var m_mul = (Matrix)A * (Matrix)B;
        var rm_mul = A * B;

        CollectionAssert.AreEqual(m_mul.Data, ((Matrix)rm_mul).Data);
    }

    [Test(Description = "Test rmMatrix mul algorithm")]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestMul()
    {
        AllureLifecycle.Instance.WrapInStep(() =>
        {
            Assert.Catch<ArgumentException>(() => { var C = (rmMatrix)tm.m1x3 * (rmMatrix)tm.m1x5; });
            Assert.Catch<ArgumentException>(() => { var C = (rmMatrix)tm.m1x3 * (rmMatrix)tm.m1x3; });
            Assert.Catch<ArgumentException>(() => { var C = (rmMatrix)tm.m3x2 * (rmMatrix)tm.m1x3; });
        }, "CheckWrongDimentionsRaise");

        CheckMul((rmMatrix)(tm.Empty), (rmMatrix)(tm.Empty));
        CheckMul((rmMatrix)(tm.m2x2), (rmMatrix)(tm.m2x2));
        
        CheckMul((rmMatrix)(tm.Random(20, 20, false)), (rmMatrix)(tm.Random(20, 20, false)));
        CheckMul((rmMatrix)(tm.Random(19, 19, false)), (rmMatrix)(tm.Random(19, 19, false)));
        CheckMul((rmMatrix)(tm.Random(3, 4, false)), (rmMatrix)(tm.Random(4, 3, false)));

        CheckMul((rmMatrix)(tm.m4x3_empty_last_row), (rmMatrix)(tm.Random(3, 4, false)));
        CheckMul((rmMatrix)(tm.Random(3, 4, false)), (rmMatrix)(tm.m4x3_empty_last_row));

        CheckMul((rmMatrix)(tm.m4x3_empty_last_col), (rmMatrix)(tm.Random(3, 4, false)));
        CheckMul((rmMatrix)(tm.Random(3, 4, false)), (rmMatrix)(tm.m4x3_empty_last_col));

        CheckMul((rmMatrix)(tm.m4x3_empty_last_row_and_col), (rmMatrix)(tm.Random(3, 4, false)));
        CheckMul((rmMatrix)(tm.Random(3, 4, false)), (rmMatrix)(tm.m4x3_empty_last_row_and_col));
        
    }
}
