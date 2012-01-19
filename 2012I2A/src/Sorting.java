
public class Sorting {
	static void printIntArray(int[] src) {
		for (int i:src)
			System.out.format("%d ", i);
		System.out.println();
	}
	static int[] insertionSort(int[] src) {
		if (src == null)
			return null;
		int n = src.length - 1;
		if (n == 0)
			return src;
		for (int j = 1; j <= n; j++) {
			int key = src[j];
			int i = j - 1;
			while (i >= 0 && src[i] > key) {
				src[i+1] = src[i];
				i--;
			}
			src[i+1] = key;
		}		
		return src;
	}
	
	static void testInsertionSort() {
		int[] a = {8,2,4,9,3,6};
		printIntArray(a);
		printIntArray(insertionSort(a));
		printIntArray(a);
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		testInsertionSort();
	}

}
