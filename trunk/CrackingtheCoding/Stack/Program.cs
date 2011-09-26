using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace StackQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Q3();
        }

        

        private static void Q3()
        {
            SetOfStack ss = new SetOfStack();
            ss.Push(1);
            ss.Push(2);
            ss.Push(3);
            ss.Push(4);


            ss.Print();
            Console.WriteLine(ss.PopAt(4));
            ss.Print();
            Console.WriteLine(ss.PopAt(0));
            ss.Print();
            Console.WriteLine(ss.PopAt(1));
            ss.Print();
            Console.WriteLine(ss.Pop());
            ss.Print();

            /*
            ss.Print();
            Console.WriteLine(ss.Pop());
            ss.Print();
            Console.WriteLine(ss.Pop());
            ss.Print();
            Console.WriteLine(ss.Pop());
            ss.Print();
            Console.WriteLine(ss.Pop());
            ss.Print();
            Console.WriteLine(ss.Pop());
            ss.Print();
             * */
        }
    }

    class MyQueue<T>
    {
        Stack<T> s1, s2;
        public MyQueue()
        {
            s1 = new Stack<T>();
            s2 = new Stack<T>();
        }

        public int Size()
        {
            return s1.Count + s2.Count;
        }

        public void Add(T value)
        {
            s1.Push(value);
        }

        public T Peek()
        {
            if (s2.Count != 0)
                return s2.Peek();
            else
            {
                while (s1.Count != 0)
                    s2.Push(s1.Pop());
                return s2.Peek();
            }
        }

        public T Remove()
        {
            if (s2.Count != 0)
                return s2.Pop();
            else
            {
                while (s1.Count != 0)
                    s2.Push(s1.Pop());
                return s2.Pop();
            }
        }
    }

    class SetOfStack
    {
        private static int CAP = 2;        
        
        ArrayList Stacks = new ArrayList();        
        public int Index = 0; 

        public void Push(int value)
        {                        
            if (Stacks.Count == 0 || Stacks[Index] == null || ((Stack<int>)Stacks[Index]).Count >= CAP)
            {
                if (Stacks.Count != 0)
                    Index++;
                Stacks.Add(new Stack<int>());                
            }
            ((Stack<int>)Stacks[Index]).Push(value);
        }

        public void Print()
        {
            int i = 0;
            foreach (Stack<int> stack in Stacks)
            {
                Console.Write("stack #" + i + ":");
                foreach (int element in stack)
                {
                    Console.Write(element + " ");
                }
                Console.WriteLine();
                i++;
            }
        }

        public int PopAt(int index)
        {
            if (Stacks.Count == 0)
            {
                Console.Write("empty.");
                return -1;
            }

            if (index > Index)
            {
                Console.Write("not available.");
                return -1;
            }

            int value = ((Stack<int>)Stacks[index]).Pop();
            if (((Stack<int>)Stacks[index]).Count == 0)
            {                
                Stacks.RemoveAt(index);
                if (Index != 0)
                {
                    Index--;
                }                
            }
            return value;
        }

        public int Pop()
        {
            if (Stacks.Count == 0 )
            {
                Console.Write("empty.");
                return -1;
            }

            int value = ((Stack<int>)Stacks[Index]).Pop();
            if (((Stack<int>)Stacks[Index]).Count == 0)
            {
                Stacks.RemoveAt(Index);
                if (Index != 0)
                {                    
                    Index--;
                }                                
            }

            return value;
        }
    }
}
