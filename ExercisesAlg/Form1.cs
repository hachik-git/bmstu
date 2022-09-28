using MatrixMult;
using TimeMeter;
namespace ExercisesAlg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var A = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });            
            var B = new Matrix(3, 2);
            
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                    B[i,j] = (i+1) * (j+1);

            //A = Matrix.GenMatrix(1500, 1000);
            //B = Matrix.GenMatrix(1000, 1500);
            
            TimeMeter.TimeMeter m = new TimeMeter.TimeMeter();            
            MessageBox.Show(m.Measure(() => { var C = A * B; }, 20).ToString());
            Matrix.MulAlg = MultiplyAlgorithm.Vinograd;
            MessageBox.Show(m.Measure(() => { var C = A * B; }, 20).ToString());
        }
    }
}