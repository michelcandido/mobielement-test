
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
		testBadNeighbors();
	}
	
	static void testBadNeighbors() {
		int d[] = { 10, 3, 2, 5,7,8 };
		System.out.println(""+badNeighbors(d));
	}
	
	static int badNeighbors(int d[]) {
		if (d.length == 1)
			return d[0];
		if (d.length == 2)
			return Math.max(d[0], d[1]);
		int s[] = new int[d.length];
		s[1] = s[2] = Math.max(d[0], d[1]);
		for (int i = 2; i < d.length; i++) {
			s[i] = Integer.MIN_VALUE;
			for (int j = 0; j < i; j++) {
				if (i - j >= 2 ) {
					if ((i != d.length - 1) || (i == d.length - 1 && j != 0)) {
						if ((s[j] + d[i]) > s[i])
							s[i] = s[j] + d[i];
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
