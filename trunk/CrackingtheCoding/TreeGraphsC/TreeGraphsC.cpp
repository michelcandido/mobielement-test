// TreeGraphsC.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "intTree.h"

BinTreeNode* Q3(int src[], int start, int end)
{
	if (start > end) return 0;
	int mid = start + (end - start) / 2;
	BinTreeNode *node = new BinTreeNode(src[mid]);
	node->left = Q3(src, start, mid - 1);
	node->right = Q3(src, mid + 1, end);
	return node;
}

int _tmain(int argc, _TCHAR* argv[])
{
	int src[] = {1,3,5,7,9,11};
	BinTreeNode *node = Q3(src, 0, 5);
	node->inorder(node);

	return 0;
}

