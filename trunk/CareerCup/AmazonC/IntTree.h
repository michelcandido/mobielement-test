#include <iostream>
#include <stack>
#include <queue>
using namespace std;

class BinaryTreeNode
{
public:
	int data;
	BinaryTreeNode *left, *right;
	BinaryTreeNode *last, *next;
	BinaryTreeNode(int value);

	void createBST();
	void createBT();
	bool isBST(BinaryTreeNode *root);
	void preOrder(BinaryTreeNode *root);
	void inOrder(BinaryTreeNode *root);
	BinaryTreeNode* createBST(int src[], int begin, int end);
	BinaryTreeNode* findLCA(BinaryTreeNode *root, int n1, int n2);
	void linkSameLevel(BinaryTreeNode *root);
};

BinaryTreeNode::BinaryTreeNode(int value)
{
	data = value;
	left = 0;
	right = 0;
	last = 0;
	next = 0;
}

void BinaryTreeNode::linkSameLevel(BinaryTreeNode *root) {
	queue<BinaryTreeNode*> q1;
	q1.push(root);
	while (!q1.empty()) {
		BinaryTreeNode* prev = 0;
		queue<BinaryTreeNode*> q2;
		BinaryTreeNode* cur = q1.front();
		q1.pop();
		cur->last = prev;

		if (cur->left)
			q2.push(cur->left);
		if (cur->right)
			q2.push(cur->right);

		while (!q1.empty()) {
			prev = cur;
			cur = q1.front();
			q1.pop();
			
			if (cur->left)
				q2.push(cur->left);
			if (cur->right)
				q2.push(cur->right);
			
			cur->last = prev;
			prev->next = cur;
		}
		cur->next = 0;
		q1 = q2;
	}

}

BinaryTreeNode* BinaryTreeNode::findLCA(BinaryTreeNode *root, int n1, int n2) {
	if (!root || root->data == n1 || root->data == n2)
		return 0;
	if (root->right && (root->right->data == n1 || root->right->data == n2))
		return root;
	if (root->left && (root->left->data == n1 || root->left->data == n2))
		return root;
	if (root->data > n1 && root->data < n2)
		return root;
	if (root->data > n1 && root->data > n2)
		return findLCA(root->left, n1, n2);
	if (root->data < n1 && root->data < n2)
		return findLCA(root->right, n1, n2);
	return 0;
}

BinaryTreeNode* BinaryTreeNode::createBST(int src[], int begin, int end) {
	if (begin > end) return 0;
	int mid = begin + (end - begin) / 2;
	BinaryTreeNode *node = new BinaryTreeNode(src[mid]);
	node->left = createBST(src, begin, mid - 1);
	node->right = createBST(src, mid + 1, end);
	return node;
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

void BinaryTreeNode::inOrder(BinaryTreeNode *root)
{
	if (!root) return;
	inOrder(root->left);
	cout << root->data << " ";	
	inOrder(root->right);
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