import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.io.StringReader;
import java.util.ArrayDeque;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Scanner;


public class GArray {
    /**
     * @param args
     */
    public static void main(String[] args) {
        //testFindMax();
        //testCheckPair();
        //testFindMajority();
        //testMergeMN();
        //testGetMedian();
        //testReverseArray();
        //testRotateArray();
        //testMaxSumNoAdjacent();
        //testLeadersInArray();
        //testSumToZero();
        //testFindManMin();
        //testSeparate0N1();
        //testMaxDiff();
        //testUnionAndIntersection();
        //testSegregateEvenOdd();
        //testFindDuplicates();
        //testFindEquilibrium();
        //testSearchinMatrix();
        //testNextGreater();
        //testAreConsecutive();
        //testFindFirstMissing();
        //testCountOccurrences();
        //testFindMaximumJI();
        //testFindMaximumInSubarrays();
        //testMinDist();
    	testFindRepeatAndMiss();
    }
    
    static void testFindRepeatAndMiss() {
    	int[] a = {4, 3, 6, 2, 1, 1};
    	findRepeatAndMiss(a);
    }
    static void findRepeatAndMiss(int[] a) {
        for (int i = 0; i < a.length; i++) {
            if (a[Math.abs(a[i]) - 1] > 0) {
                a[Math.abs(a[i]) - 1] = -a[Math.abs(a[i]) - 1];
            } else {
                System.out.println(Math.abs(a[i]));
            }
        }
        for (int i = 0; i < a.length; i++) {
            if (a[i] > 0)
                System.out.println(i+1);
        }
    }

    static void testMinDist() {
        int[] a =   {2, 5, 3, 5, 4, 4, 2, 3};
        minDist(a,3,2);
    }
    static void minDist(int[] a, int x, int y) {
        int i = 0, prev = 0, min = Integer.MAX_VALUE;
        while (a[i] != x && a[i] != y && i < a.length)
            i++;
        prev = i;
        while (i < a.length) {
            if (a[i] == x || a[i] == y) {
                if (a[i] != a[prev] && (i - prev) < min) {
                    min = i - prev;
                    prev = i;
                } else {
                    prev = i;
                }
            }

            i++;
        }
        System.out.println(min);
    }

    static void testFindMaximumInSubarrays() {
        int[] a =   {8, 5, 10, 7, 9, 4, 15, 12, 90, 13};
        findMaximumInSubarrays(a,4);
    }
    static void findMaximumInSubarrays(int[] a, int k) {
        int i = 0;
        ArrayDeque<Integer> Q = new ArrayDeque<Integer>();
        while (i < k) {
            while (!Q.isEmpty() && a[i] >= a[Q.getLast()])
                Q.removeLast();
            Q.addLast(i);
            i++;
        }
        while (i < a.length) {
            System.out.println(a[Q.getFirst()]);
            while (!Q.isEmpty() && a[i] >= a[Q.getLast()])
                Q.removeLast();
            while (!Q.isEmpty() && Q.getFirst() <= (i - k))
                Q.removeFirst();
            Q.addLast(i);
            i++;
        }
        System.out.println(a[Q.getFirst()]);
    }

    static void testFindMaximumJI() {
        int[] a =  {6, 5, 4, 3, 2, 1};
        findMaximumJI(a);
    }
    static void findMaximumJI(int[] a) {
        int[] s = new int[a.length];
        for (int i = 0; i < a.length; i++) {
            s[i] = 0;
            for (int j = 0; j < i; j++) {
                if (a[i] > a[j]) {
                    if (s[i] < (i - j))
                        s[i] = i - j;
                }
            }
        }
        int OPT = 0;
        for (int k = 0; k < a.length; k++) {
            System.out.println(s[k]);
            if (s[k] > OPT)
                OPT = s[k];
        }
        System.out.println("---------------   "+OPT+"   ----------------------");
    }

    static void testCountOccurrences() {
        int a[] = {1, 1, 2, 2, 2, 2, 3};
        countOccurrences(a,4);
    }
    static void countOccurrences(int[] a, int x) {
        int first = first(a,0,a.length-1, x);
        int last = last(a,0,a.length-1,x);
        if (first == -1 || last == -1)
            System.out.println("cannot find");
        else {
            System.out.format("count:%d, first:%d, last:%d", last - first + 1, first, last);
        }
    }
    static int first(int[] a, int start, int end, int x) {
        if (start > a.length -1 || end < 0 || start > end)
            return -1;
        int mid = (start + end) / 2;
        if (a[mid] == x) {
            if (mid > start) {
                int first = first(a, start, mid - 1, x);
                if (first != -1)
                    return first;
                else
                    return mid;
            } else {
                return mid;
            }
        } else if (x > a[mid]){
            return  first (a, mid + 1, end, x);
        } else {
            return first(a, start, mid - 1, x);
        }
    }
    static int last(int[] a, int start, int end, int x) {
        if (start > a.length -1 || end < 0 || start > end)
            return -1;
        int mid = (start + end) / 2;
        if (a[mid] == x) {
            if (end > mid) {
                int last = last(a, mid + 1, end, x);
                if (last != -1)
                    return last;
                else
                    return mid;
            } else {
                return mid;
            }
        } else if (x > a[mid]) {
            return last(a, mid + 1, end, x);
        } else {
            return last(a, start, mid - 1, x);
        }
    }

    static void testFindFirstMissing() {
        int[] a = {0, 1, 2, 3};
        System.out.println(findFirstMissing(a,0,a.length - 1));
    }
    static int findFirstMissing(int[] a, int start, int end) {
        if (start > end)
            return end + 1;

        if (start != a[start])
            return start;

        int mid = (start + end) / 2;
        if (a[mid] > mid)
            return findFirstMissing(a, start, mid);
        else
            return findFirstMissing(a, mid + 1, end);
    }

    static void testAreConsecutive() {
        int[] a =  {83, 78, 80, 81, 79, 82};
        areConsecutive(a);
    }
    static void areConsecutive(int[] a) {
        int sum = 0;
        int min =  a[0], max = a[0];
        for (int i = 0; i < a.length; i++) {
            if (a[i] < min)
                min = a[i];
            else if (a[i] > max)
                max = a[i];
            sum += a[i];
        }
        int result = (min + max) * (max - min + 1) / 2;
        if (result == sum)
            System.out.println("yes");
        else
            System.out.println("no");
    }

    static void testNextGreater() {
        int[] a = {1, 4, 2, 9, 7, 13, 3, 8};
        nextGreater(a);
    }
    static void nextGreater(int[] a) {
        int[] g = new int[a.length];
        int t = -1;
        for (int i = a.length - 1; i >=0; i--) {
            if (i == a.length - 1) {
                g[i] = -1;
                t = -1;
            } else {
                if (a[i + 1] > a[i]) {
                    g[i] = a[i + 1];
                    t =  g[i];
                } else if (a[i] < t) {
                    g[i] = t;
                } else {
                    g[i] = -1;
                    t = -1;
                }
            }
        }
        printIntArray(g);
    }

    static void testSearchinMatrix() {
        int a[][] = null;
        try {
            a = readIntMatrix();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        searchinMatrix(a,10);
    }
    static void searchinMatrix(int a[][], int x) {
        int n = a.length;
        int r = 0, c = n - 1;
        while (r < n && c >= 0) {
            if (a[r][c] == x) {
                System.out.format("found at row %d and column %d\n", r, c);
                return;
            }
            if (x > a[r][c])
                r++;
            else
                c--;
        }
        System.out.println("cannot find");

    }

    static void testFindEquilibrium() {
        int a[] = {-7,1,5,2,-4,3,0};
        findEquilibrium(a);
    }
    static void findEquilibrium(int a[]) {
        int leftsum = 0, rightsum = 0;
        for (int i: a) {
            rightsum += i;
        }
        for (int i =0;i<a.length;i++) {
            rightsum = rightsum-a[i];
            if (rightsum==leftsum)
                System.out.println(i);
            leftsum+=a[i];
        }
    }

    static void testFindDuplicates() {
        int a[] = {1, 2, 3, 1, 3, 0, 6};
        findDuplicates(a);
    }
    static void findDuplicates(int a[]) {
        for (int i = 0; i < a.length; i++) {
            if (a[Math.abs(a[i])] >= 0)
                a[Math.abs(a[i])] *= -1;
            else
                System.out.println(Math.abs(a[i]));
        }
    }

    static void testSegregateEvenOdd() {
        int a[] = {1,12, 34, 45, 9, 8, 90, 3};
        printIntArray(a);
        segregateEvenOdd(a);
        printIntArray(a);
    }

    static void segregateEvenOdd(int a[]) {
        int left = 0, right = a.length - 1;
        while (left < right) {
            while (a[left]%2 == 0 && left < right)
                left++;
            while (a[right]%2 != 0 && left < right)
                right--;
            if (left < right) {
                int t = a[left];
                a[left] = a[right];
                a[right] = t;
                left++;
                right--;
            }
        }
    }

    static void testUnionAndIntersection() {
        int a1[] = {1, 3, 4, 5, 7};
        int a2[] = {2, 3, 5, 6};
        unionAndIntersection(a1,a2);
    }
    static void unionAndIntersection(int a1[], int a2[]) {
        int i = 0, j = 0;
        while (i < a1.length && j < a2.length) {
            if (a1[i] < a2[j]) {
                System.out.print(a1[i]+" ");
                i++;
            } else if (a1[i] > a2[j]) {
                System.out.print(a2[j]+" ");
                j++;
            } else {
                System.out.print(a1[i]+" ");
                i++;
                j++;
            }
        }
        while (i < a1.length) {
            System.out.print(a1[i]+" ");
            i++;
        }
        while (j < a2.length) {
            System.out.print(a2[j]+" ");
            j++;
        }
        System.out.println();
        i = 0;j = 0;
        while (i < a1.length && j < a2.length) {
            if (a1[i] < a2[j]) {
                i++;
            } else if (a1[i] > a2[j]) {
                j++;
            } else {
                System.out.print(a1[i]+" ");
                i++;
                j++;
            }
        }
        System.out.println();
    }

    static void testMaxDiff() {
        int a[] = {7,9,1,10};//{7, 9, 5, 6, 3, 2};//{2, 3, 10, 6, 4, 8, 1};
        System.out.println(""+maxDiff(a));
    }
    static int maxDiff(int a[]) {
        int s[] = new int[a.length];
        int min;
        s[0] = a[0];
        s[1] = a[1] - a[0];
        min = Math.min(a[1], a[0]);
        for (int i = 2; i < a.length; i++) {
            if (a[i] < min || (a[i] - min) < s[i - 1]) {
                s[i] = s[i - 1];
                min = Math.min(a[i], min);
            } else {
                s[i] = a[i] - min;
            }
        }
        return s[a.length - 1];
    }

    static void testSeparate0N1() {
        int a[] = {0, 1, 0, 1, 0, 0, 1, 1, 1, 0};
        separate0N1_2(a);
    }
    static void separate0N1_2(int a[]) {
        int lo = 0, hi = a.length - 1;
        while (lo <= hi) {
            if (a[lo] == 0)
                lo++;
            else {
                int t = a[hi];
                a[hi] = a[lo];
                a[lo] = t;
                hi--;
            }
        }
        for (int i = 0; i <= a.length - 1; i++) {
            System.out.print(a[i] + " ");
        }
        System.out.println();
    }
    static void separate0N1(int a[]) {
        int first = -1,t;
        for (int i = 0; i <= a.length - 1; i++) {
            if (a[i] == 0 && first >= 0) {
                t = a[i];
                a[i] = a[first];
                a[first] = t;
                first++;
            } else if (a[i] == 1 && first < 0) {
                first = i;
            }
        }
        for (int i = 0; i <= a.length - 1; i++) {
            System.out.print(a[i] + " ");
        }
        System.out.println();
    }

    static void testFindManMin() {
        int a[] = {8,1,7,4,9,2,5};
        findMaxMin(a);
    }
    static void findMaxMin(int a[]) {
        int avg = 0;
        for (int i:a) {
            avg += i;
        }
        avg = avg / a.length;
        int max = Integer.MIN_VALUE, min = Integer.MAX_VALUE, idxMax = 0, idxMin = 0, v;
        for (int i = 0; i < a.length; i++) {
            v = a[i] - avg;
            if (v > max) {
                max = v;
                idxMax = i;
            } else if (v < min) {
                min = v;
                idxMin = i;
            }
        }
        System.out.println("max:"+a[idxMax]+"\n"+"min:"+a[idxMin]);
    }

    static void testSumToZero() {
        int a[] = {1, 60, -10, 70, -80, 85};
        System.out.println(""+sumToZero(a));
    }
    static int sumToZero(int a[]) {
        Arrays.sort(a);
        int l = 0, r = a.length - 1;
        int min = Math.abs(a[l] + a[r]);
        int sum;
        while (l < r) {
            sum = a[l] + a[r];
            if (sum < 0)
                l++;
            else if (sum > 0)
                r--;
            else return 0;
            if (Math.abs(sum) < min)
                min = Math.abs(sum);
        }
        return min;
    }

    static void testLeadersInArray() {
        int a[] = {16, 17, 4, 3, 5, 2};
        leadersInArray(a);
    }
    static void leadersInArray(int a[]) {
        if (a == null)
            return;
        int max = a[a.length-1];
        System.out.print(max+" ");
        for (int i=a.length-2;i>=0;i--) {
            if (a[i] > max) {
                max = a[i];
                System.out.print(max+" ");

            }
        }
    }

    static void testMaxSumNoAdjacent() {
        int a[] = {5,  5, 10, 40, 50, 35};
        System.out.println(""+maxSumNoAdjacent(a));
    }
    static int maxSumNoAdjacent(int a[]) {
        if (a == null || a.length == 0)
            return 0;
        if (a.length == 1)
            return a[0];
        if (a.length == 2)
            return Math.max(a[0], a[1]);
        int s[] = new int[a.length];
        s[0] = a[0];
        s[1] = Math.max(a[0], a[1]);
        for (int i = 2; i < a.length; i++) {
            s[i] = Math.max(s[i-2]+a[i], s[i-1]);
        }
        return s[a.length - 1];
    }

    static void testRotateArray() {
        int a[] = {1, 2, 3, 4, 5, 6, 7};
        printIntArray(a);
        rotateArray(a,3);
        printIntArray(a);
    }

    static void rotateArray(int a[], int n) {
        reverseArray(a);
        int a1[] = new int[a.length - n];
        int a2[] = new int[n];
        System.arraycopy(a, 0, a1, 0, a1.length);
        System.arraycopy(a, a.length - n, a2, 0, a2.length);
        reverseArray(a1);
        reverseArray(a2);
        System.arraycopy(a1, 0, a, 0, a1.length);
        System.arraycopy(a2, 0, a, a1.length, a2.length);
    }

    static void testReverseArray() {
        int a[] = {1, 2, 3, 4, 5, 6, 7};
        printIntArray(a);
        reverseArray(a);
        printIntArray(a);
    }

    static void reverseArray(int a[]) {
        int start = 0, end = a.length - 1;
        while (start < end) {
            int t = a[start];
            a[start] = a[end];
            a[end] = t;
            start++;
            end--;
        }
    }

    static void testGetMedian() {
        int a1[] = {1, 12, 15, 26, 38};
        int a2[] = {2, 13, 17, 30, 45};
        System.out.println(""+getMedian(a1, a2, a1.length));
    }

    static int getMedian(int a1[], int a2[], int n) {
        if (n <= 0)
            return -1;
        if (n == 1)
            return (a1[0] + a2[0]) / 2;
        if (n == 2)
            return (Math.max(a1[0], a2[0]) + Math.min(a1[1], a2[1])) / 2;

        int m1 = median(a1, n);
        int m2 = median(a2, n);

        if (m1 == m2)
            return m1;

        int newa1[] = new int[n - n/2];
        int newa2[] = new int[n - n/2];
        if (m1 < m2) {
            System.arraycopy(a1, n/2, newa1, 0, n - n/2);
            System.arraycopy(a2, 0, newa2, 0, n - n/2);
            return getMedian(newa1, newa2, n - n/2);
        } else {
            System.arraycopy(a1, 0, newa1, 0, n - n/2);
            System.arraycopy(a2, n/2, newa2, 0, n - n/2);
            return getMedian(newa1, newa2, n - n/2);
        }
    }

    static int median(int a[], int n) {
        if (n%2 == 0)
            return (a[n/2]+a[n/2-1])/2;
        else
            return a[n/2];
    }



    static int findMaxLinear(int src[]) {
        if (src == null || src.length == 0)
            return Integer.MIN_VALUE;
        int max = src[0];
        for (int i:src) {
            if (i > max)
                max = i;
        }
        return max;
    }

    static int findMaxBinary(int src[], int low, int high) {
        if (src == null || src.length == 0)
            return Integer.MIN_VALUE;
        if (low == high)
            return src[low];
        else if ((high == low + 1) && (src[low] >= src[high]))
            return src[low];
        else if ((high == low + 1) && (src[low] < src[high]))
            return src[high];
        int mid = low + (high - low) / 2;
        if (src[mid] > src[mid + 1] && src[mid] > src[mid - 1])
            return src[mid];
        if (src[mid] > src[mid + 1] && src[mid] < src[mid - 1])
             return findMaxBinary(src, low, mid-1);
        else
            return findMaxBinary(src, mid+1, high);
    }

    static void testFindMax() {
        int arr1[] = {8, 10, 20, 80, 100, 200, 400, 500, 3, 2, 1};
        int arr2[] = {1, 3, 50, 10, 9, 7, 6};
        int arr3[] = {10, 20, 30, 40, 50};
        int arr4[] = {120, 100, 80, 20, 0};
        System.out.println(findMaxLinear(arr1));
        System.out.println(findMaxLinear(arr2));
        System.out.println(findMaxLinear(arr3));
        System.out.println(findMaxLinear(arr4));
        System.out.println(findMaxBinary(arr1,0,arr1.length - 1));
        System.out.println(findMaxBinary(arr2,0,arr2.length - 1));
        System.out.println(findMaxBinary(arr3,0,arr3.length - 1));
        System.out.println(findMaxBinary(arr4,0,arr4.length - 1));
    }

    static boolean checkPair(int a[], int x) {
        if (a == null || a.length == 0 || a.length == 1)
            return false;
        HashMap<Integer, Integer> hash = new HashMap<Integer, Integer>();
        for (int i : a) {
            if (!hash.containsKey(i)) {
                hash.put(i, 1);
            } else {
                int value = hash.get(i);
                value++;
                hash.put(i, value);
            }
        }
        for (int i : a) {
            int result = x - i;
            if (result != i) {
                if (hash.containsKey(result))
                    return true;
                else
                    continue;
            } else {
                int count = hash.get(i);
                if (count >= 2)
                    return true;
                else
                    continue;
            }
        }
        return false;
    }

    static void testCheckPair() {
        int a[] = {3,0,1};
        System.out.println(checkPair(a, 2));
    }

    static int findMajority(int a[]) {
        int result = Integer.MIN_VALUE;
        if (a == null)
            return result;
        int valve = a.length / 2;
        HashMap<Integer, Integer> hash = new HashMap<Integer, Integer>();
        for (int i : a) {
            int count = 0;
            if (hash.containsKey(i)) {
                count = hash.get(i);
                count++;
                if (count > valve)
                    return i;
                else
                    hash.put(i, count);
            } else {
                hash.put(i, 1);
            }
        }
        return result;
    }

    static int findMajority2(int a[]) {
        if (a == null)
            return Integer.MIN_VALUE;
        int majorIdx = 0, count = 1;
        for (int i = 1; i < a.length; i++) {
            if (a[majorIdx] == a[i])
                count++;
            else {
                count--;
            }
            if (count == 0) {
                majorIdx = i;
                count = 1;
            }
        }
        int valve = a.length / 2;
        count = 0;
       for (int i : a) {
           if (i == a[majorIdx])
               count++;
       }
        if (count > valve)
            return a[majorIdx];
        else
            return Integer.MIN_VALUE;
    }

    static void testFindMajority() {
        int a[] = {3, 3, 4, 2, 4, 4, 2, 4,4};
        //int result = findMajority(a);
        int result = findMajority2(a);
        if (result == Integer.MIN_VALUE)
            System.out.println("None");
        else
            System.out.format("%d\n",result);
    }

    static void mergeMN(int[] mn, int n[]) {
        int idx = mn.length - 1;
        for (int i = mn.length - 1; i >= 0; i--) {
            if (mn[i] != -1) {
                mn[idx] = mn[i];
                idx--;
            }
        }
        for (int i = idx; i >= 0; i--)
            mn[i] = -1;
        if (idx < mn.length - 1)
            idx++;
        int idxn = 0, idxmn = 0;

        while (idxn <= n.length - 1 && idx <= mn.length - 1) {
            if (n[idxn] < mn[idx]) {
                mn[idxmn] = n[idxn];
                idxmn++;
                idxn++;
            } else {
                mn[idxmn] = mn[idx];
                idxmn++;
                idx++;
            }
        }

        if (idx > mn.length - 1) {
            while (idxn <= n.length - 1) {
                mn[idxmn] = n[idxn];
                idxmn++;
                idxn++;
            }
        }

    }

    static void testMergeMN() {
        int mn[] = {2, -1, 7, -1, -1, 10, -1};
        int n[] = {5,8,12,14};
        printIntArray(mn);
        mergeMN(mn, n);
        printIntArray(mn);
    }

    static void printIntArray(int[] src) {
        for (int i:src)
            System.out.format("%d ", i);
        System.out.println();
    }

    static int[] readIntArray() throws IOException {
        int[] src = null;
        Scanner sc = null;
        BufferedReader br = null;
        StringReader sr = null;

        try {
            br = new BufferedReader(new FileReader("testdata"));
            String l = br.readLine();
            if (l != null) {
                sr = new StringReader(l);
                sc = new Scanner(sr);
                ArrayList<Integer> arr = new ArrayList<Integer>();

                while (sc.hasNext()) {
                    arr.add(sc.nextInt());
                }
                src = new int[arr.size()];
                for (int i = 0; i < arr.size(); i++)
                    src[i] = arr.get(i).intValue();
            }
        } catch (FileNotFoundException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } finally {
            if (sr != null)
                sr.close();
            if (br != null)
                br.close();
            if (sc != null)
                sc.close();
        }
        return src;
    }

    static int[][] readIntMatrix() throws IOException{
        int[][] src = null;
        Scanner sc = null;
        BufferedReader br = null;
        StringReader sr = null;

        try {
            br = new BufferedReader(new FileReader("testdata"));
            ArrayList<String> rows = new ArrayList<String>();
            String l = null;
            while ((l=br.readLine()) != null)
                rows.add(l);
            ArrayList<ArrayList<Integer>> values = new ArrayList<ArrayList<Integer>>();
            for (int i = 0; i < rows.size(); i++) {
                sr = new StringReader(rows.get(i));
                sc = new Scanner(sr);
                ArrayList<Integer> arr = new ArrayList<Integer>();
                while (sc.hasNext()) {
                    arr.add(sc.nextInt());
                }
                values.add(arr);
            }
            src = new int[values.size()][];
            for (int i = 0; i < values.size(); i++) {
                src[i] = new int[values.get(i).size()];
                for (int j = 0; j < values.get(i).size(); j++) {
                    src[i][j] = values.get(i).get(j).intValue();
                }
            }

        } catch (FileNotFoundException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } finally {
            if (sr != null)
                sr.close();
            if (br != null)
                br.close();
            if (sc != null)
                sc.close();
        }
        return src;
    }

}
