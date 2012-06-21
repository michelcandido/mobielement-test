import java.util.Arrays;
import java.util.HashMap;


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
        testFindDuplicates();
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

    static void printIntArray(int[] src) {
        for (int i:src)
            System.out.format("%d ", i);
        System.out.println();
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



}
