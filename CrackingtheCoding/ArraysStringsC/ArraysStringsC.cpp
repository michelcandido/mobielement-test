// ArraysStringsC.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <string>

bool Q1(const char *src)
{
	int checker = 0;
	while (*src != '\0')
	{
		if (checker & 1 << *src)
			return true;
		checker = checker | 1 << *src;
		src++;
	}
	return false;
}

void Q2(char *src)
{
	char *end = src;
	char temp;
	if (src) 
	{
		while (*end)
			end++;
		end--;
		while (src < end) 
		{
			temp = *src;
			*src++ = *end;
			*end-- = temp;
		}
	}

}

void Q3(char *src)
{
	int checker = 0;
	while (*src)
	{
		if (checker & 1 << *src)
		{	
			int len = strlen(src);
			for (int i = 0; i < len; i++)
			{
				src[i] = src[i+1];
			}			
			continue;
		}
		checker = checker | 1 << *src;
		src++;
	}
}


int _tmain(int argc, _TCHAR* argv[])
{
	char src[] = "abccde";
	printf("remove duplicates in %s is: ", src);
	Q3(src);
	printf("%s\n",src);
	/*
	char src[] = "abcd";		
	printf("reverse %s is: ", src);
	Q2(src);
	printf("%s\n",src);
	*/
	/*
	char *src = "aBcd";
	printf("%s has duplicated chars: %d\n", src, Q1(src));
	*/
	return 0;
}

