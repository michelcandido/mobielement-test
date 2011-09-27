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

char* sort(char *src)
{
	int len = strlen(src);
	char *target = (char*)malloc((len+1)*sizeof(char));
	int count[256] = {0};
	while (*src) {
		count[*src]++;
		src++;
	}
	for (int i = 0; i < 256; i++)
	{
		if (count[i])
		{
			memset(target, i, count[i]);			
			target+=count[i];
		}
	}
	*target = '\0';
	target -= len;
	return target;
}

bool Q4(char *s1, char *s2)
{
	return strncmp(sort(s1),sort(s2), 256)==0;
}

int _tmain(int argc, _TCHAR* argv[])
{
	char s1[] = "aBcDgef";
	char s2[] = "gefaBcD";
	printf("%s and %s is anagrams: %d\n", s1, s2, Q4(s1, s2));

	/*
	char src[] = "abccde";
	printf("remove duplicates in %s is: ", src);
	Q3(src);
	printf("%s\n",src);
	*/
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

