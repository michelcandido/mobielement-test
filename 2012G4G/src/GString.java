import java.util.Hashtable;


public class GString {

    /**
     * @param args
     */
    public static void main(String[] args) {
        //testIsRatation();
        //testReverseStringRecursion();
        //testPrintPermutation();
        //testReverseWords();
        //testRunLengthEncoding();
    	//testFindMinWindow();
    	//testSearchPattern();
    	//testLongestNonRepeatSubString();
    	testFindMinPartitioning();
    }
    
    static void testFindMinPartitioning() {
    	String s = "ababbbabbababa";
    	findMinPartitioning(s);
    }
    static void findMinPartitioning(String s) {
    	if (s == null)
    		return;
    	if (s.length() == 1)
    		System.out.println(1);
    	int n = s.length();
    	boolean[][] p = new boolean[n][n];
    	int[][] c = new int[n][n];
    	for (int i = 0; i < n; i++) {
    		p[i][i] = true;
    		c[i][i] = 0;
    	}
    	
    	for (int l = 2; l <= n; l++) {
    		for (int i = 0; i < n-l+1; i++) {
    			int j = i+l-1;
    			if (l == 2)
    				p[i][j] = (s.charAt(i) == s.charAt(j));
    			else
    				p[i][j] = (s.charAt(i) == s.charAt(j)) && p[i+1][j-1];
    			if (p[i][j])
    				c[i][j] = 0;
    			else {
    				c[i][j] = Integer.MAX_VALUE;
    				for (int k = i; k < j; k++) {
    					int t = c[i][k] + 1 + c[k+1][j];
    					if (t < c[i][j])
    						c[i][j] = t;
    				}
    			}
    		}
    	}
    	System.out.println(c[0][n-1]);
    }
    static boolean isPalindrome(String str, int start, int end) {
    	int s = start, e = end;
    	while (s<e) {
    		if (str.charAt(s) != str.charAt(e))
    			return false;
    	}
    	return true;
    }
    static void testLongestNonRepeatSubString() {
    	String s = "ABDEFGABEF";
    	longestNonRepeatSubString(s);
    }
    static void longestNonRepeatSubString(String s) {
    	Hashtable<Character, Integer> visit = new Hashtable<Character, Integer>();
    	int max_len = 0, cur_len = 0, start = 0;
    	for (int i = 0; i < s.length(); i++) {
    		char c = s.charAt(i);
    		if (!visit.containsKey(c)) {
    			cur_len++;
    			visit.put(c, i);
    			if (cur_len > max_len)
    				max_len = cur_len;
    		} else {    			
    			int idx = visit.get(c);
    			while (start <= idx) {
    				visit.remove(s.charAt(start));
    				start++;
    			}
    			cur_len = i - start + 1;
    		}
    	}
    	System.out.println(max_len);
    }
    
    static void testSearchPattern() {
    	String s1 = "AABAACAADAABAAABAA";
    	String s2 = "AABA";
    	searchPattern(s1, s2);
    }
    static void searchPattern(String s1, String s2) {
    	if (s1 == null || s2 == null)
    		return;
    	if (s1.length() < s2.length())
    		System.out.println("no match");
    	else {
    		int i1 = 0, i2 = 0;
    		char[] c1 = s1.toCharArray(), c2 = s2.toCharArray();
    		while (i1 < s1.length()) {
    			if (c1[i1] == c2[i2]) {
    				i1++;
    				i2++;
    				if (i2 >= s2.length()) {
    					System.out.println(i1-s2.length());
    					i1 = (i1 - i2) + 1;
    					i2 = 0;
    				}
    			} else {
    				i1 = (i1 - i2) + 1;    				
    				i2 = 0;
    			}
    		}
    	}
    }

    static void testFindMinWindow() {
    	String s1 = "this is a test string";
    	String s2 = "tist";
    	findMinWindow(s1,s2);
    }
    static void findMinWindow(String s1, String s2) {
        Hashtable<Character, Integer> s2set = new Hashtable<Character, Integer>();
        Hashtable<Character, Integer> s2count = new Hashtable<Character, Integer>();
        Hashtable<Character, Integer> s1act = new Hashtable<Character, Integer>();
        char[] str1 = s1.toCharArray(), str2 = s2.toCharArray();
        int count = 0;
        // construct the target table for s2
        for (char c:str2) {
            if (s2set.containsKey(c)) {
                count = s2set.get(c);
                s2set.put(c, ++count);
            } else {
                s2set.put(c, 1);
            }
        }
        s2count.putAll(s2set);
        // find the first window
        int i = 0, start = -1;
        for (i = 0; i < str1.length; i++) {
            char c = str1[i];
            if (s2count.isEmpty())
                break;
            if (s2set.containsKey(c)) {
            	if (start == -1)
            		start = i;
                if (s1act.containsKey(c)) {
                    count = s1act.get(c);
                    s1act.put(c, ++count);
                } else {
                    s1act.put(c, 1);
                }
                if (s2count.containsKey(c)) {
                    count = s2count.get(c);
                    count--;
                    if (count == 0)
                        s2count.remove(c);
                    else
                        s2count.put(c, count);
                }
            }
        }
        //System.out.println(s1.substring(0,i));
        int min = i, min_start = start, min_end = i;
        for (; i < str1.length; i++) {
        	char c = str1[i];
        	if (s2set.containsKey(c)) {        		
        		count = s1act.get(c);
        		s1act.put(c, ++count);
        		if (c == str1[start]) {// we can move start pointer now
        			boolean keepgoing = true;
        			do {
	        			if (s2set.containsKey(str1[start])) {
	        				count = s1act.get(str1[start]);
	        				if (count <= s2set.get(str1[start])) {
	        					keepgoing = false;	        					
	        				} else {	        					
	        					s1act.put(str1[start], --count);
	        					start++;
	        				}
	        			} else {
	        				start++;
	        			}
        			} while (keepgoing);
        			if (i - start + 1 < min) {
        				min = i - start + 1;
        				min_start = start;
        				min_end = i + 1;
        			}
        		} 
        			
        	} 
        }
        System.out.println(s1.substring(min_start,min_end));
    }

    static void testRunLengthEncoding() {
        String s = "wwwwaaadexxxxxx";
        runLengthEncoding(s);
    }
    static void runLengthEncoding(String s) {
        if (s == null)
            return;
        char[] a = s.toCharArray();
        StringBuilder sb = new StringBuilder();
        char c = 0;
        int count = 0;
        for (int i = 0; i < s.length(); i++) {
            if (c != a[i]) {
                sb.append(c);
                sb.append(count);
                c = a[i];
                count = 1;
            } else {
                count++;
            }
        }
        sb.append(c);
        sb.append(count);
        System.out.println(sb.toString());
    }

    static void testReverseWords() {
        String s = "i like this program very much";
        reverseWords(s);
    }
    static void reverseWords(String s) {
        if (s == null)
            return;
        int start = 0,  end = 0;
        StringBuilder newstrsb = new StringBuilder();
        char[] str = s.toCharArray();
        while (end < s.length()) {
            if (str[end] == ' ') {

                newstrsb.append(reverseString(str, start, end -1));
                newstrsb.append(' ');
                end++;
                start = end;
            } else {
                end++;
            }
        }
        newstrsb.append(reverseString(str, start, end - 1));

        System.out.println(reverseString(newstrsb.toString().toCharArray(), 0, s.length()-1));
    }
    static String reverseString(char[] carr, int b, int e) {
        if (carr == null)
            return null;
        int begin = b, end = e;
        while (begin < end) {
            char c = carr[begin];
            carr[begin++] = carr[end];
            carr[end--] = c;
        }
        return new String(carr,b,e-b+1);
    }

    static void testPrintPermutation() {
        String s = "abcd";
        String[] set = printPermutation(s);
        for (int i = 0; i < set.length; i++)
            System.out.println(set[i]);
    }
    static String[] printPermutation(String s) {
        String[] set;
        if (s == null || s.length() == 0)
            return null;
        int size = 1;
        for (int i = 1; i <= s.length(); i++)
            size *= i;
        set = new String[size];

        if (s.length() == 1) {
            set[0] = s;
            return set;
        } else {
            String[] subset = printPermutation(s.substring(1));
            int count = 0;
            for (int i = 0; i < subset.length; i++) {
                for (int j = 0; j <= subset[i].length(); j++) {
                    String first = subset[i].substring(0,j);
                    String second = s.substring(0,1);
                    String third = subset[i].substring(j);
                    set[count] = first + second + third;
                    count++;
                }
            }
            return set;
        }

    }
    static void testReverseStringRecursion() {
        String s = "a";
        reverseStringRecursion(s);
    }
    static void reverseStringRecursion(String s) {
        if (s == null)
            return;
        if (s.length() <= 1) {
            System.out.print(s);
        } else {
            reverseStringRecursion(s.substring(1));
            System.out.print(s.substring(0, 1));
        }
    }

    static void testIsRatation() {
        String s1 = "";
        String s2 = "";
        System.out.println(isRatation(s1,s2));
    }
    static boolean isRatation(String s1, String s2) {
        if (s1 == null || s2 == null)
            return false;
        if (s1.length() != s2.length())
            return false;
        char[] c1 = s1.toCharArray();
        char[] c2 = s2.toCharArray();
        int j;
        for (int i = 0; i < c1.length; i++) {
            j = c1.length - i - 1;
            if (c1[i] != c2[j])
                return false;
        }
        return true;
    }
}
