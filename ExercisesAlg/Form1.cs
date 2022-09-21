namespace ExercisesAlg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            (int M, int N, int Q) = (2, 3, 4);
            var A = new int[M, N];
            var B = new int[N, Q];

            for (int i = 0; i < M; i++)
                for (int j = 0; j < Q; j++)
                    for (int k = 0; k < Q; k++)
                    {

                    }    
        }
    }
}