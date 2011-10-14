#include <iostream>
using namespace std;

class BinaryTreeNode
{
public:
	int data;
	BinaryTreeNode *left, *right;
	BinaryTreeNode(int value);

	void createBST();
	void createBT();
	bool isBST(BinaryTreeNode *root);
	void preOrder(BinaryTreeNode *root);
};

BinaryTreeNode::BinaryTreeNode(int value)
{
	data = value;
	left = 0;
	right = 0;
}

void BinaryTreeNode::createBST()
{
	BinaryTreeNode *one = new BinaryTreeNode(1);
	BinaryTreeNode *four = new BinaryTreeNode(4);
	BinaryTreeNode *three = new BinaryTreeNode(3);
	three->left = one;
	three->right = four;

	BinaryTreeNode *six = new BinaryTreeNode(6);
	BinaryTreeNode *seven = new BinaryTreeNode(7);
	BinaryTreeNode *nine = new BinaryTreeNode(9);

	this->left = three;
	this->right = seven;
	seven->left = six;
	seven->right = nine;
}

void BinaryTreeNode::createBT()
{
	BinaryTreeNode *eight = new BinaryTreeNode(8);
	BinaryTreeNode *one = new BinaryTreeNode(1);
	BinaryTreeNode *sixteen = new BinaryTreeNode(16);
	BinaryTreeNode *three = new BinaryTreeNode(3);
	BinaryTreeNode *nine = new BinaryTreeNode(9);
	BinaryTreeNode *two = new BinaryTreeNode(2);
	

	this->left = eight;
	this->right = three;
	eight->left = one;
	eight->right = sixteen;
	three->left = nine;
	three->right = two;
}

bool BinaryTreeNode::isBST(BinaryTreeNode *root)
{
	if (!root) return true;
	if (root->left && root->left->data > root->data) return false;
	if (root->right && root->right->data < root->data) return false;
	return isBST(root->left) && isBST(root->right);
}

void BinaryTreeNode::preOrder(BinaryTreeNode *root)
{
	if (root)
	{
		cout << root->data << " ";
		preOrder(root->left);
		preOrder(root->right);
	}
}


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