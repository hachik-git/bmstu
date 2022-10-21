п»ї#include <iostream>
#include <math.h>
using namespace std;
// baza func
typedef double (*fun) (double);

double  f0(double x) { if (cos(x / 2))     return  sin(x / 2) / cos(x / 2) - x; }
double  f1(double x) { return x * sin(x) + cos(x); }
double f2(double x) { return  -3 * x * cos(x); }
double f3(double x) { return  cos(x) * x; }

class Point {
	double x, y;
public:
	double rast() { return sqrt(x * x + y * y); }
	double rast_t(Point& t1) {
		return
			sqrt((t1.x - x) * (t1.x - x) + (t1.y - y) * (t1.y - y));
	}

	double get_x() { return x; }
	double get_y() { return y; }
	Point() { x = 0; y = 0; }
	Point(double xv, double yv) { x = xv; y = yv; }
	Point(Point& t) { x = t.x; y = t.y; }
	void set(double xv, double yv) { x = xv; y = yv; }
};


class Baza_tab {
	float a, b;
	Point* p;  int n;
	Point* p_in;	  int kol_in;
public:
	void tabul_v(double  (*f) (double x))
	{
		double h, x, y; int i;
		for (i = 0, h = (b - a) / n, x = a; i < n; i++)
		{
			y = f(x);
			p[i] = Point(x, y);
			x += h;
		}
	}

	//Р’С‚РѕСЂРѕР№ РјРµС‚РѕРґ вЂ“ РІРѕР·РІСЂР°С‰Р°РµС‚ СѓРєР°Р·Р°С‚РµР»СЊ 
	Point* tabul(double  (*f) (double x))
	{
		double h, x, y; int i;
		for (i = 0, h = (b - a) / n, x = a; i < n; i++) {
			p[i] = Point(x, f(x));
			x += h;
		}
		return p;
	}
	// РїРѕРёСЃРє РёРЅС‚РµРІР°Р»РѕРІ РЅСѓР»РµР№ 	С„СѓРЅРєС†РёРё  РїРѕС‚РѕРј СЂР°СЃСЃРјРѕС‚СЂРёРј 
	Point* interwal_nul() {
		int i, j;
		Point* dt = new Point[n];
		for (i = 0, j = 0; i < n - 2; i++)
			if (p[i].get_y() * p[i + 1].get_y() < 0)
				dt[j++] = Point(p[i].get_x(), p[i + 1].get_x());


		p_in = new Point[j];
		for (i = 0; i < j; i++) 
			p_in[i] = dt[i];

		kol_in = j;
		delete[] dt;   return p_in;
	}

	Baza_tab() {	}
	Baza_tab(double av, double bv, int nv) {
		a = av; b = bv; n = nv; 	   p = new Point[n];
		std::cout << "  construct  ";  kol_in = 0;
	}
	int get_n() { return n; }
	float get_a() { return a; }
	float get_b() { return b; }
	void get_p() {
		int i;  std::cout << std::endl;
		for (i = 0; i < n; i++)
			std::cout << p[i].get_x() << "  " << p[i].get_y() << std::endl;
	}
};


void  main() {

	int i, j, n, m, k; double a, b, s, shag;
	fun mas_f[] = { f0,f1, f2,f3 };

	Baza_tab  fp, fv;   Point* tv, * t0, * te;
	k = 4;   Baza_tab z[4];

	fp = Baza_tab(-9, 9, 10);

	tv = fp.tabul(f1);
	for (j = 0; j < fp.get_n(); j++)
		cout << tv[j].get_x() << "  " << tv[j].get_y() << endl;

	cout << "   metod tabul_v  " << endl;
	fp.tabul_v(f1);	 fp.get_p();

	m = 4;
	fv = Baza_tab(-20, 20, 10);

	for (i = 0; i < m; i++) {

		tv = fv.tabul(mas_f[i]);
		cout << "   fun   " << i << endl;
		for (j = 0; j < fv.get_n(); j++)
			cout << tv[j].get_x() << "  " << tv[j].get_y() << endl;
	}
	cout << "   massiv   " << endl;
	for (i = 0; i < k; i++) {
		z[i] = Baza_tab(-20, 20, 10);
		cout << endl << "  z[i]  " << i << endl;
		z[i].tabul_v(mas_f[i]);
		z[i].get_p();
	}


	/* РёСЃРїРѕР»СЊР·РѕРІР°РЅРёРµ СѓРєР°Р·Р°С‚РµР»СЏ РїРѕРєР° РЅРµ РЅР°РґРѕ

	   fv=new Baza_tab (-9,9, 10);
	   fv->tabul_v(f1);
	   fv->get_p();


	   tv=fv->tabul(f1);
		cout<< "    massiv 2222  "<<endl;
	   for (j=0;  j<fv->get_n(); j++)
		 cout<< tv[j].get_x()<< "  "<< tv[j].get_y()<< endl;
	*/
	cin >> n;

}

