using namespace std;

class IntLinkedListNode {
public:
	int value;
	IntLinkedListNode *prev;
	IntLinkedListNode *next;

	IntLinkedListNode(int v);
	void initSortedLinkedList();
	IntTreeNode* sortedListToBST(IntLinkedListNode *& list, int start, int end);
	void reverse(IntLinkedListNode *&head);
	void reverse2(IntLinkedListNode *&head);
	void print();
};

void IntLinkedListNode::print() {
	IntLinkedListNode *node = this;
	while (node != NULL) {
		cout << node->value << " ";
		node = node->next;
	}
	cout << endl;
}

void IntLinkedListNode::reverse(IntLinkedListNode *&head){
	if (head == NULL)
		return;
	IntLinkedListNode *cur = head;
	IntLinkedListNode *prev = NULL;
	while (cur != NULL) {
		IntLinkedListNode *next = cur->next;
		cur->next = prev;
		prev = cur;
		cur = next;
	}
	head = prev;
}

void IntLinkedListNode::reverse2(IntLinkedListNode *&head) {
	if (head == NULL)
		return;
	IntLinkedListNode *next = head->next;
	if (next == NULL)
		return;
	reverse2(next);
	head->next->next = head;
	head->next = NULL;
	head = next;
}

IntLinkedListNode::IntLinkedListNode(int v) {
	this->value = v;
	this->prev = NULL;
	this->next = NULL;
}

void IntLinkedListNode::initSortedLinkedList() {
	this->next = new IntLinkedListNode(3);
	this->next->next = new IntLinkedListNode(5);
	this->next->next->next = new IntLinkedListNode(9);
}

class IntSortedLinkedList {
public: 
	IntLinkedListNode *root;
	IntSortedLinkedList();
	IntTreeNode* sortedListToBST(IntSortedLinkedList *& list, int start, int end);
};

IntSortedLinkedList::IntSortedLinkedList() {
	root = new IntLinkedListNode(1);
	root->next = new IntLinkedListNode(3);
	root->next->next = new IntLinkedListNode(5);
	root->next->next->next = new IntLinkedListNode(9);
}

IntTreeNode* IntLinkedListNode::sortedListToBST(IntLinkedListNode *& list, int start, int end) {
	if (start > end) return NULL;
	int mid = start + (end - start) / 2;
	IntTreeNode *left = sortedListToBST(list, start, mid - 1);
	IntTreeNode *parent = new IntTreeNode(list->value);
	parent->left = left;
	list = list->next;
	parent->right = sortedListToBST(list, mid + 1, end);
	return parent;
}

class IntCyclicList {
public:
	IntLinkedListNode *root;

	IntCyclicList();
	void print();
	void insert(int value);
};

IntCyclicList::IntCyclicList() {
	root = new IntLinkedListNode(1);
	root->next = new IntLinkedListNode(3);
	root->next->next = new IntLinkedListNode(5);
	root->next->next->next = root;
}

void IntCyclicList::print() {
	IntLinkedListNode *cur = root;
	while (cur != NULL) {
		printf("%d ", cur->value);
		cur = cur->next;
		if (cur == root)
			break;
	}
	printf("\n");
}

void IntCyclicList::insert(int value) {
	IntLinkedListNode *cur = root, *prev;
	while (cur != NULL) {
		prev = cur;
		cur = cur->next;
		if (cur == root) {
			if (value > prev->value) {
				prev->next = new IntLinkedListNode(value);
				prev->next->next = root;
			} else {
				IntLinkedListNode *node = new IntLinkedListNode(value);
				node->next = root;
				root = node;
				prev->next = root;
			}
			break;
		}
		if (value > prev->value && value < cur->value) {
			prev->next = new IntLinkedListNode(value);
			prev->next->next = cur;
			break;
		}
	}
}