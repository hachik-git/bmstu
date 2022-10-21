п»їusing Microsoft.AspNetCore.Mvc.RazorPages;
using Matrices;
using Matrices.Classes;

namespace Algorithm.Matrices
{
    public class MatrixModel : PageModel
    {
        private readonly ILogger<MatrixModel> _logger;
        public int m, n, q;
        public Matrix? A, B;
        public MatrixModel(ILogger<MatrixModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int m, int n, int q, int algorithm, int[] A, int[] B)
        {
            (this.m, this.n, this.q) = (m, n, q);
            (this.A, this.B) = (new Matrix(m, n, A) { Name = "A" }, new Matrix(n, q, B) { Name = "B" });

            var M = new Matrix(new int[,] {
                {  0,   0,   0,   0,   0,   0 },
                {  0,  10,   0,  20,   0,   0 },
                { 30,   0,   0,  80,   0,  90 },
                {  0,   0,  50,   0,   0,  60 },
                { 40,   0,   0,  70,   0,   0 },
            });

            var rm = (rmMatrix)(M);
            int i = rm.GetElementCol(3);

            var M2 = new Matrix(new int[,] {
                {  0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0 },
                {  0,   0,   0,  10,   0,   0,   0,  30,   0,   0,   0,  60,   0,  0 },
                {  0,   0,   0,   0,   0,   0,   0,   0,   0,  90,   0,  60,   0,  0 },
                {  0,   0,   0,  90,   0,   0,  40,   0,   0,   0,   0,   0,   0,  0 },
            });

            var cg = new cgMatrix(M2);

            var j = new jMatrix
            (
                new List<int> { 0, 100, 110, 120, 130, -70, 0, 20, 200 },
                new int[] { 0,1,4,7,8}
            );
            var mm = (Matrix)j;
        }
    }
}