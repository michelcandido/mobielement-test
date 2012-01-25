
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
	
	static int[] mergeSort(int[] src) {
		if (src == null)
			return src;
		int n = src.length - 1;
		if (n == 0)
			return src;
		
		int[] left = new int[src.length / 2];
		int[] right = new int[src.length - left.length];
		System.arraycopy(src, 0, left, 0, left.length);
		System.arraycopy(src, left.length, right, 0, right.length);
		left = mergeSort(left);
		right = mergeSort(right);
		src = doMerge(left,right);
		return src;
	}
	
	static int[] doMerge(int[] left, int right[]) {
		int lEnd = left.length - 1, rEnd = right.length - 1;
		int lStart = 0, rStart = 0, idx = 0;
		int[] result = new int[left.length + right.length];
		while (lStart <= lEnd && rStart <= rEnd) {
			if (left[lStart] < right[rStart]) {
				result[idx] = left[lStart];
				lStart++;
			} else {
				result[idx] = right[rStart];
				rStart++;
			}
			idx++;
		}
		
		if (lStart > lEnd) {
			System.arraycopy(right, rStart, result, idx, result.length - idx);			
		} else {
			System.arraycopy(left, lStart, result, idx, result.length - idx);
		}
		
		return result;
	}
	
	static void testInsertionSort() {
		int[] a = {8,2,4,9,3,6};
		printIntArray(a);
		printIntArray(insertionSort(a));
		printIntArray(a);
	}
	
	static void testMergeSort() {
		int[] a = {8,2,4,9,3,6,1};
		printIntArray(a);
		printIntArray(mergeSort(a));
		printIntArray(a);
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		//testInsertionSort();
		testMergeSort();
	}

}
