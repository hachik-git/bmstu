#include <stdio.h>
#include <stdlib.h>

struct char_info {
	char Char;
	int Count = 0;
};


//получает строку произвольной длины до нажатия Enter; len - выходной параметр - длина полученной строки
char* get_string(int* len) {
	*len = 0;
	int capacity = 1;

	char* s = (char*)malloc(sizeof(char));

	char c;
	while ((c = getchar()) != '\n')
	{
		s[(*len)++] = c;

		if (*len >= capacity)
		{
			capacity *= 2;
			char* sp = (char*)realloc(s, capacity * sizeof(char));
			if (sp == NULL)
				return NULL;
			s = sp;
		}
	}

	s[*len] = '\0';

	return s;
}

char* get_string()
{
	int len;
	return get_string(&len);
}


//возвращает индекс символа в массиве; все параметры входные, qnt - длина переданного массива
int GetCharIndex(char c, char_info* counter, int qnt)
{
	for (int i = 0; i < qnt; i++)
		if (counter[i].Char == c)
			return i;

	return -1;
}


//возвращает ссылку на массив char_info; len - выходной параметр - длина полученного массива
char_info* GetCounter(char* s, int* len) {
	*len = 0;
	int capacity = 1;
	char_info* counter = (char_info*)malloc(sizeof(char_info));

	char c;
	int i;
	while (c = *s++)
	{
		if ((i = GetCharIndex(c, counter, *len)) >= 0)
			counter[i].Count++;
		else
		{
			counter[(*len)] = { c, 1 };

			if (++(*len) >= capacity) {
				capacity *= 2;
				counter = (char_info*)realloc(counter, capacity * sizeof(char_info));
			}
		}
	}

	return counter;
}

//обмен местами элементов массива
void SwapIndexes(char_info* counter, int index1, int index2)
{
	char_info tmp = counter[index1];
	counter[index1] = counter[index2];
	counter[index2] = tmp;
}


//сортировка массива по числу символов
void SortCounter(char_info* counter, int qnt)
{
	for (int i = 1; i <= qnt - 1; i++)
	{
		int changed = 0;
		for (int j = 0; j <= qnt - 1 - i; j++)
		{
			if (counter[j].Count < counter[j + 1].Count)
			{
				SwapIndexes(counter, j, j + 1);
				changed++;
			}
		}
		if (!changed)
			break;
	}
}

void SwapElements2(int* i1, int* i2)
{
	int tmp = *i1;
	*i1 = *i2;
	*i2 = tmp;
}

void SwapElements(int* a1, int* a2, int index)
{
	int tmp = a1[index];
	a1[index] = a2[index];
	a2[index] = tmp;
}

void SwapArrays(int* a1, int* a2, int qnt)
{
	for (int i = 0; i < qnt; i++)
		SwapElements2(&a1[i], &a2[i]);
}

int main()
{
	printf("Enter array size\n");
	int qnt;
	scanf_s("%d", &qnt);
	const int q2 = qnt;
	int* a = (int*)malloc(qnt*sizeof(int));
	int* b = (int*)malloc(qnt * sizeof(int));

	int i = 5;
	int c[i];


	for (int i = 0; i < qnt; i++)
	{
		printf("Enter element #%d of array 1:\n", i);
		scanf_s("%d", &a[i], 5);
		printf("Enter element #%d of array 2:\n", i);
		scanf_s("%d", &b[i], 5);
	}

	SwapArrays(a, b, qnt);

	printf("Array 1: ");
	for (int i = 0; i < qnt; i++)
		printf("%d, ", a[i]);
	printf("\nArray 2:");
	for (int i = 0; i < qnt; i++)
		printf("%d, ", b[i]);

	return 0;



	
	int* c;
	c = (int*)malloc(sizeof(char));
	printf("size of c = %d\n", sizeof(c));
	printf("size of *c = %d\n", sizeof(*c));
	return 0;
	printf("Enter string (256 symbol maximum) and press ENTER:\n");
	//ввод строки фиксированной длины: char s[256]; if (!fgets(s, 256, stdin)) printf("ќшибка ввода строки");
	char* s = get_string();

	int CharsCount;
	char_info* counter = GetCounter(s, &CharsCount);

	free(s);

	SortCounter(counter, CharsCount);

	printf("\nYour string consists of the following characters:\n");
	for (int i = 0; i < CharsCount; i++)
		printf("%c - %d\n", counter[i].Char, counter[i].Count);

	free(counter);
}