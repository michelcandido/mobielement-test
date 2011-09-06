// 1337C.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <string.h>
#include "IntTreeNode.h"

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


int _tmain(int argc, _TCHAR* argv[])
{
	/*
	char str[] = "ababa";
	char *pattern = "a";
	replace(str, pattern);
	
	printf("%d",backtrack(1,1,3,7));
	*/
	IntTreeNode root(30);
	IntTreeNode *tree = &root;
	/*
	tree->left = &IntTreeNode(10);
	tree->right = &IntTreeNode(20);
	tree->left->left = &IntTreeNode(50);
	tree->right->left = &IntTreeNode(45);
	tree->right->right = &IntTreeNode(35);
	*/
	tree->init();
	tree->inorder(tree);
	tree->clockPrint(tree);
	return 0;
}


