#define _CRT_SECURE_NO_WARNINGS

#include <iostream>


struct persons_data {
	char last_name[100];
	char first_name[100];
	char middle_name[100];
	char birthday[10];
	struct {
		int serie;
		int number;
	} passport;
};

void parse_fio(char * fio, char* last_name, char* middle_name, char* first_name)
{
	int i = 0, j = 0;
	while (*(fio++))
	{
		if (*(fio-1) == ' ')
		{
			i++;
			j = 0;
		}

		switch (i)
		{
			case 0: last_name[j++] = *(fio - 1); break;
			case 1: middle_name[j++] = *(fio - 1); break;
			case 2: first_name[j++] = *(fio - 1); break;
		}
	}
}

void parse_text()
{
	FILE* rf = fopen("data.txt", "r"), * wf = fopen("formated.txt", "w");
	persons_data data;
	
	char  c;
	char delim = ',';
	char tmp[100];
	char fio[100];
	char bd[100];
	char pass[100];
	int str_qnt = 0, char_qnt = 0, words_qnt = 0;	

	while ((c = getc(rf)) != EOF)
	{
		if (c != delim)
			tmp[(char_qnt++)] = c;
		else
		{
			tmp[char_qnt] = 0;
			switch (++words_qnt)
			{
				case 1:
					strcpy(fio, tmp);
					parse_fio(fio, data.last_name, data.middle_name, data.first_name);
					break;
				case 2:
					strcpy(bd, tmp);
					break;
				case 3:
					strcpy(pass, tmp);
					words_qnt = 0;
					str_qnt++;

			};
			char_qnt = 0;
			memset(tmp, 0, sizeof(tmp));
		}
	}

	fprintf(wf, "%s %s %s, %s, %d %d\n"
		, &(data.last_name)
		, &(data.first_name)
		, &(data.middle_name)
		, &(data.birthday)
		, data.passport.serie
		, data.passport.number);
	printf("%-10s | %-10s | %-10s | %s | %d | %d |\n"
		, &(data.last_name)
		, &(data.first_name)
		, &(data.middle_name)
		, &(data.birthday)
		, data.passport.serie
		, data.passport.number);

	fclose(rf);
	fclose(wf);
}

int main()
{
	int a[4][3] = {
		{1,2,3},
		{10,20,30},
		{100,200,300},
		{1000,2000,3000},
	};
	
	char* b[3] = {
		(char*)"asgf\0",
		(char*)"hwrtytrwhjhghgkhjghjkg\0",
		(char*)"etrywty\0" };
	//b = a[0];
	printf("%d\n", *(a[1] + 2));
	printf("%d\n", *(a[1]));
	printf("%s\n", b[0]);
	printf("%s\n", b[1]);
	printf("%s\n", b[2]);
	//printf("%d\n", *(b + 2*3 + 1));
	return 0;
	setlocale(LC_ALL, "Russian");
	parse_text();
	return 0;
	FILE* rf = fopen("data.txt", "r"), * wf = fopen("formated.txt", "w");
	persons_data data;

	while (((!feof(rf))
		&& (fscanf(rf, "%s %s %[^,],%[^,],%d %d,"
			, &(data.last_name)
			, &(data.first_name)
			, &(data.middle_name)
			, &(data.birthday)
			, &(data.passport.serie)
			, &(data.passport.number)
		) > 0)
		&& (fprintf(wf, "%s %s %s, %s, %d %d\n"
			, &(data.last_name)
			, &(data.first_name)
			, &(data.middle_name)
			, &(data.birthday)
			, data.passport.serie
			, data.passport.number) > 0)
		&& (printf("%-10s | %-10s | %-10s | %s | %d | %d |\n"
			, &(data.last_name)
			, &(data.first_name)
			, &(data.middle_name)
			, &(data.birthday)
			, data.passport.serie
			, data.passport.number) > 0)) || fclose(rf) || fclose(wf));

}
