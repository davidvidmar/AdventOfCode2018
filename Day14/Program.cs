using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 14\n");

            Console.WriteLine("Part 1\n");

            var result = GetResultPart1(9);
            Console.WriteLine("9: " + string.Join("", result.ToArray()));

            result = GetResultPart1(5);
            Console.WriteLine("5: " + string.Join("", result.ToArray()));

            result = GetResultPart1(18);
            Console.WriteLine("18: " + string.Join("", result.ToArray()));

            result = GetResultPart1(2018);
            Console.WriteLine("2018: " + string.Join("", result.ToArray()));

            result = GetResultPart1(824501);
            Console.WriteLine("824501: " + string.Join("", result.ToArray()));

            Console.WriteLine("\nPart 2\n");

            Console.WriteLine("51589: " + GetResultPart2("51589"));
            Console.WriteLine("01245: " + GetResultPart2("01245"));
            Console.WriteLine("92510: " + GetResultPart2("92510"));
            Console.WriteLine("59414: " + GetResultPart2("59414"));

            Console.WriteLine("824501: " + GetResultPart2("824501"));
        }

        private static IEnumerable<int> GetResultPart1(int stopAfter)
        {
            var list = new LinkedList<int>();

            var first = list.AddFirst(3);
            var second = list.AddAfter(first, 7);

            //Print(list, first, second);

            int recepies = 2;
            while (recepies < stopAfter + 10)
            {
                var newRecipe = first.Value + second.Value;
                foreach (var c in newRecipe.ToString())
                {
                    list.AddLast(Int32.Parse(c.ToString()));
                    recepies++;
                }
                first = Move(list, first);
                second = Move(list, second);
                //Print(list, first, second);
                //Console.ReadKey();
            }

            return list.Skip(stopAfter).Take(10);
        }

        private static int GetResultPart2(string stopIfFound)
        {
            var list = new LinkedList<int>();

            var first = list.AddFirst(3);
            var second = list.AddAfter(first, 7);

            //Print(list, first, second);

            int recepies = 2;
            var lastFive = "";
            //while (!string.Join("", list).Contains(stopIfFound))
            while (true)
            {
                var newRecipe = first.Value + second.Value;
                foreach (var c in newRecipe.ToString())
                {
                    lastFive += c;
                    lastFive = lastFive.Substring(Math.Max(lastFive.Length - stopIfFound.Length, 0));
                    list.AddLast(Int32.Parse(c.ToString()));
                    recepies++;

                    //if (recepies % 1000000 == 0) Console.WriteLine(recepies + ":" + lastFive);
                    if (lastFive == stopIfFound)
                        return recepies - stopIfFound.Length;
                }
                first = Move(list, first);
                second = Move(list, second);
                
                //Print(list, first, second);                
                //Console.ReadKey();
            }
        }

        private static void Print(LinkedList<int> list, LinkedListNode<int> first, LinkedListNode<int> second)
        {
            var node = list.First;           
            while (node != null)
            {
                if (node == first) Console.Write("(");
                else if (node == second) Console.Write("[");
                else Console.Write(" ");
                        
                Console.Write($"{node.Value,2}");

                if (node == first) Console.Write(")");
                else if (node == second) Console.Write("]");
                else Console.Write(" ");

                node = node.Next;
            }
            Console.WriteLine();
        }

        private static LinkedListNode<int> Move(LinkedList<int> list, LinkedListNode<int> node)
        {
            var moveSteps = node.Value + 1;
            for (int i = 0; i < moveSteps; i++)
            {
                node = node.Next;
                if (node == null) node = list.First;
            }
            return node;
        }
    }
}
