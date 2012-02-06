
public class DP {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		testCutRod();

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
