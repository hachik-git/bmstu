//#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <locale.h>
#include <stdlib.h>

void iswap(int* a, int* b)
{
    int tmp = *a;
    *a = *b;
    *b = tmp;
}

void csort(int* a, int qnt, int desc = 0)
{
    for (int i = 1; i < qnt; i++)
        for (int j = i; j > 0; j--)
            if ((!desc && a[j] < a[j - 1]) || (desc && a[j] > a[j - 1]))
                iswap(&a[j], &a[j - 1]);
}

int main(void)
{
    setlocale(LC_ALL, "Russian");

    int sc; // String_Count
    
    printf("Enter string count:\n");
    scanf_s("%d", &sc);

    char** a = (char**)malloc(sc * sizeof(char*));
    
    printf("Enter strings:\n");
    for (int i = 0; i < sc; i++)
        {
            printf("%d: ", i+1);
            //scanf_s("%d", &a[r][c]);
        }

    for (int i = 0; i < sc; i++)
        csort(a[i], sc, i%2 == 1);

    printf("Sorted strings:");
    for (int r = 0; r < sc; r++)
    {
        printf(r % 2 == 0 ? "\n %d (asc): " : "\n %d (desc): ", r+1);        
            printf(" %d", a[r]);
    }

    //dynamic_array_free(a, rc);
}