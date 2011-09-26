using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeGraphs
{
   class Program
   {
      static void Main(string[] args)
      {
         int[] array = new int[] { 1,2};
         //PrintTree(Q3(array));
         //Q4();
         FindResult(BuildTree(), new Stack<TreeNode>());
      }

      private static void FindResult(TreeNode node, Stack<TreeNode> stack)
      {
         if (node == null)
         {
            PrintStack(stack);
            CheckTarget(stack, 8);            
            return;
         }
         
         stack.Push(node);         
         FindResult(node.Left, stack);
         stack.Pop();   
         PrintStack(stack);
         CheckTarget(stack, 8);
            
                  
         stack.Push(node);
         FindResult(node.Right, stack);         
         stack.Pop();
         PrintStack(stack);
         CheckTarget(stack, 8);
         
      }

      private static void CheckTarget(Stack<TreeNode> s, int target)
      {
         Stack<TreeNode> stack = new Stack<TreeNode>();
         for (int i = s.Count - 1; i >= 0; i--)
         {
            stack.Push(s.ElementAt(i));
         }
         int result = 0;
         Stack<TreeNode> resultSet = new Stack<TreeNode>();
         while (stack.Count != 0)
         {
            TreeNode n = stack.Pop();
            resultSet.Push(n);
            result += n.Value;
            if (result == target)
            {
               Console.Write("find: ");
               PrintStack(resultSet);
               return;
            }
         }
      }

      private static void PrintStack(Stack<TreeNode> s)
      {
         Stack<TreeNode> stack = new Stack<TreeNode>();
         for (int i = 0; i < s.Count; i++)
         {
            stack.Push(s.ElementAt(i));
         }
         while (stack.Count != 0)
         {
            Console.Write(stack.Pop().Value + " ");
         }
         Console.WriteLine();
      }

      private static TreeNode BuildTree()
      {
         TreeNode root = new TreeNode(5);
         TreeNode n1 = new TreeNode(3);
         TreeNode n2 = new TreeNode(8);
         TreeNode n3 = new TreeNode(2);
         TreeNode n4 = new TreeNode(4);
         TreeNode n5 = new TreeNode(6);
         TreeNode n6 = new TreeNode(10);
         TreeNode n7 = new TreeNode(7);
         TreeNode n8 = new TreeNode(9);

         root.Left = n1;
         root.Right = n2;
         n1.Left = n3;
         n1.Right = n4;
         n2.Left = n5;
         n2.Right = n6;
         n5.Right = n7;
         n6.Left = n8;

         return root;
      }

      public static void Q4()
      {
         int[] array = new int[] { 1, 2,3,4,5,6,7,8,9 };
         LinkedList<LinkedList<int>> list = new LinkedList<LinkedList<int>>();
         TreeNode node = Q3(array);
         AddToList(node, 1, ref list);
         PrintList(list);
      }

      public static void PrintList(LinkedList<LinkedList<int>> list)
      {
         int i = 0;
         foreach (LinkedList<int> theList in list)
         {
            Console.Write("depth " + i + ":");
            foreach (int v in theList)
               Console.Write(v + " ");
            Console.WriteLine();
            i++;
         }
      }

      public static void AddToList(TreeNode node, int depth, ref LinkedList<LinkedList<int>> list)
      {
         if (node == null)
            return;
         if (depth > list.Count)
         {
            LinkedList<int> newList = new LinkedList<int>();
            newList.AddLast(node.Value);
            list.AddLast(newList);
         }
         else
         {
            LinkedList<int> theList = list.ElementAt(depth - 1);
            theList.AddLast(node.Value);
         }
         AddToList(node.Left, depth + 1, ref list);
         AddToList(node.Right, depth + 1, ref list);
      }


      public static TreeNode Q3(int[] array)
      {
         return AddToTree(array, 0, array.Length - 1);
      }

      public static TreeNode AddToTree(int[] array, int start, int end)
      {
         TreeNode n = new TreeNode(0);
         if (end < start)
            return null;
         int mid = (start + end) / 2;
         n.Value = array[mid];
         n.Left = AddToTree(array, start, mid - 1);
         n.Right = AddToTree(array, mid + 1, end);
         return n;
      }

      public static void PrintTree(TreeNode node)
      {
         if (node == null)
            return;
         PrintTree(node.Left);
         Console.WriteLine(node.Value);
         PrintTree(node.Right);

      }
   }

   class TreeNode
   {
      public TreeNode Left { get; set; }
      public TreeNode Right { get; set; }
      public int Value { get; set; }

      public TreeNode(int Value)
      {
         this.Value = Value;          
      }
   }
}
