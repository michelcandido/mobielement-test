#include <iostream>

using namespace std;

class MyString {
public:
	char* strstr(char * str1, char * str2 );
	bool isUnique(char src[]);
	void reverse(char src[]);
	void compress(char src[]);
	bool isPalindrome(char s1[], char s2[]);
	bool isAnagrams(char s1[], char s2[]);
};

bool MyString::isAnagrams(char s1[], char s2[]) {
	if (!s1 || !s2) return false;
	if (strlen(s1) != strlen(s2)) return false;
	int letters[256] = {0};
	while (*s1) {
		letters[*s1]++;
		s1++;
	}
	while (*s2) {
		letters[*s2]--;
		if (letters[*s2] < 0) return false;
		s2++;
	}
	return true;
}

bool MyString::isPalindrome(char s1[], char s2[]) {
	if (!s1 || !s2) return false;
	if (strlen(s1) != strlen(s2)) return false;
	while (*s2) s2++;
	s2--;
	while (*s1) {
		if (*s1 != *s2) return false;
		s1++;
		s2--;
	}
	return true;
}

void MyString::compress(char src[]) {
	if (!src || !*src)
		return;
	int checker = 0;
	char *pos = src;
	while (*src) {
		if (checker & (1 << (*src - 'a'))) {
			src++;
		} else {
			checker |= 1 << (*src - 'a');
			*pos = *src;
			pos++;
			src++;
		}
	}
	if (*pos) {		
		*pos = '\0';
	}
}

void MyString::reverse(char src[]) {
	if (!src || !*src)
		return;
	char *end = src, *start = src;
	while (*end) 
		end++;
	end--;
	char c;
	while (start < end) {
		c = *start;
		*start = *end;
		*end = c;
		start++;
		end--;
	}

}

bool MyString::isUnique(char src[]) {
	if (!src)
		return false;
	int checker = 0;
	while (*src) {
		if (checker & (1 << (*src - 'a')))
			return false;
		checker |= 1 << (*src - 'a');
		src++;
	}
	return true;
}

char* MyString::strstr(char *str1, char * str2) {
	while (*str1) {
		char *p = str1;
		char *q = str2;
		while (*q && *p++ == *q++); 
		if (!*q) return str1;
		str1++;
	}
	return 0;
}