using System;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 2\n");

            // part 1
            Console.WriteLine("\nPart 1");

            string[] example = { "abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" };
            Console.WriteLine($"Example result: {CalcCheckSum(example)}");            

            var input = File.ReadAllLines("input.txt");
            Console.WriteLine($"Result: {CalcCheckSum(input)}");

            // part 2
            Console.WriteLine("\nPart 2");

            string[] part2_example = { "abcde", "fghij", "klmno", "pqrst", "fguij", "axcye", "wvxyz" };
            var result = CalcSimilarStrings(part2_example);
            Console.WriteLine($"\nResult: {result[0]} & {result[1]} => {result[2]}");

            Console.WriteLine("\n");
            result = CalcSimilarStrings(input);
            Console.WriteLine($"\nResult: {result[0]} & {result[1]} => {result[2]}");

        }

        private static int CalcCheckSum(string[] input)
        {
            var doubles = 0;
            var triples = 0;
            foreach (var line in input)
            {
                var r = CalculateDoublesTriples(line);
                if (r[0] > 0) doubles++;
                if (r[1] > 0) triples++;                
            }
            return doubles * triples;
        }

        private static int[] CalculateDoublesTriples(string s)
        {
            var result = s.GroupBy(c => c).OrderBy(c => c.Key).ToDictionary(grp => grp.Key, grp => grp.Count());

            var doubles = result.Where(c => c.Value == 2).Count();
            var triples = result.Where(c => c.Value == 3).Count();

            return new[] { doubles, triples };
        }

        private static string[] CalcSimilarStrings(string[] input) 
        {
            var bestDiff = int.MaxValue;
            var result1 = "";
            var result2 = "";
            var same = "";

            for (int i = 0; i < input.Length; i++)
                for (int j = i; j < input.Length; j++)
                {
                    if (i == j) continue;
                    var s = "";
                    var diff = CompareString(input[i], input[j], out s);

                    if (diff < bestDiff)
                    {
                        bestDiff = diff;
                        result1 = input[i];
                        result2 = input[j];
                        same = s;
                        Console.Write($"{input[i]} vs {input[j]}");
                        Console.WriteLine(" " + diff);
                    }
                }

            return new[] { result1, result2, same };
        }

        private static int CompareString(string s1, string s2, out string same)
        {
            if (s1.Length != s2.Length)
                throw new Exception("Not correct length.");

            same = "";
            var diff = 0;

            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] != s2[i])
                    diff++;
                else
                    same += s1[i];
            }

            return diff;
        }        
    } 
}
