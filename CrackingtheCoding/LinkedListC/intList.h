#include <iostream>
using namespace std;

class ListNode
{
public:
	int value;
	ListNode *next;

	ListNode(int v);
	void print();
	void init();
	void init2();
	void init3();
	void removeDuplicates();
	ListNode *nthToLast(int n);
};

ListNode* ListNode::nthToLast(int n) 
{
	if (n < 0)
		return NULL;
	ListNode *p1 = this, *p2 = this;
	for (int i = 0; i < n ; i++)
	{
		if (!p2)
			return NULL;
		p2 = p2->next;
	}
	while (p2->next) 
	{
		p1 = p1->next;
		p2 = p2->next;
	}
	return p1;
}
void ListNode::removeDuplicates()
{
	int checker = 0;
	ListNode *p = this;
	ListNode *prev;
	while (p)
	{
		if (checker & 1 << p->value)
		{
			prev->next = p->next;
			p->next = NULL;
			p = prev->next;
		} else 
		{
			checker = checker | 1 << p->value;
			prev = p;
			p = p->next;
		}
	}
}
ListNode::ListNode(int v)
{
	this->value = v;
	this->next = NULL;
}

void ListNode::init()
{
	this->next = new ListNode(1);
	this->next->next = new ListNode(5);
	//this->next->next->next = new ListNode(5);
}

void ListNode::init2()
{
	this->next = new ListNode(9);
	this->next->next = new ListNode(2);
	//this->next->next->next = new ListNode(5);
}

void ListNode::init3()
{
	ListNode *n2 = new ListNode(9);
	ListNode *n3 = new ListNode(2);
	ListNode *n4 = new ListNode(7);
	ListNode *n5 = new ListNode(5);

	this->next = n2;
	n2->next = n3;
	n3->next = n4;
	n4->next = n5;
	n5->next = n3;
}

void ListNode::print()
{
	ListNode *p = this;
	while (p)
	{
		cout << p->value;
		if (p=p->next)
			cout << "->";
	}
	cout << endl;
}