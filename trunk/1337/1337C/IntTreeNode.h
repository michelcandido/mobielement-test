#include <stack>;
using namespace std;

class IntTreeNode {
public:
	int value;
	IntTreeNode *left;
	IntTreeNode *right;

	IntTreeNode(int);
	void init();

	void inorder(IntTreeNode *root);
	
	void printLeftEdges(IntTreeNode *root, bool print);
	void printRightEdges(IntTreeNode *root, bool print);
	void clockPrint(IntTreeNode *root);

	void printZigZag();
	
	bool isBSTHelper(IntTreeNode *node, int low, int high);
	bool isBST(IntTreeNode *node);
};

IntTreeNode::IntTreeNode(int value) {
	this->value = value;
	this->left = NULL;
	this->right = NULL;	
}

void IntTreeNode::init() {	
	this->left = new IntTreeNode(5);	
	this->right = new IntTreeNode(15);	
	
	this->right->left = new IntTreeNode(12);	
	this->right->right = new IntTreeNode(20);
}

void IntTreeNode::inorder(IntTreeNode *root) {
	if (!root) 
		return;
	inorder(root->left);
	printf("%d\n", root->value);
	inorder(root->right);
}

void IntTreeNode::printLeftEdges(IntTreeNode *root, bool print) {
	if (!root)
		return;
	if (print || (!root->left && !root->right))
		printf("%d ", root->value);
	root->printLeftEdges(root->left, print);
	root->printLeftEdges(root->right, false);
}

void IntTreeNode::printRightEdges(IntTreeNode *root, bool print) {
  if (!root) return;
  printRightEdges(root->left, false);
  printRightEdges(root->right, print);
  if (print || (!root->left && !root->right))
    printf("%d ", root->value);
}


void IntTreeNode::clockPrint(IntTreeNode *root) {
	if (!root) return;
		printf("%d ", root->value);
		root->printLeftEdges(root->left, true);
		root->printRightEdges(root->right, true);
}

void IntTreeNode::printZigZag() {
	stack<IntTreeNode*> current, next;
	bool left2right = true;
	current.push(this);
	while (!current.empty()) {
		IntTreeNode *node = current.top();
		current.pop();
		if (node!=NULL) {
			printf("%d ", node->value);
			if (left2right) {
				next.push(node->left);
				next.push(node->right);
			} else {
				next.push(node->right);
				next.push(node->left);
			}
		}
		if (current.empty()) {
			printf("\n");

			left2right = !left2right;			
			swap(current, next);

		}

	}
}

bool IntTreeNode::isBSTHelper(IntTreeNode *node, int low, int high) {
	if (node == NULL) 
		return true;
	if (low < node->value && node->value < high)
		return isBSTHelper(node->left, low, node->value) && isBSTHelper(node->right, node->value, high);
	else return false;
}

bool IntTreeNode::isBST(IntTreeNode *node)
{	
	return isBSTHelper(node, -9999, 9999);
}