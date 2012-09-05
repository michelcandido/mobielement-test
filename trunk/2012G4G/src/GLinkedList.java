
public class GLinkedList {

    /**
     * @param args
     */
    public static void main(String[] args) {
        //testMyList();
        //testMyBinTree();
        testLinkedList2BalancedBST();
    }

    static void testLinkedList2BalancedBST() {
        Integer[] a = {1,2,3};
        MyLinkedList<Integer> list = new MyLinkedList<Integer>(a);
        list.print();
        linkedList2BalancedBST(list,1,list.getCount()).prePrint();
    }
    static MyBinTree<Integer> linkedList2BalancedBST(MyLinkedList<Integer> list, int first, int last) {
        if (first == last ) {
            MyBinTree<Integer> tree = new MyBinTree<Integer>(list.getNth(first).element);
            return tree;
        } else if (first > last) {
            return null;
        }
        int mid = first + (last - first)/2;
        MyBinTree<Integer> root = new MyBinTree<Integer>(list.getNth(mid).element);
        MyBinTree<Integer> left = linkedList2BalancedBST(list, first, mid - 1);
        root.left = left;
        MyBinTree<Integer> right = linkedList2BalancedBST(list, mid + 1, last);
        root.right = right;
        return root;
    }

    static void testMyList() {
        MyLinkedList<Integer> list = new MyLinkedList<Integer>();
        list.push(1);
        list.push(2);
        list.push(3);
        list.print();

        Integer[] a = {4,5,6};
        MyLinkedList<Integer> list2 = new MyLinkedList<Integer>(a);
        list2.print();
    }
}
