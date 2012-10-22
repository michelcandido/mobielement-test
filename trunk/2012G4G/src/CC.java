import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map.Entry;
import java.util.PriorityQueue;
import java.util.TreeMap;


public class CC {


    /**
     * @param args
     */
    public static void main(String[] args) {
        // TODO Auto-generated method stub
        //testLongestPalindrom();
        //testLongestConsecutiveRandomSequence();
        //testLongestIncreasingSubsequence();
        //testPushAllZero();
        //testFindKthInMatrix();
        //testIsInterleaved();
        //testFineOverMth();
        //testFindLowestPositiveInteger();
        //testDeleteDuplicatesInLinkedList();
        //testFindMaximumInSubArray();
        //testFindLargestSum();
    	//testFindMax4ji();
    	testFindValueInBST();
    }

    //http://www.careercup.com/question?id=2971
    static void testFindValueInBST() {    	
    	int[] in = {1,2,3,4,6,8,9,10};
        int[] pre = {6,2,1,3,4,9,8,10};
        MyBinTree<Integer> root = GTree.buildBinTreeInPre(pre, in, 0, pre.length - 1);
        root.binPrint();
        System.out.println();
        ArrayList<Integer> list = new ArrayList<Integer>();
        findValueInBST2(root, 15, list, 0);
    }
    static void findValueInBST2(MyBinTree<Integer> tree, int value, ArrayList<Integer> list, int level) {
    	if (tree == null)
    		return;
    	list.add(level, tree.element);
    	int sum = 0;
    	for (int i = level; i >=0; i--) {
    		sum += list.get(i);
    		if (sum == value) {
    			for (int j = i; j <= level; j++)
    				System.out.print(list.get(j) + " ");
    			System.out.println();
    		}
    	}
    	findValueInBST2(tree.left, value, list, level + 1);
    	findValueInBST2(tree.right, value, list, level + 1);
    }
    static void findValueInBST(MyBinTree<Integer> tree, int value, ArrayList<Integer> list, int level) {
    	if (tree == null)
    		return;
    	list.add(level, tree.element);
    	if (tree.element == value) {
    		for (int i = 0; i <= level; i++)
    			System.out.print(list.get(i)+" ");
    		System.out.println();
    	}
    	findValueInBST(tree.left, value - tree.element, list, level + 1);
    	findValueInBST(tree.right, value - tree.element, list, level + 1);
    }
    
    //http://www.careercup.com/question?id=12705676
    static void testFindMax4ji()
    {
    	//int[] a = {7,5,4,3,1,6};
    	//int[] a = {2,1,4,100,-5};
    	int[] a = {5,15,3,10,20,1,19,0,8,16};
    	findMax4ji(a);
    	System.out.println();
    	MaxDiff(a);
    }

	static void MaxDiff(int[] a) {
		int min = a[0]; // assume first element as minimum
		int maxdiff = 0;
		int posi = -1, posj = -1, minpos = 0;

		for (int i = 1; i < a.length; i++) {
			if (a[i] < min) {
				min = a[i];
				minpos = i;
			} else {
				int diff = a[i] - min;
				if (diff > maxdiff) {
					maxdiff = diff;
					posi = minpos;
					posj = i;
				}
			}
		}
		System.out.format("max:%d, mini:%d, maxj:%d", maxdiff,a[posi], a[posj]);
	}
    static void findMax4ji(int[] a) {    	
    	int i = 0, j = a.length - 1;
    	int max = a[j] - a[i];
    	int mini = a[i], maxj = a[j];
    	i++;    	
    	while (i <= j) {    		
    		while (a[i] < mini) {
    			mini = a[i];
    			max = maxj - mini;
    			i++;
    		}
    		j--;
    		while (a[j] > maxj && i <= j) {    		
    			maxj = a[j];
    			max = maxj - mini;
    			j--;
    		}
    		i++;    		
    	}
    	System.out.format("max:%d, mini:%d, maxj:%d", max,mini,maxj);
    }
    
    //http://www.careercup.com/question?id=1777
    static void testFindLargestSum() {
        int[] a = { 1, 3,-4, 5 -2, -1, 3 , 3, -1};
        findLargestSum(a);
    }
    static void findLargestSum(int[] a) {
        int maxSum = 0, maxStart = 0, maxEnd = 0, curStart = 0, curEnd = 0, curSum = 0;
        for (int i =  0; i < a.length; i++) {
            curSum += a[i];
            if (curSum < 0) {
                curStart = i+1;
                curEnd = curStart;
                curSum = 0;
            } else {
                if (curSum > maxSum) {
                    maxSum = curSum;
                    maxStart = curStart;
                    maxEnd = curEnd;
                }
                curEnd++;
            }
        }
        for (int i = maxStart; i <= maxEnd; i++)
            System.out.println(a[i]);
    }
    //http://www.careercup.com/question?id=7760665
    static void testFindMaximumInSubArray() {
        int[] a = {1, 2, 3, 1, 4, 5, 2, 3, 6};
        findMaximumInSubArray(a, 3);
    }
    static void findMaximumInSubArray(int[] a, int k) {
        int[] s = new int[a.length];
        s[0] = a[0];
        for (int i = 1; i < a.length; i++) {
            s[i] = a[i];
            for (int j = (i - k + 1) > 0? i - k + 1:0;  j < i ; j++) {
                if (s[j] > s[i])
                    s[i] = s[j];
                else {
                    s[j] = s[i];
                }
            }
        }
        for (int i : s)
            System.out.println(i);
    }

    //http://www.careercup.com/question?id=8407365
    static void testDeleteDuplicatesInLinkedList() {
        Integer[] a = {1,2,2,2,2,2,2,3};
        deleteDuplicatesInLinkedList(a);
    }
    static void deleteDuplicatesInLinkedList(Integer[] a) {
        MyLinkedList<Integer> list = new MyLinkedList<Integer>(a);
        list.print();
        MyLinkedList<Integer>.Node<Integer> parent = list.head;
        MyLinkedList<Integer>.Node<Integer> node = list.head.next;
        boolean deleteCurrent = false;
        while (node != null) {
            if (node.element == parent.element) {
                parent.next = node.next;
                node = node.next;
                deleteCurrent = true;
            } else {
                if (deleteCurrent) {
                    parent.element = node.element;
                    parent.next = node.next;
                    node = node.next;
                    deleteCurrent = false;
                } else {
                    parent = node;
                    node = node.next;
                }
            }
        }
        list.print();
    }

    //http://www.careercup.com/question?id=8407365
    static void testFindLowestPositiveInteger() {
        int[] a = {1,2,3,5,7,9};
        findLowestPositiveInteger(a);
    }
    static void findLowestPositiveInteger(int[] a) {
        int i = 0, j = a.length - 1;
        while (i < j) {
            while (i < a.length && a[i] > 0)
                i++;
            while (j >=0 && a[j] <= 0)
                j--;
            if (i < j) {
                int t = a[i];
                a[i] = a[j];
                a[j] = t;
                i++;
                j--;
            }
        }
        int n = --i;
        i = 0; j = n;
        while (i < j) {
            while (i <= n && a[i] <= n)
                i++;
            while (j >= 0 && a[j] > n)
                j--;
            if (i < j) {
                int t = a[i];
                a[i] = a[j];
                a[j] = t;
                i++;
                j--;
            }
        }
        n = --i;
        for (i = 0; i <= n; i++) {
            if (a[i] < 0)
                continue;
            int t = a[a[i] - 1];
            a[a[i] - 1] = a[i];
            a[i] = t;
        }
        for (i = 0; i < a.length; i++) {
            if (i != a[i] - 1) {
                System.out.println(i + 1);
                return;
            }
        }
        System.out.println(i);
    }

    //http://www.careercup.com/question?id=11879708
    static void testFineOverMth() {
        int[] a = {4, 3, 3, 2, 1, 2, 3, 4, 4, 7};
        findOverMth(a,5);
    }
    static void findOverMth(int[] a, int m) {
        TreeMap<Integer, Integer> map = new TreeMap<Integer, Integer>();
        for (int i : a) {
            if (map.containsKey(i)) {
                map.put(i, map.get(i)+1);
            } else {
                if (map.size() != m - 1) {
                    map.put(i,1);
                } else {
                    Entry<Integer, Integer> e;
                    Iterator<Entry<Integer, Integer>> iter = map.entrySet().iterator();
                    while (iter.hasNext()) {
                        e = iter.next();
                        int count = e.getValue();
                        count--;
                        if (count == 0)
                            iter.remove();
                        else
                            e.setValue(count);
                    }
                }
            }
        }
        ArrayList<Integer> result = new ArrayList<Integer>();
        Iterator<Entry<Integer, Integer>> iter = map.entrySet().iterator();
        Entry<Integer, Integer> e;
        int minCount = (int) Math.ceil(((double) a.length) / m);
        while (iter.hasNext()) {
            int count = 0;
            e = iter.next();
            for (int i : a) {
                if (i == e.getKey())
                    count++;
            }
            if (count >= minCount)
                result.add(e.getKey());
        }
        for (int i : result)
            System.out.println(i);
    }

    //http://www.careercup.com/question?id=14539805
    static void testIsInterleaved() {
        String a = "bac", b="ecd", c="bacadc";
        System.out.println(isInterleaved(a,b,c));
    }
    static boolean isInterleaved(String a, String b, String c) {
        if (a == null && b == null && c == null)
            return true;
        if (a.length() + b.length() != c.length())
            return false;
        if (a.length() == 0 && b.length() == 0 && c.length() == 0)
            return true;

        if (a.length() == 0) {
            if(c.charAt(0) != b.charAt(0))
                return false;
            else
                return true;
        }
        if (b.length() == 0) {
            if(c.charAt(0) != a.charAt(0))
                return false;
            else
                return true;
        }

        if (c.charAt(0) != a.charAt(0) && c.charAt(0) != b.charAt(0))
            return false;


        if (c.charAt(0) == a.charAt(0) && c.charAt(0) != b.charAt(0))
            return isInterleaved(a.substring(1),b,c.substring(1));

        if (c.charAt(0) == b.charAt(0) && c.charAt(0) != a.charAt(0))
            return isInterleaved(a,b.substring(1),c.substring(1));

        return isInterleaved(a.substring(1),b,c.substring(1)) || isInterleaved(a,b.substring(1),c.substring(1));
    }

    //http://www.careercup.com/question?id=6335704
    static void testFindKthInMatrix() {
        int[][] a = {{1,6,9 },{3,7,10},{5,8,11}};
        findKthInMatrix(a,5);
    }
    static void findKthInMatrix(int[][] a, int k) {
        PriorityQueue<Integer> q = new PriorityQueue<Integer>();
        int n = a[0].length;
        for (int i = 0; i < n; i++) {
            for (int j = 0 ; j < n; j++) {
                q.offer(a[i][j]);
            }
        }
        System.out.println(q.toArray()[k - 1]);
    }

    //http://www.careercup.com/question?id=12986664
    static void testPushAllZero() {
        int[] a = {1, 2, 0 , 4, 0 , 0 , 8 , 6 , 7 , 0 , 3 , 0};//{0, 0, 1, 2, 0, 4, 0, 0 ,8 ,9};
        pushAllZero(a);
    }
    static void pushAllZero(int[] a) {
        int left = 0, right = a.length - 1;
        while (left < right) {
            while (a[left] != 0)
                left++;
            while (a[right] == 0)
                right--;
            a[left] = a[right] - a[left];
            a[right] = a[right] - a[left];
            a[left] = a[left] + a[right];
            left++;
            right--;
        }
        for (int i : a)
            System.out.println(i);
    }

    static void longestSameZeroOne(int[] a) {

    }

    static void testLongestIncreasingSubsequence() {
        int[] a = {0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15};
        longestIncreasingSubsequence(a);
    }
    static void longestIncreasingSubsequence(int[] a) {
        int[] s = new int[a.length], n = new int[a.length];
        s[0] = 1;
        n[0] = a[0];
        for (int i = 1; i < a.length; i++) {
            s[i] = 1;
            for (int j = 0; j < i; j++) {
                if (a[i] > n[j]) {
                    if (s[j] + 1 > s[i]) {
                        s[i] = s[j] + 1;
                        n[i] = a[i];
                    }
                }
            }
        }
        int max = 1;
        for (int i = 0; i < a.length; i++) {
            if (s[i] > max)
                max = s[i];
        }
        System.out.println(max);
    }

    //http://www.careercup.com/question?id=9783960
    static void testLongestConsecutiveRandomSequence() {
        int[] a = {101, 2, 3, 104, 5, 103, 9, 102};
        longestConsecutiveRandomSequence(a);
    }
    static void longestConsecutiveRandomSequence(int[] arr) {
        HashMap<Integer, Integer[]> list = new HashMap<Integer, Integer[]>();
        int max = 0, left=0, right=0;
        for (int i : arr) {
            if (list.containsKey(i))
                continue;
            Integer[] minmax = new Integer[2];
            if (!list.containsKey(i+1) && !list.containsKey(i-1)) {

                minmax[0] = i;
                minmax[1] = i;
                list.put(i, minmax);
            } else if (list.containsKey(i+1) && !list.containsKey(i-1)) {

                minmax[0] = i;
                minmax[1] = list.get(i+1)[1];
                list.put(i, minmax);
                list.put(minmax[1], minmax);
            } else if (!list.containsKey(i+1) && list.containsKey(i-1)) {

                minmax[1] = i;
                minmax[0] = list.get(i-1)[0];
                list.put(i, minmax);
                list.put(minmax[0], minmax);
            } else if (list.containsKey(i+1) && list.containsKey(i-1)) {

                minmax[0] = list.get(i-1)[0];
                minmax[1] = list.get(i+1)[1];
                list.put(minmax[0], minmax);
                list.put(minmax[1], minmax);
            }
            if (minmax[1] - minmax[0] > max) {
                left = minmax[0];
                right = minmax[1];
                max = minmax[1] - minmax[0];
            }
        }
        for (int i = left ; i <= right; i++)
            System.out.println(i);
    }

    //http://www.careercup.com/question?id=245679
    static void testLongestPalindrom() {
        String s = "abcdaeeadabb";
        longestPalindrom(s);
    }
    static void longestPalindrom(String ss) {
        StringBuilder sb = new StringBuilder();
        sb.append("$#");
        for (int i = 0; i < ss.length(); i++) {
            sb.append(ss.charAt(i));
            sb.append("#");
        }
        String s = sb.toString();
        int[] p = new int[s.length()];
        int id = 0;
        int max = 0;
        ///*
        for (int i  = 1; i < s.length(); i++) {
            p[i] = max > i? Math.min(p[2 * id - i], max - i) : 1;
            while (i+p[i] < s.length() && i - p[i] >= 0 && s.charAt(i + p[i]) == s.charAt(i - p[i]))
                p[i]++;
            if (i + p[i] > max) {
                max = i + p[i];
                id = i;
            }
        }
        max = 0;
        for (int i = 0; i < s.length(); i++)
        {
            if (p[i] > max) {
                max = p[i];
                id = i;
            }
        }
        //*/
        /*
        for (int i = 0; i < s.length(); i++) {
            p[i] = 1;
            int j = 1;
            while (i - j >= 0 && i + j < s.length() && s.charAt(i - j) == s.charAt(i + j)) {
                p[i]++;
                j++;
            }
            if (p[i] > max) {
                max = p[i];
                id = i;
            }
        }
        */
        p[id]--;
        for (int i = id - p[id]; i <= id + p[id]; i++) {
            if (s.charAt(i) != '#')
                System.out.print(s.charAt(i));
        }

    }

}
