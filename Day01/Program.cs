using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 1\n");

            // part 1
            Console.WriteLine("\nPart 1");

            // example
            int[] example = { +1, -2, +3, +1 };
            Console.WriteLine("Example 0 result: " + CalculateFrequency(0, example).ToString());

            // other examples
            int[] example1 = { +1, +1, +1 };
            int[] example2 = { +1, +1, -2 };
            int[] example3 = { -1, -2, -3 };

            Console.WriteLine("Example 1 result: " + CalculateFrequency(0, example1).ToString());
            Console.WriteLine("Example 2 result: " + CalculateFrequency(0, example2).ToString());
            Console.WriteLine("Example 3 result: " + CalculateFrequency(0, example3).ToString());

            var input = from q in File.ReadAllLines("input.txt") select Convert.ToInt32(q);
            var result = CalculateFrequency(0, input.ToArray());
            Console.WriteLine("\nResult: " + result);

            // part 2
            Console.WriteLine("\nPart 2");

            // example
            int[] part2_example = { +1, -1 };
            Console.Write("Example 0");
            Console.WriteLine(FindFirstDoubleFrequency(0, part2_example).ToString());

            // other examples
            int[] part2_example1 = { +3, +3, +4, -2, -4 };
            int[] part2_example2 = { -6, +3, +8, +5, -6 };
            int[] part2_example3 = { +7, +7, -2, -7, -4 };

            Console.Write("Example 1 result: ");
            Console.WriteLine(FindFirstDoubleFrequency(0, part2_example1).ToString());

            Console.Write("Example 2 result: ");
            Console.WriteLine(FindFirstDoubleFrequency(0, part2_example2).ToString());

            Console.Write("Example 3 result: ");
            Console.WriteLine(FindFirstDoubleFrequency(0, part2_example3).ToString());

            Console.Write("\nResult");
            int? result2 = FindFirstDoubleFrequency(0, input.ToArray());
            Console.WriteLine(result2);

        }

        private static int? FindFirstDoubleFrequency(int start, int[] changes)
        {
            var freqs = new List<int>();
            int? foundDuplicate = null;

            int result = start;

            while (foundDuplicate == null)
            {
                result = CalculateFrequency(result, changes, out foundDuplicate, freqs);
                Console.Write(".");
            }
            // Console.WriteLine("Duplicate Frequency: " + foundDuplicate);

            return foundDuplicate;
        }


        private static int CalculateFrequency(int start, int[] changes)
        {
            return CalculateFrequency(start, changes, out int? dummy);
        }

        private static int CalculateFrequency(int start, int[] changes, out int? foundDuplicate, List<int> freqs = null)
        {
            foundDuplicate = null;

            foreach (int c in changes)
            {
                // Console.Write(start + " ");
                start += c;
                if (freqs != null)
                    if (freqs.Contains(start))
                    {                        
                        foundDuplicate = start;
                        return start;
                    }
                    else
                    {
                        freqs.Add(start);
                    }
            }
            return start;
        }
    }
}
