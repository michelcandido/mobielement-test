import java.util.ArrayList;
import java.util.LinkedList;


public class MyBinTree<T  extends Comparable<T>> {
    public T element;
    public MyBinTree<T> left = null;
    public MyBinTree<T> right = null;

    public MyBinTree(T e) {
        element = e;
    }

    public void mirror() {
        if (left == null || right == null)
            return;
        if (left != null)
            left.mirror();
        if (right != null)
            right.mirror();
        MyBinTree<T> t = left;
        left = right;
        right = t;
    }

    public int maxDepth() {
        if (left == null && right == null)
            return 1;
        int dl = 0, dr = 0;
        if (left != null)
            dl = left.maxDepth();
        else
            dl = 1;
        if (right != null)
            dr = right.maxDepth();
        else
            dr = 1;
        if (dl > dr)
            return dl+1;
        else
            return dr+1;
    }

    public boolean equals(MyBinTree<T> t) {
        boolean result = true;
        if (t == null)
            return false;
        if (!element.equals(t.element))
            return false;
        else {
            if (left != null)
                result &= left.equals(t.left);
            else if (t.left != null)
                return false;
            else
                result &= true;

            if (right != null)
                result &= right.equals(t.right);
            else if (t.right != null)
                return false;
            else
                result &= true;

            return result;
        }
    }

    public int getSize() {
        int size = 0;
        if (left != null)
            size += left.getSize();
        if (right != null)
            size += right.getSize();
        size++;
        return size;
    }

    public void bstPush(T e) {
        if (e.compareTo(element) == -1) {
            if (left == null) {
                MyBinTree<T> tree = new MyBinTree<T>(e);
                left = tree;
            } else {
                left.bstPush(e);
            }
        } else if (e.compareTo(element) == 1) {
            if (right == null) {
                MyBinTree<T> tree = new MyBinTree<T>(e);
                right = tree;
            } else {
                right.bstPush(e);
            }
        }
    }

    public void binPrint() {
        if (left != null)
            left.binPrint();
        System.out.print(element + " ");
        if (right != null)
            right.binPrint();
    }

    public void prePrint() {
        System.out.print(element + " ");
        if (left != null)
            left.prePrint();
        if (right != null)
            right.prePrint();
    }

    public void levelPrint() {
        LinkedList<MyBinTree<T>> children = new LinkedList<MyBinTree<T>>();
        MyBinTree<T> node = this;
        while (node!=null) {
            System.out.print(node.element+" ");
            if (node.left!=null)
                children.add(node.left);
            if (node.right!=null)
                children.add(node.right);
            if (!children.isEmpty())
                node = children.remove();
            else
                break;
        }
    }
}
