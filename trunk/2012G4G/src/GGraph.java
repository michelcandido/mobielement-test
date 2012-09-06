import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.Hashtable;
import java.util.LinkedList;
import java.util.PriorityQueue;


public class GGraph {
    static class Node<T> {
        public T data;
        public Node<T>[] neightbors;
        public Node(T d) {
            data = d;
        }
    }
    public static void main(String[] args) {
        //testMarketing();
        //testCircuit();
    	testQueue();
     }

    static void testQueue() {
    	LinkedList<Integer> q = new LinkedList<Integer>();
    	q.offer(3);
    	q.offer(5);
    	q.offer(1);
    	PriorityQueue<Integer> pq = new PriorityQueue<Integer>();
    	pq.offer(3);
    	pq.offer(5);
    	pq.offer(1);
    	while (!q.isEmpty())
    		System.out.print(q.poll()+" ");
    	System.out.println();
    	while (!pq.isEmpty())
    		System.out.print(pq.poll()+" ");
    	System.out.println();
    }
    static int count_circuit = Integer.MIN_VALUE;
    static void testCircuit() {
        String[] connects = {"","2 3","3 4 5","4 6","5 6","7","5 7",""};//{"","2 3 5","4 5","5 6","7","7 8","8 9","10","10 11 12","11","12","12",""};//{"1","2","3","","5","6","7",""};//{"1 2 3 4 5","2 3 4 5","3 4 5","4 5","5",""};//{"1 2","2",""};
        String[] costs = {"","30 50","19 6 40","12 10","35 23","8","11 20",""};//{"","3 2 9","2 4","6 9","3","1 2","1 2","5","5 6 9","2","5","3",""};//{"2","2","2","","3","3","3",""};//{"2 2 2 2 2","2 2 2 2","2 2 2","2 2","2",""};//{"5 3","7",""};
        int number = connects.length;

        ArrayList<ArrayList<Integer>> connectList = new ArrayList<ArrayList<Integer>>();
        ArrayList<ArrayList<Integer>> costList = new ArrayList<ArrayList<Integer>>();
        for (int i = 0; i < number; i++) {
            ArrayList<Integer> conn = new ArrayList<Integer>();
            ArrayList<Integer> cost = new ArrayList<Integer>();
            String[] connArray = connects[i].split(" ");
            String[] costArray = costs[i].split(" ");
            for (String s: connArray) {
                if (s!= "") {
                    conn.add(Integer.valueOf(s));
                }
            }
            for (String s: costArray) {
                if (s!= "") {
                    cost.add(Integer.valueOf(s));
                }
            }
            connectList.add(conn);
            costList.add(cost);
        }
        for (int i = 0; i < number; i++) {
            for (int j:connectList.get(i))
                System.out.print(j + " ");
            System.out.println();
            for (int j:costList.get(i))
                System.out.print(j + " ");
            System.out.println();
        }
        System.out.println("------");
        for (int i = 0; i < number; i++)
            circuit(i,connectList, costList, 0);
        System.out.println("------\n"+count_circuit);
    }

    static void circuit(int c, ArrayList<ArrayList<Integer>> connectList, ArrayList<ArrayList<Integer>> costList, int totalCost) {
        if (connectList.get(c).size() <= 0) {
            System.out.println("found:"+totalCost);

            if (totalCost > count_circuit)
                count_circuit = totalCost;
            return;
        }
        for (int i = 0; i < connectList.get(c).size(); i++) {
            int target = connectList.get(c).get(i);
            if (connect(target, c, connectList)) {
                circuit(target, connectList, costList, totalCost + costList.get(c).get(i));
            }
        }
    }
    static boolean connect(int c, int last, ArrayList<ArrayList<Integer>> connectList) {
        if (last == -1)
            return true;
        if (connectList.get(last).contains(c))
            return true;
        return false;
    }

    static int count_marketing = 0;
    static void testMarketing() {
        String[] compete = {"1 4","2","3","0","",""};//{"","","","","","","","","","","","","","","","","","","","","","","","","","","","","",""};//{"1","2","3","0","5","6","4"};//{"1","2","0"};;

        int number = compete.length;
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
        System.out.println("------");
        product[0] = 1;
        System.out.println(2*marketing(1,number, product, neighbors,0));
    }
     static long marketing(int p, int size, int[] product, ArrayList<ArrayList<Integer>> neighbors, long count) {
         for (int i = 1; i <= 2; i++) {
             if (produce(p, i, product, neighbors)) {
                 product[p] = i;
                 if (p == size - 1) {
                     for (int j: product)
                         System.out.print(j + " ");
                     count_marketing++;
                     count++;
                     System.out.println("--"+count_marketing+"--"+count);
                 } else {
                     count = marketing(p+1, size, product, neighbors,count);
                 }
             }
         }
         //return count_marketing;
         return count;
     }
     static boolean produce(int p, int t, int[] product, ArrayList<ArrayList<Integer>> neighbors) {
         for (int i = 0; i < p; i++) {
             if (neighbors.get(i).contains(p) && product[i] == t)
                 return false;
         }
         return true;
     }
}
