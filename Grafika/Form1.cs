namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            Brush aBrush = (Brush)Brushes.Black;
            Graphics g = this.CreateGraphics();

            Point start = new Point(10, 10);
            Point end = new Point(300, 100);
            
            float l = Math.Max(end.X - start.X, end.Y - start.Y);
            float dx = (end.X - start.X) / l;
            float dy = (end.Y - start.Y) / l;

            (float x, float y) = (start.X, start.Y);

            for (int i = 1; i <= l+1; i++)
                g.FillRectangle(aBrush, x += dx, y += dy, 1, 1);

            Point start1 = new Point(10, 20);
            Point end1 = new Point(300, 110);

            Pen pen = Pens.Black;
            g.DrawLine(pen, start1, end1);
        }
    }
}