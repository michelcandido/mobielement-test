// AmazonC.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "IntTree.h"
#include "IntList.h"
#include "IntArray.h"

#include <hash_map>

using namespace std;

class Base
{
       public:
          Base(){ cout<<"Constructor: Base"<<endl;}
          virtual ~Base(){ cout<<"Destructor : Base"<<endl;}
};
class Derived: public Base
{
     //Doing a lot of jobs by extending the functionality
       public:
           Derived(){ cout<<"Constructor: Derived"<<endl;}
           ~Derived(){ cout<<"Destructor : Derived"<<endl;}
};



int _tmain(int argc, _TCHAR* argv[])
{
	Trie *node = new Trie();
	node->initialize(node);
	char w1[] = "tree";
	char w2[] = "trie";
	node->addWord(node, w1, 0, strlen(w1) );
	node->addWord(node, w2, 0, strlen(w2) );
	char t[] = "trie";
	cout << node->countWords(node,t, 0, strlen(t)) << endl; 
	/*
	IntListNode *head = new IntListNode(0);
	head->init4();
	head->deleteDups(head);
	if (head)
		head->print();
		*/
	/*
	IntArray *arr = new IntArray();
	char src[] = "AbcD1eF2";
	arr->sort1aA(src, strlen(src));
	*/
	/*
	int s1[] = {1, 2, 4, 5, 6};
	int s2[] = {3, 5, 7, 9};
	IntArray *arr = new IntArray();
	arr->findMinPairs(s1, sizeof(s1)/sizeof(int), s2, sizeof(s2)/sizeof(int),3);
	*/
	/*
	BinaryTreeNode *bst = new BinaryTreeNode(5);
	bst->createBST();
	vector<int> nums;
	bst->findPath(bst, 12, nums);
	*/
	/*
	IntArray *arr = new IntArray();
	//arr->findNonDecreasing();
	arr->findCoins();
	*/
	/*
	IntListNode *head = new IntListNode(1);
	head->init3();
	head->copyExtra(head);
	*/

	/*
	IntArray *arr = new IntArray();
	stack<int> src;
	src.push(7);
	src.push(5);
	src.push(10);
	src.push(9);
	arr->sortStack(src);
	*/
	/*
	IntArray *arr = new IntArray();
	int a1[] = {1,2,3,5,6,7};
	int a2[] = {3,6,7,8};
	arr->findIntersection(a1, 6, a2, 3);
	*/
	/*
	IntArray *arr = new IntArray();
	int src[] = {1,2,3,4};
	vector<vector<int>> subsets = arr->findSubSets(src,sizeof(src)/sizeof(int),0);
	for (unsigned int i = 0; i < subsets.size(); i++) {
		vector<int> sets = subsets[i];
		for (unsigned int j = 0; j < sets.size(); j++) {
			cout << sets[j] << " ";
		}
		cout << endl;
	}
	cout << endl;
	*/
	/*
	int src[] = {4,8,10,12,14,20,22};
	BinaryTreeNode *node = new BinaryTreeNode(0);
	BinaryTreeNode *newNode = node->createBST(src,0,sizeof(src)/sizeof(int)-1);
	newNode->inOrder(newNode);
	//cout << endl << sizeof(src) << endl;
	BinaryTreeNode *lca = newNode->findLCA(newNode, 20, 22);
	if (lca)
		cout << endl << lca->data << endl;
	else
		cout << endl << "not found." << endl;
	*/

	/*
	cout << strlen("hello") << endl;
	cout << sizeof("hello") << endl;
	*/

	/*
	Base *Var = new Derived();
    delete Var;
	*/
	
	/*
	IntArray *arr = new IntArray();
	int src[] = {1, 2, 3, 1, 3, 0, 6};

	arr->findDuplicates(src, 7);
	*/

	//cout << arr->equilibrium(src, 7) << endl;

	//arr->nextGreaterElement(src, 4);
	
	//arr->maxOfSubArray(src, 10, 4);

	//cout << arr->findMinDistance(src, 12,3,6) << endl;

	/*
	IntListNode *l1 = new IntListNode(1);
	l1->init();
	l1->print();

	IntListNode *l2 = new IntListNode(2);
	l2->init2();
	l2->print();

	l1->merge(l1,l2);
	l1->print();
	*/

	/*
	IntArray *arr = new IntArray();
	int src[] = {11,12,13,21,22,23,31,32,33};
	arr->matrixTransposition(src, 3);
	*/
	/*
	BinaryTreeNode *bst = new BinaryTreeNode(5);
	bst->createBST();
	bst->preOrder(bst);
	cout << endl;
	*/

	/*
	char *src = "boob";
	cout << isPalindrome(src, 0, strlen(src)-1) << endl;
	*/

	/*
	BinaryTreeNode *bst = new BinaryTreeNode(5);
	bst->createBST();

	cout << bst->isBST(bst) << endl;

	BinaryTreeNode *bt = new BinaryTreeNode(5);
	bt->createBT();

	cout << bt->isBST(bt) << endl;
	*/
	
	/*
	int src[] = {-2, 1, -3, 4, -1, 2, 1, -5, 4};
	maxSubArray(src, 9);
	*/
	/*
	int src[] = { 5, 1, 2, 1, 1, 5, 2};
	swapSort(src, 7);
	*/
	
	/*
	IntListNode *root = new IntListNode(1);
	root->init();
	root->print();
	root = root->reverse(root);
	root->print();
	root = root->reverseRecursive(root, 0);
	root->print();
	*/

	/*
	SuccessorTreeNode *root = new SuccessorTreeNode(5);
	root->init();
	root->inorder(root);
	cout << endl;
	SuccessorTreeNode *temp = NULL;
	root->fillSuccessor(root,temp);
	cout << endl;
	root->inorderWithSuccessor(root);
	cout << endl;
	*/
	return 0;
}

