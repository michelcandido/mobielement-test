
public class GArray {
	static int findMaxLinear(int src[]) {
		if (src == null || src.length == 0)
			return -1;
		int max = src[0];
		for (int i:src) {
			if (src[i] > max)
				max = src[i];
		}
		return max;
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}

}
