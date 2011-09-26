// ArraysStringsC.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

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

int _tmain(int argc, _TCHAR* argv[])
{
	char *src = "aBcd";
	printf("%s has duplicated chars: %d\n", src, Q1(src));

	return 0;
}

