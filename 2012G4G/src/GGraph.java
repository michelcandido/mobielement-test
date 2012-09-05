import java.util.ArrayList;
import java.util.LinkedList;


public class GGraph {
    static class Node<T> {
        public T data;
        public Node<T>[] neightbors;
        public Node(T d) {
            data = d;
        }
    }
    public static void main(String[] args) {
        testMarketing();
     }

    static void testMarketing() {
        String[] compete = {"1 4","2","3","0",""};
        System.out.println(marketing(compete));
    }
     static long marketing(String[] compete) {
         int number = compete.length, count = -1;
         boolean[] visited = new boolean[number];
         ArrayList<ArrayList<Integer>> neighbors = new ArrayList<ArrayList<Integer>>();
         for (int i = 0; i < number; i++) {
        	 ArrayList<Integer> list = new ArrayList<Integer>();
        	 neighbors.add(list);
         }
         int[] product = new int[number];
         ArrayList<Integer> nodes = new ArrayList<Integer>();
         for (int i = 0; i < number; i++) {
             visited[i] = false;
             product[i] = -1;             
             nodes.add(i);
             String[] clist = compete[i].split(" ");
             ArrayList<Integer> list = neighbors.get(i);
             for (String c:clist) {
            	 if (c!="") {
            		 list.add(Integer.valueOf(c));
            		 neighbors.get(Integer.valueOf(c)).add(i);
            	 }
             }
         }
         for (int i = 0; i < number; i++) {
        	 for (int j:neighbors.get(i))
        		 System.out.print(j + " ");
        	 System.out.println();
         }
         
         return count;
     }
}
