// RecursionC.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <string>
#include <iostream>
using namespace std;
int factorial(int n)
{
	if (n == 0 || n == 1) return 1;
	else return n * factorial (n - 1);
}

char** getPerms(char src[])
{
	if (!src) return 0;
	int len = strlen(src);
	int size = factorial(len);
	
	char **perms = new char*[size];
	
	if (len == 0) 
	{	
		char *string = "";
		perms[0] = string;
		return perms;
	}

	char first = src[0];
	char *rest = &src[1];

	char **words = getPerms(rest);
	int wordsCount = factorial(len - 1);
	for (int i = 0; i < wordsCount; i++)
	{
		for (unsigned int j = 0; j <= strlen(words[i]); j++)
		{
			char *string = new char[len + 1]; 
			if (j == 0) {
				string[0] = first;
				strncpy(string+1,words[i], len-1);
				string[len] = '\0';
			} else {
				strncpy(string, words[i],j);
				string[j] = first;
				strncpy(string+j+1, words[i]+j+1, len-j);
				string[len] = '\0';
			}
			perms[j] = string;
		}
	}
	return perms;
}

int _tmain(int argc, _TCHAR* argv[])
{
	char src[] = "abc";
	char** result = getPerms(src);
	for (int i = 0; i < factorial(strlen(src)); i++ ){
		cout << result[i] << endl;
	}
	return 0;
}

