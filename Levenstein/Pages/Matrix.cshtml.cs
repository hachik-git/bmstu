п»їusing Microsoft.AspNetCore.Mvc.RazorPages;

namespace Algorithm.Matrix
{
    public class MatrixModel : PageModel
    {
        private readonly ILogger<MatrixModel> _logger;
        public int m, n, q;
        public Matrix A, B, C;
        public Matrix? S;
        public MultiplyAlgorithm Algorithm;
        public MatrixModel(ILogger<MatrixModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int m, int n, int q, int algorithm, int[] A, int[] B)
        {
            (this.m, this.n, this.q) = (m, n, q);
            (this.A, this.B) = (new Matrix(m, n, A), new Matrix(n, q, B));
            Algorithm = Matrix.MulAlg = (MultiplyAlgorithm)algorithm;
            C = this.A * this.B;

            var J = (jMatrix)this.A;

            int ms = sizeof(int) * this.A.RowCount * this.A.ColCount;
            int js = sizeof(int) * J.Counter.Length + sizeof(int) * J.Elements.Length;

            var mt = (Matrix)J;
            int t = J[3, 4];
            J[1, 2] = 10;
        }
    }
}