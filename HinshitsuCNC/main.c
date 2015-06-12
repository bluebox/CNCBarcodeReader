#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>

int search_path_count = 0;
char search_paths[32][256];

int read_search_path();

int main(int argc, char **argv) 
{
	char buffer[4096];

	printf("***************************************\n");
	printf("HINSHISTU Manufacturing Private Limited\n");
	printf("***************************************\n");

	printf("\n");
	printf("\n");

	read_search_path();

	while(1) 
	{
		printf("Scan or type code: ");
		
		memset(buffer, 0, 4096);
		scanf("%4095s", buffer);
		buffer[4095] = '\0';
		printf("\n");
		printf("\n");

		printf("%s\n", buffer);

		if (strcmp("exit", buffer) == 0) 
		{
			return 0;
		}

		system(buffer);
	}
}

int file_exists(const char* file_path) 
{
	struct stat buffer;   
	int return_value;

	return_value = stat(file_path, &buffer);

	return return_value == 0 && buffer.st_size > 0;
}

int read_search_path() 
{
	char* search_file_name = "searchpath.txt";
	FILE* fp;

    fp = fopen(search_file_name, "r");

	if (fp == NULL)
	{
		return -1;
	}

	for (search_path_count = 0; search_path_count < 32; search_path_count++) 
	{
		if (0 == fgets(search_paths[search_path_count], 256, fp)) 
		{
			break;
		}

		if (search_paths[search_path_count][0] == '#') 
		{
			search_path_count --;
			continue;
		}
    }

    fclose(fp);

	return search_path_count;
}