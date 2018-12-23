using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 23\n");

            Console.WriteLine("Part 1\n");

            var example = new[] { "pos=<0,0,0>, r=4",
                                   "pos=<1,0,0>, r=1",
                                   "pos=<4,0,0>, r=3",
                                   "pos=<0,2,0>, r=1",
                                   "pos=<0,5,0>, r=3",
                                   "pos=<0,0,3>, r=1",
                                   "pos=<1,1,1>, r=1",
                                   "pos=<1,1,2>, r=1",
                                   "pos=<1,3,1>, r=1" };

            var bots = Parse(example);
            var strongest = bots.OrderByDescending(b => b.Radius).First();
            Console.WriteLine("Example: " + bots.Count(b => b.InRange(strongest)));

            var input = File.ReadAllLines("input.txt");
            bots = Parse(input);
            strongest = bots.OrderByDescending(b => b.Radius).First();
            Console.WriteLine("Result: " + bots.Count(b => b.InRange(strongest)));

            Console.WriteLine("\nPart 2\n");

            example = new[] {
                "pos=<10,12,12>, r=2",
                "pos=<12,14,12>, r=2",
                "pos=<16,12,12>, r=4",
                "pos=<14,14,14>, r=6",
                "pos=<50,50,50>, r=200",
                "pos=<10,10,10>, r=5"
            };
            //bots = Parse(example);
            bots = Parse(input);

            //var divider = 10;
            var divider = 100000000;
            var factor = 10;
            int minx = 0, miny = 0, minz = 0, maxx = 0, maxy = 0, maxz = 0;
            while (true)
            {
                Console.WriteLine(divider + "...");
                var (mx, my, mz, max) = FindSpace(bots, divider, minx * factor, miny * factor, minz * factor, maxx * factor, maxy * factor, maxz * factor);

                minx = mx;
                miny = my;
                minz = mz;

                maxx = (mx + 1);
                maxy = (my + 1);
                maxz = (mz + 1);

                if (divider > factor)
                    divider /= factor;
                else if (divider > 1)
                    divider = 1;
                else
                    break;
            }
            Console.WriteLine(minx + miny + minz);
        }

        private static (int, int, int, int) FindSpace(List<Bot> bots, int divider, int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
        {
            var botsM = bots.Select(b => new Bot(b.X / divider, b.Y / divider, b.Z / divider, b.Radius / divider));

            if (minX == 0)
                minX = botsM.Min(b => b.X);
            if (minY == 0)
                minY = botsM.Min(b => b.Y);
            if (minZ == 0)
                minZ = botsM.Min(b => b.Z);

            if (maxX == 0)
                maxX = botsM.Max(b => b.X);
            if (maxY == 0)
                maxY = botsM.Max(b => b.Y);
            if (maxZ == 0)
                maxZ = botsM.Max(b => b.Z);            

            int max = 0;
            int mx = 0, my = 0, mz = 0;

            for (int x = minX; x < maxX; x++)
                for (int y = minY; y < maxY; y++)
                    for (int z = minZ; z < maxZ; z++)
                    {
                        var ir = botsM.Count(b => b.InRange(new Bot(x, y, z, b.Radius)));
                        if (max < ir)
                        {
                            mx = x;
                            my = y;
                            mz = z;
                            max = ir;
                        }
                    }
            Console.WriteLine((mx, my, mz, max).ToString());
            return (mx, my, mz, max);
        }

        private static List<Bot> Parse(string[] example)
        {
            var list = new List<Bot>();
            foreach (var item in example)
            {
                list.Add(new Bot(item));
            }
            return list;
        }
    }
}
