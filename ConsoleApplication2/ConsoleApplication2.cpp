#define _CRT_SECURE_NO_WARNINGS

#include <stdio.h>
#include <stdlib.h>
#include <iostream>

struct  person {
    char name[10];
    char lastname[10];
    int yearbirth;
    unsigned char old;
} ;

int main()
{
    setlocale(LC_ALL, "Russian");
    char str[] = "иван.федоров.2000.22";
    FILE* fp, * gp;
    fp = fopen("my_file.txt", "w");
    for (int i = 0; str[i] != '\0'; i++)
        putc(str[i], fp);
    fclose(fp);

    fp = fopen("my_file.txt", "r"), gp = fopen("my_file1.txt", "w");
    person p;

    while (!feof(fp)) fscanf(fp, "%[^.].%[^.].%d.%d", &p.name, &p.lastname, &p.yearbirth, &p.old),
        printf("%s, %s,%d,%d\n", &p.name, &p.lastname, p.yearbirth, p.old);

    fclose(fp);
    fclose(gp);
    return 0;
}
