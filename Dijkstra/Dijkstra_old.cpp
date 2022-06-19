#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <stdio.h>
#include <locale.h>
#include <stdlib.h>
#include <conio.h>
#include <cstdio>
#include <time.h>

typedef char POINT[100];

struct POINTS {
	POINT* points;
	int qnt;
};

struct ROUTE {
	POINT start;
	POINT finish;
	int price;
};

struct PRICE_LIST {
	ROUTE* routes;
	int qnt;
};

struct DESTINATION {
	POINT name;
	int cost;
	int visited;
	POINTS transfers;
};

struct DESTINATIONS {
	DESTINATION* destinations;
	int qnt;
};

void TrimNewLine(char* s)
{
	while (*s)
		if (10 == *s++)
		{
			*(s - 1) = 0;
			return;
		}
}

void generate_prices()
{
	setlocale(LC_ALL, "Russian");
	char city[100];
	char** cities = (char**)malloc(100 * sizeof(char*));
	int q = 0;

	char FileName[1200] = "cities.txt";
	FILE* f = fopen(FileName, "r");
	char* NewCity;
	while (fgets(city, 100, f))
	{
		TrimNewLine(city);
		if ((cities[q] = (char*)malloc(100 * sizeof(char))) == NULL)
			return;

		strcpy(cities[q], city);
		q++;
	}
	fclose(f);

	f = fopen("prices.txt", "w");
	srand(time(NULL));
	for (int i = 0; i < q; i++)
		for (int j = 0; j < q; j++)
			if (j != i)
				fprintf(f, "%s;%s;%d\n", cities[i], cities[j], ((int)(rand() / 100)) * 100);

	for (int i = 0; i < q; i++)
		free(cities[i]);

	free(cities);
	fclose(f);
}

PRICE_LIST InitPrices(const char* price_file_mame)
{
	FILE* f = fopen(price_file_mame, "r");
	ROUTE* routes = (ROUTE*)malloc(sizeof(ROUTE));
	ROUTE route;
	int q = 0;
	while (!feof(f))
	{
		fscanf(f, "%[^;];%[^;];%d\n", &(route.start), &(route.finish), &(route.price));

		routes = (ROUTE*)realloc(routes, ++(q) * sizeof(ROUTE));
		routes[q - 1] = route;
	}
	fclose(f);

	PRICE_LIST price_list;
	price_list.routes = routes;
	price_list.qnt = q;

	return price_list;
}

int IndexOf(char** a, char* str, int len)
{
	for (int i = 0; i < len; i++)
		if (strcmp(a[i], str) == 0)
			return i;

	return -1;
}

int IndexOfPoint(POINTS points, POINT point)
{
	for (int i = 0; i < points.qnt; i++)
		if (strcmp(points.points[i], point) == 0)
			return i;

	return -1;
}

int GetDestinationByPoint(DESTINATIONS destinations, POINT point)
{
	for (int i = 0; i < destinations.qnt; i++)
		if (strcmp(destinations.destinations[i].name, point) == 0)
			return i;
	return -1;
}

DESTINATIONS InitDestinations(PRICE_LIST price_list)
{
	char** cities = (char**)malloc(sizeof(char*));

	int qnt = 0;
	for (int i = 0; i < price_list.qnt; i++)
	{
		if (IndexOf(cities, price_list.routes[i].start, qnt) == -1)
		{
			cities = (char**)realloc(cities, ++(qnt) * sizeof(char*));
			cities[qnt - 1] = (char*)malloc(strlen(price_list.routes[i].start) * sizeof(char));
			strcpy(cities[qnt - 1], price_list.routes[i].start);
		}
		if (IndexOf(cities, price_list.routes[i].finish, qnt) == -1)
		{
			cities = (char**)realloc(cities, ++(qnt) * sizeof(char*));
			cities[qnt - 1] = (char*)malloc(strlen(price_list.routes[i].finish) * sizeof(char));
			strcpy(cities[qnt - 1], price_list.routes[i].finish);
		}
	}

	DESTINATIONS d;
	d.destinations = (DESTINATION*)malloc(qnt * sizeof(DESTINATION));
	for (int i = 0; i < qnt; i++)
	{
		strcpy(d.destinations[i].name, cities[i]);
		d.destinations[i].cost = -1;
		d.destinations[i].visited = 0;
		d.destinations[i].transfers.qnt = 0;
		d.destinations[i].transfers.points = (POINT*)malloc(sizeof(POINT));
	}
	d.qnt = qnt;

	free(cities);
	return d;
}

int GetNearestUnvisitedDest(DESTINATIONS* destinations)
{
	int min_cost = -1;
	int idx = -1;
	for (int i = 0; i < destinations->qnt; i++)
		if (destinations->destinations[i].visited == 0)
			if ((min_cost == -1 && destinations->destinations[i].cost != -1) || ((destinations->destinations[i].cost != -1) && destinations->destinations[i].cost < min_cost))
			{
				min_cost = destinations->destinations[i].cost;
				idx = i;
			}
	return idx;
}

void UpdateDestinations(DESTINATIONS* destinations, PRICE_LIST price_list, DESTINATION* start_point)
{
	int neibors_count = 0;
	DESTINATIONS neibors;
	neibors.destinations = (DESTINATION*)malloc(sizeof(DESTINATION));
	neibors.qnt = 0;

	for (int i = 0; i < price_list.qnt; i++)
		if (strcmp(price_list.routes[i].start, start_point->name) == 0)
		{
			int idx = GetDestinationByPoint(*destinations, price_list.routes[i].finish);
			if (destinations->destinations[idx].visited == 0)
			{
				neibors.destinations = (DESTINATION*)realloc(neibors.destinations, ++(neibors.qnt) * sizeof(DESTINATION));
				strcpy(neibors.destinations[neibors.qnt - 1].name, price_list.routes[i].finish);
				neibors.destinations[neibors.qnt - 1].cost = price_list.routes[i].price;
			}
		}

	for (int i = 0; i < neibors.qnt; i++)
	{
		int idx = GetDestinationByPoint(*destinations, neibors.destinations[i].name);
		int new_cost;
		if (destinations->destinations[idx].cost == -1)
			destinations->destinations[idx].cost = start_point->cost + neibors.destinations[i].cost;
		else
		{
			if ((new_cost = start_point->cost + neibors.destinations[i].cost) < destinations->destinations[idx].cost)
				destinations->destinations[idx].cost = new_cost;
		}
		destinations->destinations[idx].transfers.qnt = start_point->transfers.qnt+1;
		destinations->destinations[idx].transfers.points = (POINT*)realloc(destinations->destinations[idx].transfers.points, (start_point->transfers.qnt + 1) * sizeof(POINT));
		for (int j = 0; j < start_point->transfers.qnt; j++)
			strcpy(destinations->destinations[idx].transfers.points[j], start_point->transfers.points[j]);
		strcpy(destinations->destinations[idx].transfers.points[start_point->transfers.qnt], destinations->destinations[idx].name);
	}
	start_point->visited = 1;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	//generate_prices(); // генерирует прайсы случайным образом

	PRICE_LIST price_list = InitPrices("prices.txt");
	//do {
	DESTINATIONS destinations = InitDestinations(price_list);

	printf("Откуда летим? Возможные пункты отправления (%d): \n\n", destinations.qnt);
	for (int i = 0; i < destinations.qnt; i++)
		printf("   %d: %s\n", i + 1, destinations.destinations[i].name);

	int start = 0;

	printf("\nВведите номер пункта отправленя: ");
	start = _getche();
	while (start < 49 || start > 58 || start - 48 > destinations.qnt)
	{
		printf("\nНеверный номер. Повторите ввод: ");
		start = _getche();
	}
	start = start - 49;

	destinations.destinations[start].cost = 0;

	int idx = GetNearestUnvisitedDest(&destinations);
	int trans_qnt = 0;
	while (idx != -1)
	{
		UpdateDestinations(&destinations, price_list, &destinations.destinations[idx]);
		idx = GetNearestUnvisitedDest(&destinations);
		trans_qnt++;
	}

	int c = 0;
	for (int i = 0; i < destinations.qnt; i++)
		if ((i != start) && (destinations.destinations[i].cost != -1))
		{
			if (++c == 1)
				printf("\n\nИз пункта \"%s\" Вы можете долететь до следующих пунктов\n(показана минимальная стоимость перелета):\n\n", destinations.destinations[start].name);

			printf("   %s: Стоимость - %d руб.", destinations.destinations[i].name, destinations.destinations[i].cost);
			if (destinations.destinations[i].transfers.qnt > 1)
			{
				printf(" Пересадки (%d): ", destinations.destinations[i].transfers.qnt - 1);
				for (int j = 0; j < destinations.destinations[i].transfers.qnt - 1; j++)
				{
					printf(destinations.destinations[i].transfers.points[j]);
					if (j < destinations.destinations[i].transfers.qnt - 2)
						printf(", ");
				}
			}
			else
				printf(" Прямой");

			printf("\n");
		}

	if (c == 0)
		printf("\n\nК сожалению, маршруты из указанной точки не найдены\n");

	printf("\nРассчитано на основе имеющегося прайса:\n\n");
	for (int i = 0; i < price_list.qnt; i++)
		printf(" %s - %s: %d руб.\n", price_list.routes[i].start, price_list.routes[i].finish, price_list.routes[i].price);
	printf("\nНaжмите любую клавишу для выхода");
	_getche();

	free(price_list.routes);
	free(destinations.destinations);
	
	//printf("Нажмите \"y\" для продожения или любую клавишу дя выхода) ?");
	//} while (_getche() == 'y');
}