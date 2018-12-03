using System;
using System.Collections.Generic;
using System.IO;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 3\n");

            // part 1
            Console.WriteLine("\nPart 1 & 2\n");

            var example = new[] { "#1 @ 1,3: 4x4", "#2 @ 3,1: 4x4", "#3 @ 5,5: 2x2" };
            Console.WriteLine($"Example result: {CalcOverlap(example)}");
            
            var input = File.ReadAllLines("input.txt");
            Console.WriteLine($"Result: {CalcOverlap(input)}");
        }

        private static int CalcOverlap(string[] claims)
        {
            int[,] cloth = new int[1000, 1000];

            List<string> noOverlap = new List<string>();
            List<string>[,] overlap = new List<string>[1000, 1000];            

            int count = 0;

            foreach (var claim in claims)
            {                
                Parse(claim, out string id, out int x, out int y, out int w, out int h);

                noOverlap.Add(id);

                for (int i = 0; i < w; i++)
                    for (int j = 0; j < h; j++)
                    {
                        cloth[x + i, y + j]++;
                        if (overlap[x + i, y + j] == null)
                        {
                            overlap[x + i, y + j] = new List<string>();
                        }
                        else
                        {
                            noOverlap.Remove(id);
                            foreach (var o in overlap[x + i, y + j])
                            {
                                if (noOverlap.Contains(o))
                                    noOverlap.Remove(o);
                            }
                        }
                        overlap[x + i, y + j].Add(id);                                                    
                    }
            }

            for (int i = 0; i < cloth.GetUpperBound(0); i++)
            {
                for (int j = 0; j < cloth.GetUpperBound(1); j++)
                {
                    if (cloth[i, j] > 1) count++;
                }
            }

            foreach (var n in noOverlap)
            {
                Console.WriteLine("No overlap: " + n);
            }

            return count;
        }

        private static void Parse(string c, out string id, out int x, out int y, out int h, out int w)
        {
            var s = c;
            id = s.Substring(0, s.IndexOf('@')).Trim();
            s = s.Remove(0, s.IndexOf("@") + 1).Trim();

            x = int.Parse(s.Substring(0, s.IndexOf(',')));
            s = s.Remove(0, s.IndexOf(",") + 1).Trim();

            y = int.Parse(s.Substring(0, s.IndexOf(':')));
            s = s.Remove(0, s.IndexOf(":") + 1).Trim();

            h = int.Parse(s.Substring(0, s.IndexOf('x')));
            s = s.Remove(0, s.IndexOf("x") + 1).Trim();

            w = int.Parse(s);
        }
    }
}
