using namespace std;

class IntLinkedListNode {
public:
	int value;
	IntLinkedListNode *prev;
	IntLinkedListNode *next;

	IntLinkedListNode(int v);
};

IntLinkedListNode::IntLinkedListNode(int v) {
	this->value = v;
	this->prev = NULL;
	this->next = NULL;
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