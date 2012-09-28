import java.util.HashMap;


public class CC {

    /**
     * @param args
     */
    public static void main(String[] args) {
        // TODO Auto-generated method stub
        //testLongestPalindrom();
        //testLongestConsecutiveRandomSequence();
        //testLongestIncreasingSubsequence();
        testPushAllZero();
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
