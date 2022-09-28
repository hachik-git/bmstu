using MatrixMult;
namespace MatrixMult.Tests
{

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Matrix A = new MatrixMult.Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            var B = new Matrix(3, 2);

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                    B[i, j] = (i + 1) * (j + 1);

            var Control = new MatrixMult.Matrix(new int[,] { { 14, 28 }, { 32, 64 } });

            var C = A * B;
            
            Matrix.MulAlg = MultiplyAlgorithm.Vinograd;            
            C = A * B;

            Assert.AreEqual(C, Control);

        }
    }
}