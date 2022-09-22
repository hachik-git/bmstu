#include <iostream>
#include <iterator>
using namespace std;

/*#define min(x,y) ((x) < (y) ? (x) : (y))
#define max(x,y) ((x) > (y) ? (x) : (y))
#define min3(a,b,c) ((a)< (b) ? min((a),(c)) : min((b),(c)))
#define min4(k,m,n,e) ((k)< (m) ? min3((k),(n),(e)) : min3((m),(n),(e)))
#define cmp(a,b) ((a) == (b) ? 0 : 1)
#define d(i,j) d[(i) * (m+1) + (j)]*/


int* vvod_massiva(char name, int &n);
void print_massiv(int* a, int n, char name);
float avg(int* a, int qnt);
int* a_cross(int* a, int* b, int n, int m, int &res_length);
int* a_union(int* a, int* b, int n, int m);
int* a_distinct(int* a, int n, int& res_length);
int* a_minus(int* a, int* b, int n, int m, int& res_length);
int* a_difference(int* a, int* b, int n, int m, int& res_length, bool distinct = false);

int** vvod_matrix(int m, int n, char name);
void print_matrix(int** matrix, int n, int m);

float* avg(int** matrix, int m, int n);
float vector_scalar(int* a, int* b, int n);
float* matrix_scalar(int** a, int** b, int m, int n);

int main()
{
    int* a, * b;
    int n = 0, m = 0;

    a = vvod_massiva('A', n);
    b = vvod_massiva('B', m);
    
    print_massiv(a, n, 'A');
    print_massiv(b, m, 'B');

    cout << "\navg in A: " << avg(a, n);
    cout << "\navg in B: " << avg(b, m);

    int c_len;
    int* c = a_cross(a, b, n, m, c_len);
    cout << "\nA cross B: ";
    print_massiv(c, c_len, 'C');

    int* u = a_union(a, b, n, m);
    cout << "\nA union B: ";
    print_massiv(u, m+n, 'U');

    int d_len;
    int* d = a_distinct(a, n, d_len);
    cout << "\nA distinct: ";
    print_massiv(d, d_len, 'D');

    int m1_len;
    int* m1 = a_minus(a, b, n, m, m1_len);
    cout << "\nA - B: ";
    print_massiv(m1, m1_len, 'M1');

    int m2_len;
    int* m2 = a_minus(b, a, m, n, m2_len);
    cout << "\nB - A: ";
    print_massiv(m2, m2_len, 'M2');

    int dif_len;
    int* dif = a_difference(a, b, n, m, dif_len, false);
    cout << "\nA dif B: ";
    print_massiv(dif, dif_len, 'DIF');

    int dif_unq_len;
    int* dif_unq = a_difference(a, b, n, m, dif_unq_len, true);
    cout << "\nA dif B unique: ";
    print_massiv(dif_unq, dif_unq_len, 'DIFU');

    /*cout << "\nA*A: " << vector_scalar(a, a, n);

    cout << "\n\nEnter matrix row count: ";
    cin >> n;

    cout << "Enter matrix col count: ";
    cin >> m;

    cout << "Enter matrix:\n";
    int** matrix = vvod_matrix(m, n, 'm');
    cout << "\nMatrix:\n";

    print_matrix(matrix, m, n);

    float* m_avg = avg(matrix, m, n);
    cout << "\nMatrix AVG: ";
    for (int i = 0; i < m; i++)
        cout << m_avg[i] << " ";

    float* m_scaler = matrix_scalar(matrix, matrix, m, n);

    cout << "\nMatrix scalar: ";
    for (int i = 0; i < m; i++)
        cout << m_scaler[i] << " "; */
}

int* vvod_massiva(char name, int &n)
{
    cout << "Enter array " << name << " length : ";
    cin >> n;

    cout << "Enter array " << name << endl;
    int* res = new int[n];

    for (int i = 0; i < n; i++)
    {
        cout << name << "[" << i << "]: ";
        cin >> res[i];
    }

    return res;
}

int** vvod_matrix(int m, int n, char name)
{
    int** res = new int* [m];

    for (int i = 0; i < m; i++)
        res[i] = new int[n];

    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < n; j++)
        {
            cout << name << "[" << i << "," << j << "]: ";
            cin >> res[i][j];
        }
    }

    return res;
}

void print_massiv(int* a, int n, char name)
{
    cout << name << " = [";
    for (int i = 0; i < n; i++)
    {
        cout << a[i];
        if (i != n - 1)
            cout << ",";
    }
    cout << "]\n";
}

void print_matrix(int** matrix, int n, int m)
{
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < m; j++)
            cout << matrix[i][j] << " ";

        cout << "\n";
    }
}

float avg(int* a, int qnt)
{
    float res = 0;
    for (int i = 0; i < qnt; i++)
        res += a[i];

    return res / qnt;
}

int getIndex(int *a, int elem, int qnt)
{
    for (int i = 0; i < qnt; i++)
        if (a[i] == elem)
            return i;
    return -1;
}

int* a_cross(int* a, int* b, int n, int m, int& res_length)
{
    int* tmp = new int[min(n, m)];
    res_length = 0;

    for (int i = 0; i < n; i++)
        if ((getIndex(b, a[i], m) != -1) && (getIndex(tmp, a[i], res_length) == -1))
            tmp[res_length++] = a[i];

    int * res = new int[res_length];
    std::copy_n(tmp, res_length, res);
    
    delete[] tmp;

    return res;
}

int* a_union(int* a, int* b, int n, int m)
{
    int *res = new int[n + m];

    std::copy_n(a, n, res);
    std::copy_n(b, m, &res[n]);

    return res;
}

int* a_distinct(int* a, int n, int &res_length)
{
    int* tmp = new int[n];
    res_length = 0;

    for (int i = 0; i < n; i++)
        if (getIndex(tmp, a[i], n) == -1)
            tmp[res_length++] = a[i];

    int* res = new int[res_length];
    std::copy_n(tmp, res_length, res);
    
    delete [] tmp;

    return res;
}

int* a_minus(int* a, int* b, int n, int m, int& res_length)
{
    int* tmp = new int[n];
    res_length = 0;

    for (int i = 0; i < n; i++)
        if (getIndex(b, a[i], m) == -1)
            tmp[res_length++] = a[i];

    int* res = new int[res_length];
    std::copy_n(tmp, res_length, res);

    delete[] tmp;

    return res;
}

int* a_difference(int* a, int* b, int n, int m, int& res_length, bool distinct)
{
    int l1 = 0, l2 = 0, rl = 0;
    int *u = a_union(a_minus(a, b, n, m, l1), a_minus(b, a, m, n, l2), l1, l2);
    if (!distinct)
    {
        res_length = l1 + l2;
        return u;
    }
    else
        return a_distinct(u, l1 + l2, res_length);
}

float* avg(int** matrix, int m, int n)
{
    float* m_out = new float[m];

    float res = 0;
    for (int i = 0; i < m; i++)
    {
        res = 0;
        for (int j = 0; j < n; j++)
        {
            res += matrix[i][j];
        }
        m_out[i] = res / n;
    }
    return m_out;
}

float vector_scalar(int* a, int* b, int n)
{
    float res = 0;

    for (int i = 0; i < n; i++)
        res += a[i] * b[i];

    res = sqrt(res);

    return res;
}

float* matrix_scalar(int** a, int** b, int m, int n)
{
    float* res = new float[m];

    for (int i = 0; i < m; i++)
        res[i] = vector_scalar(a[i], b[i], n);

    return res;
}