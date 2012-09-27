import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;


public class CC {

    /**
     * @param args
     */
    public static void main(String[] args) {
        // TODO Auto-generated method stub
        //testLongestPalindrom();
        testLongestConsecutiveRandomSequence();
    }

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
