using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            Q4();
        }

        private static void Q4()
        {
            int[] n1 = new int[] { 9,9,9,9 };
            int[] n2 = new int[] { 9,9,9 };
            LinkedList<int> l1 = new LinkedList<int>(n1);
            LinkedList<int> l2 = new LinkedList<int>(n2);
            print(l1);
            print(l2);

            LinkedList<int> l3 = new LinkedList<int>();
            LinkedListNode<int> d1 = l1.First, d2 = l2.First;
            LinkedListNode<int> d = null;

            d = d1;
            int num = 0;
            int i  = 1;
            while (d != null)
            {
                num += d.Value * i;
                d = d.Next;
                i = i * 10;                
            }
            int num1 = num;
            num = 0;
            i = 1;
            d = d2;
            while (d != null)
            {
                num += d.Value * i;
                d = d.Next;
                i = i * 10;                
            }
            int num2 = num;
            num = num1 + num2;
            Console.WriteLine(num);
            /*
            int carryover = 0;
            while (d1 != null && d2 != null)
            {
                int value = d1.Value + d2.Value + carryover;
                carryover = value / 10;
                value = value - carryover * 10;
                l3.AddLast(value);
                d1 = d1.Next;
                d2 = d2.Next;
            }
            LinkedListNode<int> d = null;
            if (d1 == null)
                d = d2;
            else if (d2 == null)
                d = d1;
            while (d != null)
            {
                int value = d.Value + carryover;
                carryover = value / 10;
                value = value - carryover * 10;
                l3.AddLast(value);
                d = d.Next;
            }
            if (carryover != 0)
                l3.AddLast(carryover);
            */
            print(l3);
        }

        private static void Q3()
        {
            LinkedList<int> list = new LinkedList<int>();
            int[] array = new int[] { 23, 45, 12, 46, 87, 74, 65, 89 };            
            for (int i = 0; i < array.Length; i++)
                list.AddFirst(array[i]);
            print(list);

            LinkedListNode<int> node;
            node = list.Find(87);

            while (node.Next != null)
            {
                node.Value = node.Next.Value;
                node = node.Next;
            }
            list.Remove(node);

            print(list);
        }

        private static void Q1()
        {
            LinkedList<int> list = new LinkedList<int>();
            //int[] array = new int[] { 23, 45, 12, 46, 87, 74, 65, 87 };
            int[] array = new int[] { 12, 12, 12};
            for (int i = 0; i < array.Length; i++)
                list.AddFirst(array[i]);
            print(list);

            int checker = 0;
            LinkedListNode<int> node;
            node = list.First;
            
            while(node != null)
            {
                if ((checker & (1 << node.Value)) > 0)
                {
                    LinkedListNode<int> tmp = node.Next;
                    list.Remove(node);
                    node = tmp;
                    continue;
                }
                else 
                {
                    checker |= (1 << node.Value);
                    node = node.Next;
                }
                
            }
            print(list);
        }

        private static void print(LinkedList<int> list)
        {
            foreach (int i in list)
                Console.Out.Write(i + " ");
            Console.Out.WriteLine("");
        }
    }
}
