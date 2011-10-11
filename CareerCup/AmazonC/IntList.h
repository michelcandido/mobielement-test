#include <iostream>
using namespace std;

class IntListNode
{
public:
	int data;
	IntListNode *next;

	IntListNode(int value);
	void init();
	void print();
	IntListNode* reverse(IntListNode *root);
	IntListNode* reverseRecursive(IntListNode *root, IntListNode *prev);
};

IntListNode::IntListNode(int value)
{
	data = value;
	next = 0;
}

void IntListNode::init()
{
	this->next = new IntListNode(3);
	this->next->next = new IntListNode(5);
	this->next->next->next = new IntListNode(7);
}

void IntListNode::print()
{
	IntListNode *node = this;
	while(node)
	{
		cout << node->data << " ";
		node = node->next;
	}
	cout << endl;
}

IntListNode* IntListNode::reverse(IntListNode *root)
{
	if (!root) return 0;
	IntListNode *temp = root;
	IntListNode *prev = 0;
	while (root)
	{
		temp = root->next;
		root->next = prev;
		prev = root;
		root = temp;
	}
	return prev;
}

IntListNode* IntListNode::reverseRecursive(IntListNode *root, IntListNode *prev)
{
	IntListNode *temp;
	if (!root->next)
	{
		root->next = prev;
		return root;
	}
	else
	{
		temp = reverseRecursive(root->next, root);
		root->next = prev;
		return temp;
	}
}