#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <locale.h>
#include <stdlib.h>
#include <conio.h>
#include <cstdio>
#include <iostream>
#include <cstdio>

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

    printf("Массив для сортировки:\n");
    for (int i = 0; i < al; i++)
        printf("  %d - %d\n", i, ia[i]);
    
    printf("\n");

    bubbleSort(&ia[0], &ia[10]);
    printf("Отсортирован пузырьком:\n");
    for (int i = 0; i < al; i++)
        printf("%d - %d\n", i, ia[i]);

    srand(42);

    for (int i = 0; i < al; i++)
        ia[i] = rand();
    printf("\n"); 
    shakerSort(&ia[0], &ia[10]);
    printf("Отсортирован шейкером:\n");
    for (int i = 0; i < al; i++)
        printf("%d - %d\n", i, ia[i]);

    return 0;
}