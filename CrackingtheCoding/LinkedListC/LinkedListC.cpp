// LinkedListC.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "intList.h"

ListNode* Q4(ListNode *p1, ListNode *p2)
{
	if (!p1 || !p2) return NULL;
	ListNode *p3 = new ListNode(0);
	ListNode *result = p3;
	int add = 0;
	
	while (p1 && p2)
	{
		add = p1->value + p2->value + add;
		if (add > 9)
		{
			p3->value = add % 10;
			add = add / 10;
		} else {
			p3->value = add;
			add = 0;
		}		
		p1 = p1->next;
		p2 = p2->next;
		if (p1 && p2) 
		{
			p3->next = new ListNode(0);
			p3 = p3->next;		
		} else {
			if (p1)
				p3->next = p1;
			else if (p2)
				p3->next = p2;			
		}
	}	
	
	if (add) {
		if (!p3->next)
			p3->next = new ListNode(0);
		p3 = p3->next;
		add = add + p3->value;
		if (add > 9)
		{
			p3->value = add % 10;
			p3->next = new ListNode(add / 10);
		} else {
			p3->value = add;
		}
	}

	return result;
}


int _tmain(int argc, _TCHAR* argv[])
{
	ListNode *root = new ListNode(1);
	root->init3();
	root->print();

	/*
	ListNode *p1 = new ListNode(3);
	p1->init();
	p1->print();
	ListNode *p2 = new ListNode(5);
	p2->init2();
	p2->print();

	ListNode *p3 = Q4(p1, p2);
	p3->print();
	*/
	/*
	ListNode *root = new ListNode(1);
	root->init();
	root->print();
	*/
	/*
	printf("removed dupliates...\n");
	root->removeDuplicates();
	root->print();
	*/
	/*
	ListNode *nth = root->nthToLast(9);
	printf("nth to last %d\n", nth?nth->value:-1);
	*/
	
	return 0;
}

