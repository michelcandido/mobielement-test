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
};

IntTreeNode::IntTreeNode(int value) {
	this->value = value;
	this->left = NULL;
	this->right = NULL;	
}

void IntTreeNode::init() {	
	this->left = new IntTreeNode(10);	
	this->right = new IntTreeNode(20);	
	this->left->left = new IntTreeNode(50);	
	this->right->left = new IntTreeNode(45);	
	this->right->right = new IntTreeNode(35);
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
