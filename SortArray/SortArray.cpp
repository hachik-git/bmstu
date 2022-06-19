#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <locale.h>
#include <stdlib.h>
#include <conio.h>
#include <cstdio>
#include <iostream>
#include <cstdio>

int** dynamic_array_alloc(int row_count, int col_count)
{
    int** a = (int**)malloc(row_count * sizeof(int*));    

    for (int i = 0; i < row_count;)
        a[i++] = (int*)malloc(col_count * sizeof(int));

    return a;
}

void dynamic_array_free(int** a, int row_count)
{
    for (int i = 0; i < row_count; i++)
        free(a[i]);
    
    free(a);
}

void iswap(int* a, int* b)
{
    int tmp = *a;    
    *a = *b;
    *b = tmp;
}

void isort(int* a, int qnt, int desc = 0)
{
    for (int i = 1; i < qnt; i++)
        for (int j = i; j > 0; j--)
            if ((!desc && a[j] < a[j - 1]) || (desc && a[j] > a[j - 1]))
                iswap(&a[j], &a[j - 1]);
}

void bubbleSort(int* l, int* r) {
    int sz = r - l;
    if (sz <= 1) return;
    bool b = true;
    while (b) {
        b = false;
        for (int* i = l; i + 1 < r; i++) {
            if (*i > *(i + 1)) {
                std::swap(*i, *(i + 1));
                b = true;
            }
        }
        r--;
    }
}

void shakerSort(int* l, int* r) {    
    int sz = r - l;
    if (sz <= 1) return;
    bool b = true;
    int* beg = l - 1;
    int* end = r - 1;
    while (b) {
        b = false;
        beg++;
        for (int* i = beg; i < end; i++) {
            if (*i > *(i + 1)) {
                std::swap(*i, *(i + 1));
                b = true;
            }
        }
        if (!b) break;
        end--;
        for (int* i = end; i > beg; i--) {
            if (*i < *(i - 1)) {
                std::swap(*i, *(i - 1));
                b = true;
            }
        }
    }
}

int main(void)
{
    setlocale(LC_ALL, "Russian");
    int ia[10];
    int al = 10;

    srand(42);//srand(time(NULL));
    for (int i = 0; i < al; i++)
        ia[i] = rand();

    for (int i = 0; i < al; i++)
        printf("%d - %d\n", i, ia[i]);
    
    printf("\n");

    bubbleSort(&ia[0], &ia[10]);
    for (int i = 0; i < al; i++)
        printf("%d - %d\n", i, ia[i]);

    shakerSort(&ia[0], &ia[10]);
    for (int i = 0; i < al; i++)
        printf("%d - %d\n", i, ia[i]);

    return 0;

    /*
    int rc, cc; // Row_Count, Col_Count or an array
    
    printf("Enter row count:\n");
    scanf_s("%d", &rc);
    printf("Enter col count:\n");
    scanf_s("%d", &cc);

    int** a = dynamic_array_alloc(rc, cc);
    
    printf("Enter array elements:\n");
    for (int r = 0; r < rc; r++)
        for (int c = 0; c < cc; c++)
        {
            printf("[%d,%d]: ", r+1, c+1);
            scanf_s("%d", &a[r][c]);
        }

    printf("Input array:\n");
    for (int r = 0; r < rc; r++)
    {
        for (int c = 0; c < cc; c++)
            printf(" %d", a[r][c]);

        printf("\n");
    }

    for (int i = 0; i < rc; i++)
        isort(a[i], cc, i%2 == 1);

    printf("Sorted array:");
    for (int r = 0; r < rc; r++)
    {
        printf(r % 2 == 0 ? "\n %d (asc): " : "\n %d (desc): ", r+1);
        for (int c = 0; c < cc; c++)
            printf(" %d", a[r][c]);
    }

    dynamic_array_free(a, rc);*/
}