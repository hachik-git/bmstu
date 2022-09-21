using Rasterisation;

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
            using (Graphics g = this.CreateGraphics())
            {
                var r = new Rasterisator(g);
                r.DrawLine(new Point(10, 10), new Point(300, 100), DraLineAlgorithm.DDA);
                r.DrawLine(new Point(10, 30), new Point(300, 120), DraLineAlgorithm.Default);
                r.DrawLine(new Point(10, 50), new Point(300, 140), DraLineAlgorithm.DDA2);
            }

            using (Graphics g = Graphics.FromImage(new Bitmap(1, 1)))
            {
                var r = new Rasterisator(g);
                var m = new TimeMeter();
                (var p1, var p2) = (new Point(10, 10), new Point(300, 100));

                textBox1.Text = m.Measure(r.DrawLine, 100, p1, p2, DraLineAlgorithm.DDA, null).ToString();
                textBox2.Text = m.Measure(r.DrawLine, 100, p1, p2, DraLineAlgorithm.Default, null).ToString();
                textBox3.Text = m.Measure(r.DrawLine, 100, p1, p2, DraLineAlgorithm.DDA2, null).ToString();
            }
        }
    }
}