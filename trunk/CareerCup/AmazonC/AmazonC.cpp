// AmazonC.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "IntTree.h"
#include "IntList.h"

#include <hash_map>

void swapSort(int src[], int size)
{
	int min = src[0], mid = src[0], max = src[0];
	for (int i = 0; i < size; i++)
	{
		if (src[i] == min || src[i] == mid || src[i] == max)
			continue;
		if (src[i] < min)
		{
			max = mid;
			mid = min;
			min = src[i];
			continue;
		}
		if (src[i] > min && src[i] < mid)
		{
			max = mid;
			mid = src[i];
			continue;
		}
		if (src[i] > mid)
		{
			max = src[i];
			continue;
		}
	}
	cout << min << " " << mid << " " << max << endl;
	int pos = -1;
	for (int i = 0; i < size; i++)
	{
		if (src[i] == min)
		{
			pos++;
			swap(src[i], src[pos]);
		}
	}
	for (int i = 0; i < size; i++)
	{
		cout << src[i] << " ";
	}
	cout << endl;
	pos = size;
	for (int i = size - 1; i >= 0; i--)
	{
		if (src[i] == max)
		{
			pos--;
			swap(src[i], src[pos]);
		}
	}
	for (int i = 0; i < size; i++)
	{
		cout << src[i] << " ";
	}
	cout << endl;

}

void maxSubArray(int src[], int size)
{
	int maxSoFar = 0, maxEndingHere = 0, start = 0, end = 0;
	for (int i = 0; i < size; i++)
	{
		maxEndingHere += src[i];
		if (maxEndingHere < 0 ) {
			maxEndingHere = 0;
			end = i+1;
			start = i+1;
		}
		if (maxEndingHere > maxSoFar) {
			maxSoFar = maxEndingHere;
			end = i;
		}
	}
	cout << src[start] << " " << src[end] << " " << maxSoFar << endl;
}

bool isPalindrome(const char *src, int start, int end) 
{
	if (end < start) return true;
	if (*(src+start) == *(src+end))
		return isPalindrome(src, start + 1, end - 1);
	else
		return false;
}

void matrixTransposition(int src[], int N)
{
	for (int i = 0; i < N; i++) {
		for (int j = 0; j < N; j++) {
			cout << src[i*N+j] << " ";
		}
		cout << endl;
	}
	cout << endl;
	
	for (int n = 0; n <= N -2;n++) {
		for (int m = n + 1; m <= N - 1; m++) {
			swap(src[n*N+m], src[m*N+n]);
		}
	}

	for (int i = 0; i < N; i++) {
		for (int j = 0; j < N; j++) {
			cout << src[i*N+j] << " ";
		}
		cout << endl;
	}
}

int _tmain(int argc, _TCHAR* argv[])
{
	int src[] = {11,12,13,21,22,23,31,32,33};
	matrixTransposition(src, 3);
	/*
	BinaryTreeNode *bst = new BinaryTreeNode(5);
	bst->createBST();
	bst->preOrder(bst);
	cout << endl;
	*/

	/*
	char *src = "boob";
	cout << isPalindrome(src, 0, strlen(src)-1) << endl;
	*/

	/*
	BinaryTreeNode *bst = new BinaryTreeNode(5);
	bst->createBST();

	cout << bst->isBST(bst) << endl;

	BinaryTreeNode *bt = new BinaryTreeNode(5);
	bt->createBT();

	cout << bt->isBST(bt) << endl;
	*/
	
	/*
	int src[] = {-2, 1, -3, 4, -1, 2, 1, -5, 4};
	maxSubArray(src, 9);
	*/
	/*
	int src[] = { 5, 1, 2, 1, 1, 5, 2};
	swapSort(src, 7);
	*/
	
	/*
	IntListNode *root = new IntListNode(1);
	root->init();
	root->print();
	root = root->reverse(root);
	root->print();
	root = root->reverseRecursive(root, 0);
	root->print();
	*/

	/*
	SuccessorTreeNode *root = new SuccessorTreeNode(5);
	root->init();
	root->inorder(root);
	cout << endl;
	SuccessorTreeNode *temp = NULL;
	root->fillSuccessor(root,temp);
	cout << endl;
	root->inorderWithSuccessor(root);
	cout << endl;
	*/
	return 0;
}

