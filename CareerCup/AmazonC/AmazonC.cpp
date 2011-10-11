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

int _tmain(int argc, _TCHAR* argv[])
{
	int src[] = {-2, 1, -3, 4, -1, 2, 1, -5, 4};
	maxSubArray(src, 9);
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

