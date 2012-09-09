import java.util.ArrayList;
import java.util.HashSet;
import java.util.Hashtable;
import java.util.PriorityQueue;


public class DP {

    /**
     * @param args
     */
    public static void main(String[] args) {
        //testCutRod();
        //testMinCoins();
        //testLCS();
        //testLND();
        //testZigZag();
        //testBadNeighbors();
        //testNumWays();
        testMaxCredit();
    }
    static void testMaxCredit() {
        int[] a = {1,2,3,4,5,6};//{5000,6500};//{100,200,300,1200,6000};//
        int[] b = {1,2,3,4,5,6,7};//{6000};//{};//
        int[] c = {1,2,3,4,5,6};//{6000};//{900,902,1200,4000,5000,6001};//
        int[] d = {0,1,2};//{6000};//{0,2000,6002};//
        int[] e = {1,2,3,4,5,6,7,8};//{0,5800,6000};//{1,2,3,4,5,6,7,8};//
        maxCredit(a,b,c,d,e);
    }
    static void maxCredit(int[] a, int[] b, int[] c, int[] d, int[] e) {
        PriorityQueue<Integer> times = new PriorityQueue<Integer>();
        Hashtable<Integer, ArrayList<String>> buttons = new Hashtable<Integer, ArrayList<String>>();
        initMaxCredit(a, "a", times, buttons);
        initMaxCredit(b, "b", times, buttons);
        initMaxCredit(c, "c", times, buttons);
        initMaxCredit(d, "d", times, buttons);
        initMaxCredit(e, "e", times, buttons);
        ArrayList<Integer> tlist = new ArrayList<Integer>();
        while (!times.isEmpty()) {
            tlist.add(times.poll());
        }
        int start = 0, end = 0;
        Hashtable<String, Integer> jset = new Hashtable<String, Integer>();
        for (;start < tlist.size() && end < tlist.size();) {
            if (tlist.get(end) - tlist.get(start) < 1000) {
                for (String s: buttons.get(tlist.get(end))) {
                    if (jset.containsKey(s)) {
                        int count = jset.get(s);
                        jset.put(s, ++count);
                    } else {
                        jset.put(s, 1);
                    }
                }

                if (jset.size() >= 3) {
                    System.out.println(tlist.get(start)+"-"+tlist.get(end));
                    jset.clear();
                    end++;
                    start = end;
                } else {
                    end++;
                }
            } else {
                while (tlist.get(end) - tlist.get(start) >= 1000) {
                    for (String s: buttons.get(tlist.get(start))) {
                        int count = jset.get(s);
                        count--;
                        if (count==0)
                            jset.remove(s);
                        else
                            jset.put(s, count);

                    }
                    start++;
                }
            }
            /*
            System.out.print(i+":");
            for (String s: buttons.get(i))
                System.out.print(s+" ");
            System.out.println();
            */
        }
    }
    static void initMaxCredit(int[] j, String s, PriorityQueue<Integer> times, Hashtable<Integer, ArrayList<String>> buttons) {
        for (int i:j) {
            if (!times.contains(i))
                times.offer(i);
            if (!buttons.containsKey(i))
                buttons.put(i, new ArrayList<String>());
            buttons.get(i).add(s);
        }
    }

    static void testNumWays() {
        //String[] bad = {"0001","6656"};
        //String[] bad = {"0001"};
        //String[] bad = {"0111","1011","1121","1112"};
        //String[] bad = {"0010","1222","1121"};
        String[] bad = {};
        System.out.println(""+numWays(1,1,bad));
    }
    static long numWays(int width, int height, String[] bad) {
        long s[][] = new long[width + 1][height + 1];

        for (int i = 0; i <= width; i++) {
            for (int j = 0; j <= height; j++) {
                s[i][j] = 0;
            }
        }

        if (!isBlocked(0,0,1,0,bad))
            s[1][0] = 1;
        if (!isBlocked(0,0,0,1,bad))
            s[0][1] = 1;

        for (int i = 1; i <= width; i++) {
            if (!isBlocked(i-1,0,i,0,bad) && s[i-1][0] != 0)
                s[i][0] = 1;
        }
        for (int i = 1; i <= height; i++) {
            if (!isBlocked(0,i-1,0,i,bad) && s[0][i-1] != 0)
                s[0][i] = 1;
        }

        for (int i = 1; i <= width; i++) {
            for (int j = 1; j <= height; j++) {
                if (!isBlocked(i,j-1,i,j,bad)) {
                    if (s[i][j-1] != 0) {
                        s[i][j] += s[i][j-1];
                    }
                }
                if  (!isBlocked(i-1,j,i,j,bad)) {
                    if (s[i-1][j] != 0) {
                        s[i][j] += s[i-1][j];
                    }
                }
            }
        }

        return s[width][height];
    }

    static boolean isBlocked(int x0,int y0, int x1, int y1, String[] bad) {
        if (bad == null)
            return false;
        String checker = ""+x0+y0+x1+y1;
        String checker1 = ""+x1+y1+x0+y0;
        for (String blocker:bad) {
            if (blocker.equalsIgnoreCase(checker) || blocker.equalsIgnoreCase(checker1))
                return true;
        }
        return false;
    }

    static void testBadNeighbors() {
        int d[] = { 94, 40, 49, 65, 21, 21, 106, 80, 92, 81, 679, 4, 61,
                6, 237, 12, 72, 74, 29, 95, 265, 35, 47, 1, 61, 397,
                52, 72, 37, 51, 1, 81, 45, 435, 7, 36, 57, 86, 81, 72  };
        System.out.println(""+badNeighbors(d));
    }

    static int badNeighbors(int d[]) {
        if (d.length == 1)
            return d[0];
        if (d.length == 2)
            return Math.max(d[0], d[1]);
        int s[] = new int[d.length];
        boolean a[] = new boolean[d.length];
        s[0] = d[0];
        a[0] = true;
        if (d[1] > d[0]) {
            s[1] = d[1];
            a[1] = false;
        } else {
            s[1] = d[0];
            a[1] = true;
        }

        for (int i = 2; i < d.length; i++) {
            s[i] = d[i];
            a[i] = false;
            for (int j = 0; j < i; j++) {
                if (i - j >= 2 ) {
                    if ((i != d.length - 1) || (i == d.length - 1 && !a[j])) {
                        if ((s[j] + d[i]) > s[i]) {
                            s[i] = s[j] + d[i];
                            a[i] = a[j];
                        }
                    }
                }
            }
        }
        int max = 0;
        for (int i = 0; i < d.length; i++) {
            if (s[i] > max)
                max = s[i];
        }
        return max;
    }

    static void testZigZag() {
        int a[] = { 374, 40, 854, 203, 203, 156, 362, 279, 812, 955,
                600, 947, 978, 46, 100, 953, 670, 862, 568, 188,
                67, 669, 810, 704, 52, 861, 49, 640, 370, 908,
                477, 245, 413, 109, 659, 401, 483, 308, 609, 120,
                249, 22, 176, 279, 23, 22, 617, 462, 459, 244 };
        System.out.println(""+zigZag(a));
    }

    static int zigZag(int a[]) {
        if (a.length <= 2)
            return a.length;
        int s[] = new int[a.length];
        s[0] = 1;
        s[1] = 2;
        for (int i = 2; i < a.length; i++) {
            s[i] = 2;
            for (int j = 1; j < i; j++) {
                if ((a[i]-a[j])*(a[j]-a[j-1]) < 0) {
                    if (s[j] + 1 > s[i]) {
                        s[i] = s[j] + 1;
                    }
                }
            }
        }
        int max = 0;
        for (int i = 0; i < a.length; i++) {
            if (s[i] > max)
                max = s[i];
        }
        return max;
    }

    static void testLND() {
        int a[] = {3,9,8,1,2,5,4,6,7,8,9,3};
        System.out.println(""+LND(a));
    }
    static int LND(int a[]) {
        int s[] = new int[a.length];
        int idx[] = new int[a.length];
        int OPT = 0;
        int final_idx = -1;
        for (int i = 0; i < a.length; i++) {
            s[i] = 1;
            for (int j = 0; j < i; j++) {
                if (a[i] >= a[j]) {
                    if (s[j] + 1 > s[i]) {
                        s[i] = s[j] + 1;
                        idx[i] = j;
                    }
                }
            }

        }
        for (int k = 0; k < a.length; k++) {
            System.out.println(""+s[k]);
            if (s[k] > OPT) {
                OPT = s[k];
                final_idx = k;
            }
        }
        System.out.println();
        printLND(a, idx, final_idx);
        System.out.println();
        return OPT;
    }
    static void printLND(int a[], int idx[], int final_idx) {
        int prev_idx = idx[final_idx];
        if (prev_idx == final_idx)
            return;
        printLND(a, idx, prev_idx);
        System.out.println(a[final_idx]+"");
    }
    static void testLCS() {
        char x[] = {'a','b','c','b','d','a','b'};
        char y[] = {'b','d','c','a','b','a'};
        int c[][] = new int[x.length][y.length];
        for (int i = 0; i < x.length; i++)
            for (int j = 0; j < y.length; j++)
                c[i][j] = Integer.MIN_VALUE;
        System.out.println(""+LCS(x,y,x.length-1,y.length-1,c));
        int i = x.length - 1;
        int j = y.length - 1;
        while (i >= 0 && j >= 0) {
            if (x[i] == y[j]) {
                System.out.println(x[i]);
                i--;
                j--;
            } else {
                if (i-1>=0 && j-1>=0) {
                    if (c[i-1][j] > c[i][j-1])
                        i--;
                    else
                        j--;
                }
            }
        }

    }
    static int LCS(char[] x, char[] y, int i, int j, int c[][]) {
        if (i < 0 || j < 0)
            return 0;
        if (c[i][j] == Integer.MIN_VALUE) {
            if (x[i] == y[j]) {
                c[i][j] = LCS(x,y,i-1,j-1,c) + 1;
            } else {
                c[i][j] = Math.max(LCS(x,y,i-1,j,c), LCS(x,y,i,j-1,c));
            }
        }
        return c[i][j];
    }
    static void testMinCoins() {
        int v[] = {1,3,5};
        for (int i = 0; i < 12; i++) {
            int c[] = new int[i+1];
            System.out.print(i+":"+minCoins(v,i,c)+"---");
            int value = i;
            while (value > 0) {
                System.out.print(c[value]+",");
                value = value - c[value];
            }
            System.out.println();
        }
    }

    static int minCoins(int v[], int n, int c[]) {
        int min[] = new int[n+1];
        //c = new int[n+1];
        for (int i = 0; i < min.length; i++)
            min[i] = Integer.MAX_VALUE;
        min[0] = 0;
        for (int i = 1; i <= n; i++) {
            for (int j = 0; j < v.length; j++) {
                if (v[j] <= i && min[i-v[j]] + 1 < min[i]) {
                    min[i] = min[i-v[j]] + 1;
                    c[i] = v[j];
                }
            }
        }
        return min[n];
    }

    static void testCutRod() {
        int p[] = {0,1,5,8,9,10,17,17,20,24,30};
        for (int i = 0; i < p.length; i++) {
            long begin = System.currentTimeMillis();
            //System.out.print(""+cutRod(p,i));
            /*
            int r[] = new int[p.length];
            for (int j = 0; j < r.length; j++)
                r[j] = Integer.MIN_VALUE;
            System.out.print(""+cutRodDP1(p, i, r));
            */
            System.out.print(""+cutRodDP2(p, i));
            long end = System.currentTimeMillis();
            System.out.println("----"+(end-begin));
        }
    }

    static int cutRodDP2(int p[], int n) {
        int r[] = new int[p.length];
        int s[] = new int[p.length];
        r[0] = 0;
        for (int j = 1; j <= n; j++) {
            int q = Integer.MIN_VALUE;
            for (int i = 1; i <= j; i++) {
                if (q < (p[i] + r[j - i])) {
                    q = p[i] + r[j - i];
                    s[j] = i;
                }
            }
            r[j] = q;
        }
        return r[n];
    }

    static int cutRodDP1(int p[], int n, int r[]) {
        int q;
        if (r[n] >= 0)
            return r[n];
        if (n == 0)
            q = 0;
        else {
            q = Integer.MIN_VALUE;
            for (int i = 1; i <= n; i++) {
                q = Math.max(q, p[i]+cutRodDP1(p,n-i,r));
            }
        }
        r[n] = q;
        return q;
    }

    static int cutRod(int p[], int n) {
        if (n == 0)
            return 0;
        int q = Integer.MIN_VALUE;
        for (int i = 1; i <= n; i++) {
            q = Math.max(q, p[i]+cutRod(p,n-i));
        }

        return q;
    }

}
