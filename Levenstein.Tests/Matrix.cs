using Algorithm.Matrix;

namespace Algorithm.Matrix.Tests;

[TestClass]
public class MatrixText
{

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