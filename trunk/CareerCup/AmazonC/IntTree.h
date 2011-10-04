#include <iostream>
using namespace std;

class SuccessorTreeNode
{
public:
	int data;
	SuccessorTreeNode *left, *right;
	SuccessorTreeNode *successor;

	SuccessorTreeNode(int value);
	void init();
	void inorder(SuccessorTreeNode *root);
	void fillSuccessor(SuccessorTreeNode *root, SuccessorTreeNode *&prev);
	void inorderWithSuccessor(SuccessorTreeNode *root);
};

SuccessorTreeNode::SuccessorTreeNode(int value)
{
	data = value;
	left = 0; right = 0; successor = 0;
}

void SuccessorTreeNode::init()
{	
	this->left = new SuccessorTreeNode(7);
	this->right = new SuccessorTreeNode(3);
	this->left->left = new SuccessorTreeNode(9);
	this->left->right = new SuccessorTreeNode(11);
	this->right->left = new SuccessorTreeNode(1);
	this->right->right = new SuccessorTreeNode(4);
}

void SuccessorTreeNode::inorder(SuccessorTreeNode *root)
{
	if (!root) return;
	inorder(root->left);
	cout << root->data << " ";	
	inorder(root->right);
}

void SuccessorTreeNode::fillSuccessor(SuccessorTreeNode *root, SuccessorTreeNode *&prev)
{
	if (!root) return;
	fillSuccessor(root->left,prev);
	cout << root->data << " ";	
	if (!prev)
		prev = root;
	else {
		prev->successor = root;
		prev = root;
	}
	fillSuccessor(root->right,prev);
}

void SuccessorTreeNode::inorderWithSuccessor(SuccessorTreeNode *root)
{
	if (!root) 
		return;
	inorderWithSuccessor(root->left);
	cout << root->data << "(";
	if (root->successor)
		cout << root->successor->data;
	cout << ") ";
	inorderWithSuccessor(root->right);
}