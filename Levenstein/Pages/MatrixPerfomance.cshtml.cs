п»їusing Matrices;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Algorithm.Matrices
{
    public class MatrixPerfomanceModel : PageModel
    {
        private readonly ILogger<MatrixModel> _logger;
        public int qnt;
        public long[,] meassures = new long[3, 5];
        public MatrixPerfomanceModel(ILogger<MatrixModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int? qnt)
        {
            int q = (int)(((qnt ?? 0) == 0) ? 10 : qnt);            
            var m = new TimeMeter.TimeMeter();

            for (int i = 1; i < 5; i++)
            {
                (var A, var B) = (Matrix.GenMatrix(q * i, q * i), Matrix.GenMatrix(q * i, q * i));

                meassures[0, i] = m.Measure(() => { var C = A * B; }, 100);

                Matrix.MulAlg = MultiplyAlgorithm.Vinograd;
                meassures[1, i] = m.Measure(() => { var C = A * B; }, 100);
            }
        }
    }
}