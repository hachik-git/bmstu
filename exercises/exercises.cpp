// exercises.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
using namespace std;

#define min(x,y) ((x) < (y) ? (x) : (y))
#define max(x,y) ((x) > (y) ? (x) : (y))
#define min3(a,b,c) ((a)< (b) ? min((a),(c)) : min((b),(c)))
#define min4(k,m,n,e) ((k)< (m) ? min3((k),(n),(e)) : min3((m),(n),(e)))
#define cmp(a,b) ((a) == (b) ? 0 : 1)
#define d(i,j) d[(i) * (m+1) + (j)]


int* vvod_massiva(int n, char name);
int** vvod_matrix(int m, int n, char name);
void print_massiv(int* a, int n);
void print_matrix(int** matrix, int n, int m);
float avg(int* a, int qnt);
float* avg(int** matrix, int m, int n);
float vector_scalar(int* a, int* b, int n);
float* matrix_scalar(int** a, int** b, int m, int n);

int distLevenshtein(char* s1, char* s2, int n, int m)
{
    int* d = new int(sizeof((n + 1) * (m + 1)));
    int i, j;
    for (i = 0; i <= n; i++)
    {
        for (j = 0; j <= m; j++)
        {
            if (i == 0 and j == 0) {
                d(i, j) = 0;
                printf("%2d ", d(i, j));
            }
            else if (i > 0 and j == 0) {
                d(i, j) = i;
                printf("%2d ", d(i, j));
            }
            else if (j > 0 and i == 0) {
                d(i, j) = j;
                printf("%2d ", d(i, j));
            }
            else {
                d(i, j) = min3(
                    d(i, j - 1) + 1,
                    d(i - 1, j) + 1,
                    d(i - 1, j - 1) + cmp(s1[i], s2[j])
                );
                printf("%2d ", d(i, j));
            }
        }
        printf("\n");
    }
    free(d);
    return d[(m + 1) * (n + 1) - 1];

}

int main()
{
    char first_string[] = "aabqq";
    char second_string[] = "aaczq";

    int result1 = distLevenshtein(first_string, second_string, 5, 5);
    printf("\n%d", result1);
    return 0;

    int* a, * b;
    int n, m;

    std::cout << "Enter array A length: ";
    std::cin >> n;

    std::cout << "Enter array A:\n";
    a = vvod_massiva(n, 'a');

    std::cout << "Enter array B length: ";
    std::cin >> m;

    std::cout << "Enter array B:\n";
    b = vvod_massiva(m, 'b');

    cout << "a = ";
    print_massiv(a, n);
    cout << "\n";

    cout << "b = ";
    print_massiv(b, m);
    cout << "\n";

    cout << "\navg in A: " << avg(a, n);
    cout << "\navg in B: " << avg(b, m);

    cout << "\nA*A: " << vector_scalar(a, a, n);

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
        cout << m_scaler[i] << " ";
}

int* vvod_massiva(int n, char name)
{
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

void print_massiv(int* a, int n)
{
    cout << "[";
    for (int i = 0; i < n; i++)
    {
        cout << a[i];
        if (i != n - 1)
            cout << ",";
    }
    cout << "]";
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