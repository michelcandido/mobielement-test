
public class MyLinkedList<T> {
     class Node<T> {
        public T element;
        public Node<T> next;
    }

    public Node<T> head = null;
    public Node<T> last = null;

    public MyLinkedList(T[] a) {
        this.head = new Node<T>();
        head.element = a[0];
        head.next = null;
        last = head;
        for (int i = 1; i < a.length; i++) {
            Node<T> newNode = new Node<T>();
            newNode.element = a[i];
            newNode.next = null;
            last.next = newNode;
            last = newNode;
        }
    }

    public MyLinkedList() {
        // TODO Auto-generated constructor stub
    }

    public Node<T> getNth(int n) {
        Node<T> node = head;
        int count = 0;
        while (node != null) {
            count++;
            if (count == n)
                return node;
            node = node.next;
        }
        return null;
    }

    public int getCount() {
        Node<T> node = head;
        int count = 0;
        while (node != null) {
            count++;
            node = node.next;
        }
        return count;
    }

    public void print() {
        Node<T> node = head;
        while (node != null) {
            if (node.next!=null)
                System.out.print(node.element+"->");
            else
                System.out.println(node.element);
            node = node.next;
        }
    }

    public void push(T element) {
        if (head == null) {
            head = new Node<T>();
            head.element = element;
            head.next = null;
            last = head;
        } else {
            Node<T> node = new Node<T>();
            node.element = element;
            node.next = null;
            last.next = node;
            last = node;
        }
    }
}
