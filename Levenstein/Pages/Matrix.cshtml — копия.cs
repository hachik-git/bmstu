п»їusing Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace Algorithm.Matrix
{
    public class MatrixModel : PageModel
    {
        private readonly ILogger<MatrixModel> _logger;
        public int m, n, q;
        public Matrix? A, B, C, JM, MT;
        public Matrix? S;
        public jMatrix? J, SJ;
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

            if (this.A.IsSymmetric)
            {
                J = (jMatrix)this.A;
                var JA = (jMatrix)this.A;

                JM = (Matrix)J;

                S = this.A + this.B;
                if (this.B.IsSymmetric)
                {
                    var JB = (jMatrix)this.B;
                    SJ = JA + JB;
                    MT = (Matrix)SJ;
                }
            }
        }
    }
}