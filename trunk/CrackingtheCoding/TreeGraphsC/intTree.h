#include <iostream>
using namespace std;

class BinTreeNode
{
public:
	int value;
	BinTreeNode *left;
	BinTreeNode *right;

	BinTreeNode(int v);
	void inorder(BinTreeNode *root);
	
};

BinTreeNode::BinTreeNode(int v)
{
	value = v;
	left = 0;
	right = 0;
}

void BinTreeNode::inorder(BinTreeNode *root)
{
	if (!root) return;
	inorder(root->left);
	cout << root->value << "-";
	inorder(root->right);
}