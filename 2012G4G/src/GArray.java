import java.util.HashMap;


public class GArray {
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

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		//testFindMax();
		testCheckPair();
	}

}
