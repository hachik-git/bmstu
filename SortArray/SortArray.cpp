#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <locale.h>
#include <stdlib.h>
#include <conio.h>
#include <cstdio>
#include <iostream>
#include <cstdio>

void iswap(int &a, int &b)
{
    int tmp = a;
    a = b;
    b = tmp;
}

void printArray(int* a, int n)
{
    for (int i = 0; i < n; i++)
        printf("  %d - %d\n", i, a[i]);

    printf("\n");
}

void initArray(int* a, int n)
{
    srand(42);//srand(time(NULL));

    for (int i = 0; i < n; i++)
        a[i] = rand();
}

void bubbleSort(int* a, int qnt)
{
    bool flag = 1;
    
    while (flag)
    {
        flag = false;
        for (int i = 0; i < qnt - 1; i++)
            if (a[i] > a[i + 1])
            {
                iswap(a[i], a[i + 1]);
                flag = true;
            }
    }
}

void completeSort(int* a, int qnt)
{
    for (int i = 0; i < qnt - 1; i++)
        for (int j = i; j < qnt; j++)
            if (a[j] < a[i])
                iswap(a[i], a[j]);
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

    initArray(&ia[0], 10);
    
    printf("Массив для сортировки:\n");
    printArray(&ia[0], al);

    bubbleSort(&ia[0], al);

    printf("Отсортирован пузырьком:\n");
    for (int i = 0; i < al; i++)
        printf("%d - %d\n", i, ia[i]);

    initArray(&ia[0], 10);
    completeSort(&ia[0], al);

    printf("Отсортирован перебором:\n");
    for (int i = 0; i < al; i++)
        printf("%d - %d\n", i, ia[i]);

    return 0;

    initArray(&ia[0], 10);

    shakerSort(&ia[0], &ia[10]);
    printf("Отсортирован шейкером:\n");
    for (int i = 0; i < al; i++)
        printf("%d - %d\n", i, ia[i]);

    return 0;
}