#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <string>
#include <string_view>
#include <cstring>


struct IP {

	unsigned byte1;
	unsigned byte2;
	unsigned byte3;
	unsigned byte4;

	void print()
	{
		printf("%d.%d.%d.%d", byte1, byte2, byte3, byte4);
	}
};

struct node {
	IP ip;
	node* next = NULL;
	node* prev = NULL;
	void print()
	{
		printf("%d.%d.%d.%d", ip.byte1, ip.byte2, ip.byte3, ip.byte4);
	}
};

class listners {
private:
	int _count = 0;

public:

	node* first;

	int count()
	{
		return _count;
	}

	bool isEmpty()
	{
		return _count == 0;
	}

	node* last()
	{
		node* cur = first, * l;

		while ((cur != NULL) && (l = cur->next))
			cur = l;

		return cur;
	}

	node* push(IP ip)
	{
		node* l = this->last();
		node* newNode = (node*)malloc(sizeof(node));

		if (newNode == NULL)
			return NULL;

		newNode->ip = ip;
		newNode->next = NULL;
		newNode->prev = l;

		if (l == NULL)
			first = newNode;
		else
			l->next = newNode;

		_count++;

		return newNode;
	}

	node* pop(bool log = 0)
	{
		if (isEmpty())
			return NULL;

		node* l = last();
		l->prev->next = NULL;
		_count--;

		if (log)
		{
			printf("Deleted IP ");
			l->print();
			printf("\n");
		}

		return l;
	}

	void remove(node* n, bool log = 0)
	{
		if (isEmpty())
			return;

		node* cur = first;
		while (cur && (cur != n))
			cur = cur->next;

		if (cur == NULL)
			return;

		cur->prev->next = cur->next;
		cur->next->prev = cur->prev;
		_count--;

		if (log)
		{
			printf("Deleted IP ");
			cur->print();
			printf("\n");
		}
	}

	void remove(int index, bool log = 0)
	{
		if (isEmpty())
			return;

		node* cur = first;
		for (int i = 0; i < index; i++)
			cur = cur->next;

		cur->prev->next = cur->next;
		cur->next->prev = cur->prev;
		_count--;

		if (log)
		{
			printf("Deleted IP ");
			cur->print();
			printf("\n");
		}
	}
};

int main()
{
	setlocale(LC_ALL, "Russian");
	listners l;

	IP a;

	for (int i = 1; i <= 10; i++)
	{
		a.byte1 = 127;
		a.byte2 = 0;
		a.byte3 = 0;
		a.byte4 = i;
		l.push(a);
	}

	node* cur = l.first;

	printf("Вывод списка IP\n");
	while (cur)
	{
		cur->print();
		printf("\n");
		cur = cur->next;
	}

	printf("Удаление последних 5 элементов\n");
	for (int i = 1; i <= 5; i++)
		cur = l.pop(1);

	cur = l.first;
	for (int i = 0; i < 2; i++)
		cur = cur->next;

	printf("Удаление 3-го с начала элемента\n"); 
	l.remove(cur, 1);

	printf("Удаление 2-го с начала элемента\n");
	l.remove(1, true);

	cur = l.first;

	printf("Вывод списка IP\n");
	while (cur)
	{
		cur->print();
		printf("\n");
		cur = cur->next;
	}

	return 0;
}


