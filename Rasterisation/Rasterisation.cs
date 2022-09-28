п»їusing System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rasterisation
{
    public enum DraLineAlgorithm
    {
        Default,
        DDA,
        DDA2,
    }
    
    public class Rasterisator
    {
        Graphics Graphics { get; set; } 
        public Brush DefaultBrush = new SolidBrush(Color.Black);
        public Rasterisator(Graphics graphics)
        {
            Graphics = graphics;
        }

        public void DrawLine(Point Start, Point End, DraLineAlgorithm algorithm, Brush? Brush = null)
        {
            var brush = Brush ?? DefaultBrush;
            switch (algorithm)
            {
                case DraLineAlgorithm.DDA:
                    float l = Math.Max(End.X - Start.X, End.Y - Start.Y);
                    float dx = (End.X - Start.X) / l;
                    float dy = (End.Y - Start.Y) / l;

                    (float x, float y) = (Start.X, Start.Y);

                    for (int i = 1; i <= l + 1; i++)
                        Graphics.FillRectangle(brush, (int)Math.Round(x += dx), (int)Math.Round(y += dy), 1, 1);

                    break;

                case DraLineAlgorithm.DDA2:
                    float l2 = Math.Max(End.X - Start.X, End.Y - Start.Y);
                    float dx2 = (End.X - Start.X) / l2;
                    float dy2 = (End.Y - Start.Y) / l2;

                    (float x2, float y2) = (Start.X, Start.Y);

                    for (int i = 1; i <= l2 + 1; i++)
                    {
                        Graphics.FillRectangle(brush,
                            (int)(x2 += (dx2 + (float)0.5)),
                            (int)(y2 += (dy2 + (float)0.5)),
                            1, 1);
                    }

                    break;

                case DraLineAlgorithm.Default:
                    Graphics.DrawLine(new Pen(brush), Start, End);
                    break;
            }
        }
    }
}
