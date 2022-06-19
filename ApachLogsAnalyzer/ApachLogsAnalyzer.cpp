#define _CRT_SECURE_NO_WARNINGS

#include <iostream>
#include <stdio.h>
#include <conio.h>
#include <ctype.h>

typedef char url[2048];
typedef char ip[16];

union responce_code
{
	char type[1];
	char code[4];
};

enum responce_code_type {
	rct_info = '1',
	rct_success = '2',
	rct_redirect = '3',
	rct_clientError = '4',
	rct_serverError = '5'
};

struct access_log_rec {
	ip client_ip; // IP - адрес клиента
	char ident[100]; // «идентификатор» на клиенте, который используется для идентификации
	char user[100]; // Идентификатор пользователя клиента, если использовалась HTTP - аутентификация    
	struct tm request_time; // Отметка времени записи в журнале.	
	struct {
		char method[5];
		url address;
		char protocol[20];
	} request;

	responce_code responce_code; // Код состояния, который был возвращен клиенту

	unsigned file_size; // Размер файла(включая заголовки) в байтах, который был запрошен.
	url referer; // откуда пришли
	url user_agent; // Содержит информацию о подключающемся веб - браузере клиента и операционной системе.
};

int month_name_to_num(char mon_name[3])
{
	if (!strcmp(mon_name, "Jan")) return 1;
	else if (!strcmp(mon_name, "Feb")) return 2;
	else if (!strcmp(mon_name, "Mar")) return 3;
	else if (!strcmp(mon_name, "Apr")) return 4;
	else if (!strcmp(mon_name, "May")) return 5;
	else if (!strcmp(mon_name, "Jun")) return 6;
	else if (!strcmp(mon_name, "Jul")) return 7;
	else if (!strcmp(mon_name, "Aug")) return 8;
	else if (!strcmp(mon_name, "Sep")) return 9;
	else if (!strcmp(mon_name, "Oct")) return 10;
	else if (!strcmp(mon_name, "Nov")) return 11;
	else if (!strcmp(mon_name, "Dec")) return 12;
	else return -1;
}

access_log_rec* get_errors(access_log_rec* logs, int qnt_logs, char err_type, int* qnt)
{
	access_log_rec log;
	int cap = 1;
	*qnt = 0;
	access_log_rec* result = (access_log_rec*)malloc(sizeof(access_log_rec));
	for (int i = 0; i < qnt_logs; i++)
	{
		log = logs[i];
		if ((*(log.responce_code.type) == err_type)
			|| ((err_type == '0') && ((*(log.responce_code.type) == (char)rct_clientError) || (*(log.responce_code.type) == (char)rct_serverError))))
		{
			if (cap < (++(*qnt)))
			{
				cap *= 2;
				result = (access_log_rec*)realloc(result, cap * sizeof(access_log_rec));
			}
			memcpy(&(result[(*qnt) - 1]), &log, sizeof(access_log_rec));
		}
	}
	if (*qnt == 0)
	{
		free(result);
		return NULL;
	}
	return result;
}

access_log_rec* read_logs(int* qnt)
{
	*qnt = 0;
	access_log_rec* logs = (access_log_rec*)malloc(sizeof(access_log_rec));
	char log_file_name[255] = "logs/access.2021.04.01.log"; 
	FILE* f;
	fopen_s(&f, log_file_name, "r");
	if (f == NULL)
	{
		printf("Невозможно открыть файл %s", log_file_name);
		return NULL;
	}

	access_log_rec log;
	int cap = 1; // максимальный размер массива логов

	char request_time[27], mon[4], request[2048];
	int tz;
	while (!(feof(f)))
	{
		fscanf(f, "%s %s %s [%[^]]] \"%[^\"]\" %s %d \"%[^\"]\" \"%[^\"]\"\n"
			, &(log.client_ip)
			, &(log.ident)
			, &(log.user)
			, &request_time
			, &request
			, &(log.responce_code)
			, &(log.file_size)
			, &(log.referer)
			, &(log.user_agent));

		if (cap < (++(*qnt)))
		{
			cap *= 2;
			logs = (access_log_rec*)realloc(logs, cap * sizeof(access_log_rec));
		}

		sscanf(request_time, "%d/%3s/%d:%d:%d:%d %d"
			, &(log.request_time.tm_mday)
			, &mon
			, &(log.request_time.tm_year)
			, &(log.request_time.tm_hour)
			, &(log.request_time.tm_min)
			, &(log.request_time.tm_sec)
			, &tz
		);
		sscanf(request, "%s %s %s"
			, &(log.request.method)
			, &(log.request.address)
			, &(log.request.protocol)
		);
		log.request_time.tm_mon = month_name_to_num(mon);
		logs[*qnt - 1] = log;
	}
	fclose(f);
	if (*qnt == 0)
	{
		free(logs);
		return NULL;
	}
	return logs;
}

void print_errors(access_log_rec* errors, int err_qnt)
{
	if (0 == err_qnt)
		printf("\nОшибок не найдено\n\n");
	else
	{
		printf("\n------------------------------------------------------------------------------------------");
		printf("\n| Код | Дата                | IP              | Адрес на сайте");
		printf("\n------------------------------------------------------------------------------------------");
		for (int i = 0; i < err_qnt; i++)
			printf("\n| %s | %02d.%02d.%04d %02d:%02d:%02d | %015s | %s "
				, errors[i].responce_code.code
				, errors[i].request_time.tm_mday
				, errors[i].request_time.tm_mon
				, errors[i].request_time.tm_year
				, errors[i].request_time.tm_hour
				, errors[i].request_time.tm_min
				, errors[i].request_time.tm_sec
				, errors[i].client_ip
				, errors[i].request.address);
		printf("\n------------------------------------------------------------------------------------------\n\n");
	}
}

int lcmp(const void* pa, const void* pb) {
	//return *(int*)a - *(int*)b;
	//else if (typeid(a) == typeid(access_log_rec*))	
	access_log_rec a = *(access_log_rec*)pa, b = *(access_log_rec*)pb;
	int j = strcmp(a.responce_code.code, b.responce_code.code);
	if (j != 0)
		return (int)(a.responce_code.code) - (int)(b.responce_code.code);
	else
		return strcmp(b.request.address, a.request.address);
}

int main()
{
	setlocale(LC_ALL, "Russian");

	int l_qnt = 0;
	access_log_rec* logs = read_logs(&l_qnt);

	int c = 0;
	do
	{
		printf("Выберите отчет (1 - Ошибки 4хх;  2 - Ошибки 5хх;  3 - Все ошибки; Другая клавиша - выход): ");
		c = _getche() - 48;
		if (c >= 1 and c <= 3)
		{
			int err_qnt;

			access_log_rec* errors = get_errors(logs, l_qnt, (c == 1 ? '4' : (c == 2 ? '5' : '0')), &err_qnt);
			qsort(errors, err_qnt, sizeof(access_log_rec), lcmp);
			print_errors(errors, err_qnt);
			free(errors);
		}
	} while (c >= 1 and c <= 3);

	if (logs != NULL)
		free(logs);
}