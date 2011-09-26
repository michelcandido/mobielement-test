using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Recursion
{
   class Program
   {
      static void Main(string[] args)
      {
         Q4();
      }

      private static void Q4()
      {
         string source = "abdfsdfs";
         ArrayList result = PermuteString(source);
         foreach (string aset in result)
         {
            Console.WriteLine(aset);            
         }
      }

      private static ArrayList PermuteString(string source)
      {
         ArrayList result = new ArrayList();
         if (source.Length == 1)
         {
            result.Add(source);
            return result;
         }
         string head = source.Substring(0,1);
         string rest = source.Substring(1);
         ArrayList permuteSet = PermuteString(rest);
         foreach (string astring in permuteSet)
         {
            for (int i = 0; i <= astring.Length; i++)
            {
               result.Add(astring.Insert(i, head));
            }
         }
         return result;
      }
      private static void Q3()
      {
         ArrayList source = new ArrayList();
         source.Add(1);
         source.Add(2);
         source.Add(3);
         source.Add(4);

         ArrayList subsets = GetSubset(source);
         foreach (ArrayList aset in subsets)
         {
            foreach (int value in aset)
               Console.Write(value + " ");
            Console.WriteLine();
         }
      }

      private static ArrayList GetSubset(ArrayList source)
      {
         ArrayList subsets = new ArrayList();
         if (source.Count == 1)
         {
            subsets.Add(source);
            return subsets;
         }
         
         ArrayList newsource = (ArrayList)source.Clone();
         newsource.RemoveAt(0);
         int myvalue = (int)source[0];
         ArrayList mySubsets = GetSubset(newsource);
         foreach(ArrayList aset in mySubsets)
         {
            ArrayList newsubset = new ArrayList();
            newsubset = (ArrayList)aset.Clone();
            newsubset.Add(myvalue);
            subsets.Add(newsubset);
            subsets.Add(aset);
         }
         
         ArrayList mysubset = new ArrayList();
         mysubset.Add(myvalue);
         subsets.Add(mysubset);


         
         return subsets;
 
      }
   }
}
