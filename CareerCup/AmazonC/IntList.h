#include <iostream>
using namespace std;

class IntListNode
{
public:
	int data;
	IntListNode *next;

	IntListNode(int value);
	void init();
	void init2();
	void print();
	IntListNode* reverse(IntListNode *root);
	IntListNode* reverseRecursive(IntListNode *root, IntListNode *prev);
	IntListNode* merge(IntListNode *l1, IntListNode *l2);
	IntListNode* sort(IntListNode *l);
};

IntListNode* IntListNode::sort(IntListNode *l){
	if (!l) return 0;
	if (!l->next) return l;
	IntListNode* temp = sort(l->next);
	IntListNode* head = temp;
	IntListNode* parent = 0;
	while (temp)
	{
		if (l->data > temp->data) {
			parent = temp;
			temp = temp->next;
		} else {
			break;
		}
	}
	if (parent == 0) {
		head = l;
		head->next = temp;
	} else {
		parent->next = l;
		l->next = temp;
	}
	return head;
}

IntListNode* IntListNode::merge(IntListNode *l1, IntListNode *l2)
{
	IntListNode* list = l1;
	while (list->next)
		list = list->next;
	list->next = l2;
	
	return sort(l1);
}
IntListNode::IntListNode(int value)
{
	data = value;
	next = 0;
}

void IntListNode::init()
{
	this->next = new IntListNode(3);
	this->next->next = new IntListNode(9);
	this->next->next->next = new IntListNode(7);
}

void IntListNode::init2()
{
	this->next = new IntListNode(4);
	this->next->next = new IntListNode(6);
	this->next->next->next = new IntListNode(8);
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