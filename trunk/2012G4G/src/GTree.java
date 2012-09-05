import java.util.ArrayList;
import java.util.Stack;


public class GTree {

    /**
     * @param args
     */
    public static void main(String[] args) {
        //testGetSize();
        //testEquals();
        //testMaxDepth();
        //testMirror();
        //testBuildBinTreeInPre();
        //testPrintAllPath();
        //testFindLCA();
        //testTreeToList();
        //testLevelPrint();
        //testGetDiameter();
        testInorderNoRecursion();
    }

    static MyBinTree<Integer>  getInorderSuccessor(MyBinTree<Integer> root) {
        if (root == null) {
            return null;
        }
        if (root.right == null)
            return null;
        else
            return getMin(root.right);

    }
    static MyBinTree<Integer> getMin(MyBinTree<Integer> root) {
        if (root == null)
            return null;
        if (root.left == null && root.right == null)
            return root;
        if (root.left != null)
            return getMin(root.left);
        else
            return root;
    }

    static void testInorderNoRecursion() {
        int[] in = {1,2,3,4,5};
        int[] pre = {4,2,1,3,5};
        MyBinTree<Integer> root = buildBinTreeInPre(pre, in, 0, pre.length - 1);
        inorderNoRecursion(root);
    }
    static void inorderNoRecursion(MyBinTree<Integer> root) {
        if (root == null)
            return;
        MyBinTree<Integer> node = root;
        Stack<MyBinTree<Integer>> stack = new Stack<MyBinTree<Integer>>();
        boolean done = false;
        while (!done) {
            if (node != null) {
                stack.push(node);
                node = node.left;
            } else {
                if (!stack.isEmpty()){
                    node = stack.pop();
                    System.out.println(node.element);
                    node = node.right;
                } else
                    done = true;
            }
        }
    }

    static void testGetDiameter() {
        int[] in = {1,2,3,4,5};
        int[] pre = {4,2,1,3,5};
        MyBinTree<Integer> root = buildBinTreeInPre(pre, in, 0, pre.length - 1);
        System.out.println(getDiameter(root));
    }
    static int getDiameter(MyBinTree<Integer> root) {
        if (root == null)
            return 0;
        int ld = getDiameter(root.left);
        int rd = getDiameter(root.right);
        int lh = 0, rh = 0;
        if (root.left != null)
            lh = root.left.maxDepth();
        if (root.right != null)
            rh = root.right.maxDepth();
        int max = 0;
        if (ld > rd)
            max = ld;
        else
            max = rd;
        if (max < lh+rh+1)
            max = lh+rh+1;
        return max;
    }

    static void testLevelPrint() {
        int[] in = {1,2,3,4,5};
        int[] pre = {4,2,1,3,5};
        MyBinTree<Integer> root = buildBinTreeInPre(pre, in, 0, pre.length - 1);
        root.levelPrint();
    }

    static void testTreeToList() {
        int[] in = {1,2,3,4,5};
        int[] pre = {4,2,1,3,5};
        MyBinTree<Integer> root = buildBinTreeInPre(pre, in, 0, pre.length - 1);

        ArrayList<MyBinTree<Integer>> result = treeToList(root);
        MyBinTree<Integer> node = result.get(0);
        for (int i = 0; i < 5; i++) {
            System.out.println(node.element+"("+node.left.element+","+node.right.element+")");
            node = node.right;
        }
    }
    static ArrayList<MyBinTree<Integer>> treeToList(MyBinTree<Integer> root) {
        ArrayList<MyBinTree<Integer>> result = new ArrayList<MyBinTree<Integer>>();
        if (root.left == null && root.right == null) {
            root.left = root;
            root.right = root;

            result.add(0,root);
            result.add(1,root);
            return result;
        }

        MyBinTree<Integer> lmin = null,lmax = null;
        if (root.left != null) {
            ArrayList<MyBinTree<Integer>> lresult = treeToList(root.left);
            lmin = lresult.get(0);
            lmax = lresult.get(1);
        }

        MyBinTree<Integer> rmin = null,rmax = null;
        if (root.right != null) {
            ArrayList<MyBinTree<Integer>> rresult = treeToList(root.right);
            rmin = rresult.get(0);
            rmax = rresult.get(1);
        }

        root.left = lmax;
        root.right = rmin;

        lmin.left = rmax;
        lmax.right = root;

        rmax.right = lmin;
        rmin.left = root;

        result.add(0,lmin);
        result.add(1,rmax);
        return result;
    }

    static void testFindLCA() {
        int[] in = {1,2,3,4,5,6,8,9};
        int[] pre = {6,2,1,4,3,5,8,9};
        MyBinTree<Integer> root = buildBinTreeInPre(pre, in, 0, pre.length - 1);
        findLCA(root,1,5);
    }
    static void findLCA(MyBinTree<Integer> root, int v1, int v2) {
        if (root == null) {
            System.out.println("not found");
            return;
        }

        if (v1 == root.element) {
            System.out.println(v1);
            return;
        }
        if (v2 == root.element) {
            System.out.println(v2);
            return;
        }
        if (v1 < root.element && root.element < v2) {
            System.out.println(root.element);
            return;
        }
        if (v1 < root.element && v2 < root.element)
            findLCA(root.left, v1, v2);
        if (v1 > root.element && v2 > root.element)
            findLCA(root.right, v1, v2);
    }

    static void testPrintAllPath() {
        int[] in = {1,2,3,4,5,6,8,9};
        int[] pre = {6,2,1,4,3,5,8,9};
        MyBinTree<Integer> root = buildBinTreeInPre(pre, in, 0, pre.length - 1);
        ArrayList<Integer> path = new ArrayList<Integer>();
        printAllPath(root, path, 0);
    }
    static void printAllPath(MyBinTree<Integer> root, ArrayList<Integer> path, int len) {
        if (root != null) {
            path.add(len, root.element);
            len++;
        }
        else
            return;
        if (root.left == null && root.right == null) {
            int i = 0;
            for (int e : path) {
                System.out.print(e + " ");
                i++;
                if (i == len)
                    break;
            }
            System.out.println();
        } else {
            printAllPath(root.left, path, len);
            printAllPath(root.right, path, len);
        }
    }

    static void testBuildBinTreeInPre() {
        int[] in = {1,2,3,4,5,6,8,9};
        int[] pre = {6,2,1,4,3,5,8,9};
        MyBinTree<Integer> root = buildBinTreeInPre(pre, in, 0, pre.length - 1);
        root.levelPrint();
    }
    static MyBinTree<Integer> buildBinTreeInPre(int[] pre, int[] in, int start, int end) {
        if (pre == null)
            return null;

        MyBinTree<Integer> root = new MyBinTree<Integer> (pre[start]);
        if (start == end) {
            return root;
        } else if (start > end) {
            return null;
        } else {
            int mid = findMid(pre, in, start, end);
            if (mid == -1)
                return null;
            MyBinTree<Integer> left = buildBinTreeInPre(pre, in, start + 1, mid);
            MyBinTree<Integer> right = buildBinTreeInPre(pre, in, mid + 1, end);
            root.left = left;
            root.right = right;
            return root;
        }
    }
    static int findMid(int[] pre, int[] in, int start, int end) {
        if (start >= end)
            return start;
        int idx = 0;
        for (int i = 0; i < in.length; i++) {
            if (in[i] == pre[start]) {
                idx = i;
                break;
            }
        }
        if (idx - 1 < 0)
            return -1;
        for (int i = start + 1; i <= end; i++) {
            if (pre[i] == in[idx - 1])
                return i;
        }
        return start;
    }

    static void testMirror() {
        MyBinTree<Integer> root = new MyBinTree<Integer>(4);
        root.bstPush(2);
        root.bstPush(5);
        root.bstPush(1);
        root.bstPush(3);

        root.levelPrint();
        System.out.println();
        root.mirror();
        root.levelPrint();
    }

    static void testMaxDepth() {
        MyBinTree<Integer> root = new MyBinTree<Integer>(4);
        root.bstPush(2);
        root.bstPush(5);
        root.bstPush(1);
        root.bstPush(3);
        System.out.println(root.maxDepth());
    }

    static void testEquals() {
        MyBinTree<Integer> t1 = new MyBinTree<Integer>(1);
        t1.bstPush(2);
        t1.bstPush(3);
        t1.bstPush(4);
        t1.bstPush(5);

        MyBinTree<Integer> t2 = new MyBinTree<Integer>(1);
        t2.bstPush(2);
        t2.bstPush(3);
        t2.bstPush(4);
        t2.bstPush(5);

        System.out.println(t1.equals(t2));
    }

    static void testGetSize() {
        MyBinTree<Integer> root = new MyBinTree<Integer>(1);
        root.bstPush(2);
        root.bstPush(3);
        root.bstPush(4);
        root.bstPush(5);
        System.out.println(root.getSize());
    }

    static void testMyBinTree() {
        MyBinTree<Integer> root = new MyBinTree<Integer>(2);
        root.bstPush(1);
        root.bstPush(3);
        root.bstPush(8);
        root.bstPush(6);
        root.binPrint();
    }
}
