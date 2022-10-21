п»ї#define _USE_MATH_DEFINES
#include <iostream>
#include <vector>
#include <math.h>
using namespace std;
//# define PI           3.14159265358979323846;

class Point
{
public:
	string Name;
	double X, Y;
	Point() : X(0), Y(0) { ; }
	Point(double x, double y) : X(x), Y(y) { ; }

	double DistanceTo(Point& Another)
	{
		return sqrt(pow(X - Another.X, 2) +
			pow(Y - Another.Y, 2));
	}

	double Distance()
	{
		Point Origin = Point(0, 0);
		return DistanceTo(Origin);
	}
};

class Rectangle
{
public:
	Point A, B, C, D;

	Rectangle(Point& a, Point& c)
	{
		this->A = Point(a.X, a.Y);
		this->B = Point(a.X, c.Y);
		this->C = Point(c.X, c.Y);
		this->D = Point(c.X, a.Y);
	}

	Rectangle(int x1, int y1, int x2, int y2)
	{
		this->A = Point(x1, y1);
		this->B = Point(x1, y2);
		this->C = Point(x2, y2);
		this->D = Point(x2, y1);
	}

	double square()
	{
		return A.DistanceTo(B) * B.DistanceTo(C);
	}
};

class Line {
public:
	string Name;
	Point Start, End;

	Line(Point start, Point end) : Start(start), End(end) { ; }

	double Length()
	{
		return Start.DistanceTo(End);
	}
};

class Shape
{

};

class Circle {
public:
	string Name;
	Point Center;
	double Radius;

	Circle(Point center, double radius) : Center(center), Radius(radius) { ; }

	double Square() {
		return Radius * Radius * M_PI;
	}
};



class PointArray
{
public:
	vector<Point> Points;

	int AddPoint(Point p)
	{
		Points.push_back(p);
		return Points.size();
	}

	int** DistanceMatrix() {
		int** dsts = new int* [Points.size()];
		for (int i = 0; i < Points.size(); i++)
			dsts[i] = new int[Points.size()];

		for (int i = 0; i < Points.size(); i++)
			for (int j = 0; j < Points.size(); j++)
				dsts[i][j] = Points[i].DistanceTo(Points[j]);

		return dsts;
	}

	vector<vector<float>> DistanceMatrixV() {
		vector<vector<float>> v;
		vector<float> vi;

		for (int i = 0; i < Points.size(); i++)
		{
			vi.clear();
			for (int j = 0; j < Points.size(); j++)
				vi.push_back(Points[i].DistanceTo(Points[j]));
			v.push_back(vi);
		}

		return v;
	}
};

typedef double(*CurveFunc) (double);

typedef enum DefIntegralCalcMethod {
	dicmRect,
	dicmTrap,
	dicmParab
};

vector<Point> TabFunc(CurveFunc func, double a, double b, int qnt);
vector<Line> GetNulIntervals(vector<Point> points);
vector<Line> GetNulIntervals(CurveFunc func, double a, double b, int qnt);

double GetSquare(CurveFunc func, double a, double b, DefIntegralCalcMethod method = dicmRect)
{
	double s;
	switch (method)
	{
	case dicmRect: s = abs(func((b + a) / 2) * (b - a)); break;
	case dicmTrap: s = (func(a) + func(b) * (b - a)) / 2; break;  // РІРѕР·РјРѕР¶РЅРѕ РЅРµРІРµСЂРЅРѕРµ
	//case dicmParab: ((i % 2) ? 2 : 4)* func(x)* (О”x / 3); break; // РЅРµРІРµСЂРЅРѕРµ
	default: return 0;
	};
	/*cout << "a: " << a << ", b: " << b << "; x=" << (b - a) << "; y=" << func((b + a) / 2) <<
		"; sin=" << sin((b + a) / 2) << endl;*/
	return s;
	//https://ru.wikipedia.org/wiki/Р§РёСЃР»РµРЅРЅРѕРµ_РёРЅС‚РµРіСЂРёСЂРѕРІР°РЅРёРµ#РњРµС‚РѕРґ_РїСЂСЏРјРѕСѓРіРѕР»СЊРЅРёРєРѕРІ
}

double DefIntegral(CurveFunc func, double a, double b, int split, DefIntegralCalcMethod method)
{
	double s = 0;
	double О”x = (b - a) / split;
	double x = a;

	for (int i = 0; i < split; i++, x += О”x)
		s += GetSquare(func, x, x + О”x, dicmRect);

	return s;
}

double DefIntegral2(CurveFunc func, double a, double b, int split, DefIntegralCalcMethod method)
{
	vector<Point> points = TabFunc(func, a, b, split);
	double s = 0;
	for (int i = 0; i < points.size()-1; i++)
		s += DefIntegral(func, points[i].X, points[i+1].X, 1, dicmRect);
		//https://ru.wikipedia.org/wiki/Р§РёСЃР»РµРЅРЅРѕРµ_РёРЅС‚РµРіСЂРёСЂРѕРІР°РЅРёРµ#РњРµС‚РѕРґ_РїСЂСЏРјРѕСѓРіРѕР»СЊРЅРёРєРѕРІ

	return s;
}

double pryamaya(double x) {
	return x;
}

double polukrug(double x) {
	return sqrt(1 - (x * x));
}

double ikssinus(double x) {
	return x * sin(x);
}

double  f0(double x) {
	if (cos(x / 2))
		return  sin(x / 2) / cos(x / 2) - x; 
}

vector<Point> TabFunc(CurveFunc func, double a, double b, int qnt)
{
	vector<Point> p;
	double d = (b - a) / (qnt-1);
	double x = a;

	for (int i = 0; i < qnt; x += d, i++)
		p.push_back(Point(x, func(x)));

	return p;
}

vector<Line> GetNulIntervals(vector<Point> points)
{
	vector<Line> lines;

	for (int i = 0; i < points.size() - 2; i++)
		if ((points[i].Y * points[i + 1].Y < 0) || (points[i + 1].Y == 0))
			lines.push_back(Line(Point(points[i].X, 0), Point(points[i + 1].X, 0)));

	return lines;
}

vector<Line> GetNulIntervals(CurveFunc func, double a, double b, int qnt)
{
	vector<Point> points = TabFunc(func, a, b, qnt);
	return GetNulIntervals(points);
	points.clear();
}
/*
int main()
{
	CurveFunc* funcs = new CurveFunc[3] { pryamaya, polukrug, ikssinus };

	for (int i = 0; i < 3; i++) {
		DefIntegral(funcs[i], 0, 1, 2000, dicmRect);
		cout << "Rect: ";
		cout << DefIntegral(funcs[i], 0, 1, 2000, dicmRect);
		cout << "; Trap: " << DefIntegral(funcs[i], 0, 1, 2000, dicmTrap);
		cout << "; Parab: " << DefIntegral(funcs[i], 0, 1, 2000, dicmParab);
		cout << endl << "******************************" << endl;
	}

	return 0;

	Point p1 = Point(0, 0);
	Point p2 = Point(10, 10);
	Rectangle r = Rectangle(p1, p2);

	cout << "Distance p1-p2: " << p1.DistanceTo(p2) << endl;
	cout << "Square of r is: " << r.square() << endl;

	PointArray pa = PointArray();
	pa.AddPoint(r.A);
	pa.AddPoint(r.B);
	pa.AddPoint(r.C);
	pa.AddPoint(r.D);

	vector<vector<float>> dsts = pa.DistanceMatrixV();
	for (int i = 0; i < dsts.size(); i++)
		for (int j = 0; j < dsts.size(); j++)
			cout << "[" << i << "," << j << "] = " << dsts[i][j] << endl;
}*/