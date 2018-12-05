using System;
using System.IO;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Advent of Code 2018 - Day 4\n");

            // part 1
            Console.WriteLine("Part 1\n");

            var example = "dabAcCaCBAcCcaDA";
            var result = ReactPolymer(example);
            Console.WriteLine($"Example result: {result}");
            Console.WriteLine($"Example length: {result.Length}");

            var input = File.ReadAllText("input.txt");
            result = ReactPolymer(input);
            //Console.WriteLine($"Example result: {result}");
            Console.WriteLine($"Example length: {result.Length}");

            // part 2
            Console.WriteLine("\nPart 2\n");

            OptimizePolymer(example, out char exampleBestChar, out int exampleBestLength);
            Console.WriteLine("Example best Length: " + exampleBestLength);
            Console.WriteLine("Example best Char: " + exampleBestChar);

            OptimizePolymer(input, out char bestChar, out int bestLength);
            Console.WriteLine("\nBest Length: " + bestLength);
            Console.WriteLine("Best Char: " + bestChar);

        }

        private static string OptimizePolymer(string polymer, out char bestChar, out int bestLength)
        {
            var abc = "abcdefghijklmnopqrstuvxyz";
            var result = "";

            bestChar = '.';
            bestLength = int.MaxValue;

            foreach (char c in abc)
            {
                if (!polymer.Contains(c.ToString())) continue;

                Console.Write($"{c}: ");
                var op = polymer.Replace(c.ToString(), "").Replace(c.ToString().ToUpper(), "");
                result = ReactPolymer(op);
                //Console.Write(result);
                //Console.WriteLine(result.Length);

                if (result.Length < bestLength)
                {
                    bestLength = result.Length;
                    bestChar = c;
                }
            }

            return result;
        }

        private static string ReactPolymer(string s)
        {
            var repeat = true;

            while (repeat)
            {
                repeat = false;
                for (int i = 0; i < s.Length - 1; i++)
                {
                    var s1 = s[i];
                    var s2 = s[i + 1];
                    if ((Char.IsLower(s1) && Char.IsUpper(s2) && Char.ToUpper(s1) == s2) || Char.IsLower(s2) && Char.IsUpper(s1) && Char.ToUpper(s2) == s1)
                    {
                        s = s.Remove(i, 2);
                        repeat = true;
                        break;
                    }
                }
            }

            return s;
        }
    }
}