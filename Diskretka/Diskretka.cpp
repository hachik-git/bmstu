#include <stdio.h>
#include <math.h>
#include <stdlib.h>

int iscan(char * hello)
{
    printf("%s", hello);
    char c = getchar();
    while (c < '0' || c > '9')
        c = getchar();
    return c - 48;
}

int GetOrd(int b, int p)
{
    printf("Calc group order:\n");
    printf("Power = %d: mod = %d\n", 0, 1);
    int ord = 1;
    int i = b;
    printf("Power = %d: mod = %d\n", ord, i);
    while ((i = ((i * b) % p)) != 1)
    {
        printf("Power = %d: mod = %d\n", ord+1, i);
        ord++;
    }
    printf("Power = %d: mod = %d\n", ++ord, 1);
    return ord;
}

int* GetGroup(int b, int p, int *ord)
{   
    *ord = 1;
    int i = b;
    int* a = (int*)(malloc(sizeof(int)));
    a[0] = 0;
    a[1] = b;
    while ((i = ((i * b) % p)) != 1)
    {
        if (i < 0)
            i = p + i;
        if (i > (p / 2))
            i = i - p;
        //printf("Power = %d: mod = %d\n", ((*ord)++)+1, i);
        (*ord)++;
        int* ip = (int*)realloc(a, sizeof(int) * ((*ord)+1));
        if (ip == NULL)
        {
            free(a);
            return NULL;
        }
            
        a = ip;
        a[(*ord) - 1] = i;
        if (i == 1)
            break;
    }    
    return a;
}

int main()
{
    int b, p;
    printf("%s", "Enter b: ");
    scanf_s("%d", &b);
    printf("%s", "Enter p: ");
    scanf_s("%d", &p);

    int GroupOrder;
    printf("\nCalc group order:\n");
    //printf("\nPower = %d: mod = %d\n", 0, 1);
    int* a = GetGroup(b, p, &GroupOrder);
    for (int i = 0; i<GroupOrder;)
        printf("Power = %d: mod = %d\n", i, a[i++]);
    
    printf("Group order = %d", GroupOrder);
}