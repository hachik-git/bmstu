п»ї#include <iostream>
#include <vector>
#include "../Figures/Figures.cpp"
/*
typedef double(*CurveFunc) (double);
vector<Point> TabInterval(CurveFunc func, double a, double b, int qnt);

double FindRoot(CurveFunc func, double a, double b, double precise);
*/

int main()
{
    //printf("%f", FindRoot(parabola, 0, 10, 1));
    cout << "Points: " << endl;
    vector<Point> pts = TabFunc(f0, -20, 20, 10);
    for (int i = 0; i < pts.size(); i++)
        cout << "(" << pts[i].X << " , " << pts[i].Y << ")" << endl;    

    cout << endl << endl << "Intervals: " << endl ;
    vector<Line> ints = GetNulIntervals(pts);
    for (int i = 0; i < ints.size(); i++)
        cout << "(" << ints[i].Start.X << " , " << ints[i].Start.Y << ") - "
            << "(" << ints[i].End.X << " , " << ints[i].End.Y << ")" << endl;
    
    pts.clear();
    ints.clear();

    cout << DefIntegral(polukrug, 0, 1, 20, dicmRect) << endl;
    cout << DefIntegral2(polukrug, 0, 1, 20, dicmRect);
    //cout << DefIntegral(sin, 0, 90, 100000, dicmRect);
}

/*
double FindRoot2(CurveFunc func, double a, double b, double precise)
{
    double fa;
    double fb;
    double xt;
    int n = 0;

    fa = func(a);
    fb = func(b);
    xt = func(a) * ((b - a) / (func(b) - func(a)));
    double fx = func(xt);

    while (fabs(fx) > precise)
    {
        fa = func(a);
        fb = func(b);
        
        if (fa * fx < 0)
        {
            b = xt;
            fb = fx;
        }
        else
        {
            a = xt;
            fb = fx;
        }
        xt = a - fa*((b - a) / (fb - fa));
    }
    return b-a;
}

double FindRoot3(CurveFunc func, double a, double b, double precise)
{
    double fa;
    double fb;
    double xt;
    double fx;
    double xp;
    int n = 0, d = 1;

    fa = func(a);
    fb = func(b);
    xp = fa * ((b - a) / (fb - fa));
    fx = func(xp);

    while (fabs(fx) > precise && fabs(d) > precise && n < 1000)
    {
        fa = func(a);
        fb = func(b);

        if (fa * fx < 0)
        {
            b = xt;
            fb = fx;
        }
        else
        {
            a = xt;
            fb = fx;
        }
        xt = a - fa*((b - a) / (fb - fa));
        fx = func(xt);
        d = xp - xp;
        xp = xt;
        n++;
    }
    return b - a;
}

double FindRoot(CurveFunc func, double a, double b, double precise)
{
    double fa;
    double fb;
    double fc;
    double c;

    while (fabs(b - a) > precise)
    {
        c = (b - 1) / 2;
        fa = func(a);
        fb = func(b);
        fc = func(c);
        if (fc * fb > 0)
            a = c;
        else
            b = c;
    }
    return b - a;
}

*/