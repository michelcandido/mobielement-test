using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArraysStrings
{
    class Program
    {
        static void Main(string[] args)
        {
            //Q1_3("Abc");
            //Console.Out.WriteLine(Q2("abccc"));
            Q5("AbaB    asd                             ");
            //Q8("waterbottle", "erbottlewat");
        }

        private static void Q8(String s1, String s2)
        {
            String tmp = String.Empty;
            Boolean result = false;
            if (s1.Length != s2.Length)
                result = false;
            else
            {
                for (int i = 0; i < s1.Length; i++)
                {
                    tmp = s1.Substring(i + 1) + s1.Substring(0, i + 1);
                    if (tmp.Equals(s2, StringComparison.CurrentCultureIgnoreCase))
                    {
                        result = true;
                        break;
                    }
                }
            }

            Console.Out.WriteLine(result);
        }

        private static void Q5(String source)
        {
            
            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == ' ')
                    count++;
            }
            char[] target = new char[source.Length + 2 * count];
            Array.Copy(source.ToCharArray(), target, source.Length);
            int pos = target.Length - 1;
            for (int i = source.Length - 1; i >= 0; i--)
            {
                if (target[i] != ' ')
                {
                    target[pos] = target[i];
                    pos--;
                }
                else
                {
                    target[pos] = '0';
                    target[pos - 1] = '2';
                    target[pos - 2] = '%';
                    pos -= 3;
                }
            }
            Console.WriteLine(new String(target));
        }

        private static void Q3(String str)
        {
            str = str.ToLower();
            int checker = 0;

            for (int i = 0; i < str.Length; i++)
            {
                int idx = str[i] - 'a';
                if ((checker & (1 << idx)) > 0)
                {
                    str = str.Remove(i,1);
                    i--;
                    continue;
                }
                checker |= (1 << idx);
                
            }

            Console.Out.WriteLine(str);

        }

        private static void Q3_1(String str)
        {
            int checker = 0;
            char[] target = str.ToCharArray();
            for (int i = 0; i < target.Length; i++)
            {
                int idx = target[i] - 'a';
                if ((checker & (1 << idx)) != 0)
                {
                    target[i] = '\0';
                }
                checker |= (1 << idx);
            }
            int pos = 0;
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] != '\0')
                {
                    if (i != pos)
                    {
                        target[pos] = target[i];
                        target[i] = '\0';
                    }
                    pos++;
                }
            }

            Console.WriteLine(new String(target));
        }

        private static String Q2(String str)
        {
            if (str.Length == 0)
                return "";
            
            return Q2(str.Substring(1)) + str.Substring(0, 1);
        }

        private static void Q1(String source)
        {
            source = source.ToLower();
            int[] check = new int[26];
            for (int i = 0; i < check.Length; i++)
                check[i] = 0;

            for (int i = 0; i < source.Length; i++)
            {
                int idx = source[i] - 'a';
                if (check[idx] != 0)
                {
                    Console.Out.WriteLine("Fail.");
                    return;
                }
                else
                {
                    check[idx] = 1;
                }
            }

            Console.Out.WriteLine("Success.");
        }

        private static void Q1_2(String source)
        {
            source = source.ToLower();
            int checker = 0;
            for (int i = 0; i < source.Length; i++)
            {
                int idx = source[i] - 'a';
                if ((checker & (1 << idx)) > 0)
                {
                    Console.Out.WriteLine("Fail.");
                    return;
                }
                checker |= (1 << idx);
            }

            Console.Out.WriteLine("Success.");
        }

        private static void Q1_3(String source)
        {
            source = source.ToLower();
            int checker = 0;
            for (int i = 0; i < source.Length; i++)
            {
                int idx = source[i] - 'a';
                if ((checker & (1 << idx)) != 0)
                {
                    Console.WriteLine("false.");
                    return;
                }
                checker |= (1 << idx);
            }
            Console.WriteLine("true.");
        }
    }
}
