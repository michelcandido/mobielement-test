// 1337C.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <string>
#include <iostream>
#include "IntTreeNode.h"
#include "IntLinkedList.h"
#include <hash_set>

bool isMatch(char *str, const char* pattern) {
  while (*pattern)
    if (*str++ != *pattern++)
      return false;
  return true;
}

void replace(char str[], const char *pattern) {
  if (str == NULL || pattern == NULL) return;
  char *pSlow = str, *pFast = str;
  int pLen = strlen(pattern);
  while (*pFast != '\0') {
    bool matched = false;
    while (isMatch(pFast, pattern)) {
      matched = true;
      pFast += pLen;
    }
    if (matched)
      *pSlow++ = 'X';
    // tricky case to handle here:
    // pFast might be pointing to '\0',
    // and you don't want to increment past it
    if (*pFast != '\0')
      *pSlow++ = *pFast++;  // *p++ = (*p)++
  }
  // don't forget to add a null character at the end!
  *pSlow = '\0';
  printf("%s\n",str);
}

int backtrack(int r, int c, int m, int n) {
  if (r == m && c == n)
    return 1;
  if (r > m || c > n)
    return 0;
 
  return backtrack(r+1, c, m, n) + backtrack(r, c+1, m, n);
}

const char* myStrstr(const char *src, const char *pat) {
	const char *pSrc = src;
	const char *pPat = pat;
	int len = 0;
	while (*pSrc != '\0') {
		if (*pSrc == *pPat) {
			if (*(++pPat) == '\0') {
				return pSrc-len;
			}
			len++;
		} else {
			pPat = pat;
			len = 0;
		}
		pSrc++;
	}
	return 0;
}

void dec2Bin(int num) {
	int remainder;
	if (num <= 1) {
		printf("%d",num);
		return;
	}
	remainder = num % 2;
	dec2Bin(num >> 1 );
	printf("%d",remainder);
}

void reversebits(int num) {
	if (num <= 1) {
		printf("%d",num);
	}

	while (num > 0) {
		printf("%d",num%2);
		num = num >> 1;
	}
}

int kthSmallest(const int a[], int m, const int b[], int n, int k) {
	int idx = 0, pa = 0, pb = 0, result;

	while (idx <= k) {
		if (pa < m && pb < n) {
			if (a[pa] < b[pb]) {
				result = a[pa];
				pa++;
			} else {
				result = b[pb];
				pb++;
			}
		} else if (pa >= m) {
			result = b[pb];
			pb++;
		} else if (pb >= n) {
			result = a[pa];
			pa++;
		}
		idx++;
	}
	
	return result;
}

void twoSum(int numbers[], int size, int target, int &index1, int &index2) {
	for (int i = 0; i < size; i++) {
		int t = target - numbers[i];
		for (int j = i + 1; j < size; j++) {
			if (numbers[j] == t) {
				index1 = i;
				index2 = j;
				return;
			}
		}
	}
}

void buyAndSell(const int price[], int size, int &buy, int &sell) {
	int max = 0;
	for (int i = 0; i < size - 1; i++) {
		for (int j = i + 1; j < size; j++) {
			if ((price[j] - price[i]) > max) {
				buy = i;
				sell = j;
				max = price[j] - price[i];
			}
		}
	}
}

void buyAndSell2(const int price[], int size, int &buy, int &sell) {
	int min = 0;
  int maxDiff = 0;
  buy = sell = 0;
  for (int i = 0; i < size; i++) {
    if (price[i] < price[min])
      min = i;
    int diff = price[i] - price[min];
    if (diff > maxDiff) {
      buy = min;
      sell = i;
      maxDiff = diff;
    }
  }
}

void subString(char src[]) {
	if (*src == '\0') 
		return;
	else if (*(src+1) != '\0')
		printf("%s\n",src);
	printf("%c\n", *src);
	subString(++src);
}

bool containsElement(char src[], char pat[]) {
	long iSrc = 0;
	while (*src != '\0') {
		iSrc = iSrc | (1 << *src);
		src++;
	}
	while (*pat != '\0') {
		if (!(iSrc & (1 << *pat)))
			return false;		
		pat++;
	}

	return true;
}

bool minWindow(const char *s, const char *t, int &minBegin, int &minEnd) {
	int sLen = strlen(s);
	int tLen = strlen(t);
	int needToFind[256] = {0};
	int hasFound[256] = {0};
	int minWindowLen = sLen;
	int count = 0;

	for (int i = 0; i < tLen; i++)
		needToFind[t[i]]++;

	for (int begin = 0, end = 0; end < sLen; end++) {
		if (needToFind[s[end]] == 0)
			continue;
		hasFound[s[end]]++;
		if (hasFound[s[end]] <= needToFind[s[end]])
			count++;

		if (count == tLen) {
			while (needToFind[s[begin]] == 0 || hasFound[s[begin]] > needToFind[s[begin]]) {
				if (hasFound[s[begin]] > needToFind[s[begin]])
					hasFound[s[begin]]--;
				begin++;
			}

			int windowLen = end - begin + 1;
			if (windowLen < minWindowLen) {
				minBegin = begin;
				minEnd = end;
				minWindowLen = windowLen;
			}
		}
	}
	return (count == tLen)? true:false;
}

string numToStr(int n) {
  string str(1, 'a' + n%26);
  n = n/26;
  while (n != 0) {
    str = (char)('a' + (n-1)%26) + str;
    n = (n-1)/26;
  } 
  return str;
}

bool binSearch(int data[], int target, int start, int end, int &idx) {
	if (start > end)
		return false;
	int mid = start + (end - start) / 2;
	if (data[mid] == target) {
		idx = mid;
		return true;
	} else {
		if (target > data[mid])
			return binSearch(data, target, mid + 1, end, idx);
		else
			return binSearch(data, target, start, mid - 1, idx);
	}
	
}

int numOfOne(int num) {
	int checker = 1;
	int result = 0;
	while (num != 0) {
		if ((checker & num) == 1)
			result++;
		num = (num >> 1);
	}
	return result;
}

int _tmain(int argc, _TCHAR* argv[])
{
	IntLinkedListNode *head = new IntLinkedListNode(1);
	head->initSortedLinkedList();
	head->print();
	head->reverse(head);
	head->print();
	head->reverse2(head);
	head->print();
	/*
	int num = 11;
	printf("there are %d 1s in number %d\n", numOfOne(num), num); 
	*/

	//int data[][5] = {{1,4,7,11,15},{2,5,6,12,19}};
	/*
	int data[] = {1,3,4,5,7,8};
	int result;
	if (binSearch(data,2,0,5,result))
		cout << "idx:" << result << endl;
	else 
		cout << "not found" << endl;
	*/
	//cout << numToStr(52) << endl;
	/*
	char src[] = "ADOBECODEBANC";
	char pat[] = "ABC";
	printf("%s contains %s: %d\n", src, pat, containsElement(src, pat));
	int minBegin = 0, minEnd = 0;
	if (minWindow(src, pat, minBegin, minEnd)) {
		char *result = (char*)(malloc(strlen(src)*sizeof(char)));
		result = src+minBegin;
		*(result+(minEnd - minBegin) + 1) = '\0';
		printf("found: start at %d, end at %d. It is %s\n", minBegin, minEnd, result);
	}
	else 
		printf("not found.\n");
	*/
	//subString("abc");
	
	/*
	int buy = 0, sell = 0;
	int price[] = {3,  2, 5, 6, 7, 3, 8, 7, 4,1};
	buyAndSell2(price, 10, buy, sell);
	printf("buy at %d, sell at %d\n", price[buy], price[sell]);
	*/
	
	/*
	IntLinkedListNode *list = new IntLinkedListNode(1);
	list->initSortedLinkedList();
	IntTreeNode *root = list->sortedListToBST(list, 0, 3);
	root->inorder(root);
	*/

	/*
	int num[] = {1, 3, 5, 2, 4, 6};
	int target = 11;
	int index1 = -1, index2 = -1;
	twoSum(num,6,target, index1, index2);
	if (index1 == -1 || index2 == -1) {
		printf("cannot found\n");
	} else {
		printf("%d = %d + %d\n", target, num[index1], num[index2]);
	}
	*/

	/*
	int a[] = {0};
	int b[] = {1,2,3};
	printf("%d\n", kthSmallest(a,1,b,3,3));
	*/

	/*
	int num = 100;
	dec2Bin(num);
	printf("\n");
	reversebits(num);
	printf("\n");
	*/

	/*
	char str[] = "ababa";
	char *pattern = "a";
	replace(str, pattern);
	
	printf("%d",backtrack(1,1,3,7));
	*/

	/*
	IntTreeNode root(30);
	IntTreeNode *tree = &root;	
	tree->init();
	tree->inorder(tree);
	tree->clockPrint(tree);
	*/
	
	//printf("%s\n", myStrstr("aabbaabbaaabbbaabb", "aaabb"));

	/*
	IntTreeNode root(3);
	IntTreeNode *tree = &root;	
	tree->init();
	tree->printZigZag();

	*/
	/*
	IntTreeNode root(10);
	IntTreeNode *tree = &root;	
	tree->init();
	printf("isBST=%x\n", tree->isBST(tree)); 
	*/

	/*
	IntCyclicList *list = new IntCyclicList();
	list->print();
	list->insert(0);
	list->print();
	*/
	
	
	
	return 0;
}



