#include <iostream>
#include <hash_set>
using namespace std;

class IntListNode
{
public:
	int data;
	IntListNode *next;
	IntListNode *extra;

	IntListNode(int value);	
	void init();
	void init2();
	void init3();
	void init4();
	void print();
	IntListNode* reverse(IntListNode *root);
	IntListNode* reverseRecursive(IntListNode *root, IntListNode *prev);
	IntListNode* merge(IntListNode *l1, IntListNode *l2);
	IntListNode* sort(IntListNode *l);
	IntListNode* getLastNth(IntListNode *head, int n);
	void copyExtra(IntListNode *head);
	void deleteDups(IntListNode *&head);
	void init0(IntListNode *head, int src[], int size);
	void removeDups(IntListNode *head);
};

void IntListNode::removeDups(IntListNode *head) {
	int checker = 0;
	IntListNode *prev;
	while (head) {
		if (checker & (1 << head->data))
			prev->next = head->next;
		else {
			checker |= (1 << head->data);
			prev = head;
		}
		head = head->next;
	}
}
void IntListNode::init0(IntListNode *head, int src[], int size) {
	IntListNode *p = head;
	for (int i = 0; i < size; i++) {
		IntListNode *node = new IntListNode(src[i]);
		p->next = node;
		p = node;
	}
}

void IntListNode::deleteDups(IntListNode *&head) {
	IntListNode *p = head;
	IntListNode *prev = 0;
	while (p) {
		if (p->next && p->data == p->next->data) {
			while (p->next && p->data == p->next->data) {
				p->next = p->next->next;
			}
			if (prev) {
				prev->next = p->next;
				p = p->next;
			} else {
				head = p->next;
				p = p->next;
			}
		} else {
			prev = p;
			p = p->next;
		}
	}
}

void IntListNode::init4() {
	IntListNode *p1 = new IntListNode(1);
	IntListNode *p2 = new IntListNode(2);
	IntListNode *p3 = new IntListNode(3);
	IntListNode *p4 = new IntListNode(3);
	IntListNode *p5 = new IntListNode(4);
	IntListNode *p6 = new IntListNode(4);
	IntListNode *p7 = new IntListNode(5);
	this->next = p1;
	p1->next = p2;
	p2->next = p3;
	p3->next = p4;
	p4->next = p5;
	p5->next = p6;
	p6->next = p7;
	
}

void IntListNode::copyExtra(IntListNode *head) {
	IntListNode *newHead,  *current = head, *newCurrent, *prev, *next;
	while (current) {
		newCurrent = new IntListNode(current->data);		
		if (current != head)
			prev->next = newCurrent;
		else
			newHead = newCurrent;
		prev = newCurrent;

		newCurrent->extra = current->next;
		current->next = newCurrent;
		current = newCurrent->extra;
	}
	current = head;
	newCurrent = newHead;
	while (newCurrent) {
		next = newCurrent->extra;
		newCurrent->extra = current->extra->next;
		newCurrent = newCurrent->next;
		current = next;
	}
	newCurrent = newHead;
	while (newCurrent) {
		cout << newCurrent->data << " extra:" << newCurrent->extra->data << endl;
		newCurrent = newCurrent->next;
	}
}

void IntListNode::init3()
{
	IntListNode *p1 = new IntListNode(3);
	IntListNode *p2 = new IntListNode(9);
	IntListNode *p3 = new IntListNode(7);
	
	this->next = p1;
	this->extra = p1;
	p1->next = p2;
	p1->extra = p3;
	p2->next = p3;
	p2->extra = p1;
	p3->next = 0;
	p3->extra = p2;	
}

IntListNode* IntListNode::getLastNth(IntListNode *head, int n) {
	if (n < 1) return NULL;
	IntListNode *p1 = head;
	IntListNode *p2 = head;
	while (p1 && n-- > 0)
		p1 = p1->next;
	if (n > 0) return NULL;
	while (p1) {
		p1 = p1->next;
		p2 = p2->next;
	}
	return p2;
}

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