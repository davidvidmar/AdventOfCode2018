using System;

namespace Day22
{
    class Program
    {
        readonly static char rocky = '.';
        readonly static char wet = '=';
        readonly static char narrow = '|';

        readonly static int m = 20183;

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 22\n");

            Console.WriteLine("Part 1\n");
            Console.WriteLine("Example: ");

            var depth = 510;
            var targetX = 10;
            var targetY = 10;

            var el = GetCave(depth, targetX + 5, targetY + 5, targetX, targetY);
            Print(el, targetX, targetY);

            Console.WriteLine("\nInput: ");
            depth = 11394;
            targetX = 7;
            targetY = 701;

            el = GetCave(depth, targetX, targetY, targetX, targetY);
            Print(el, targetX, targetY, true);
        }

        private static int[,] GetCave(int depth, int x, int y, int tx, int ty)
        {
            var gi = new int[x + 1, y + 1];
            var el = new int[x + 1, y + 1];

            // just checking....
            for (int i = 0; i <= x; i++)
                for (int j = 0; j <= y; j++)
                {
                    gi[i, j] = -1;
                    el[i, j] = -1;
                }

            // The region at 0,0(the mouth of the cave) has a geologic index of 0.
            // If the region's Y coordinate is 0, the geologic index is its X coordinate times 16807.                
            for (int i = 0; i <= x; i++)
            {
                gi[i, 0] = i * 16807;
                el[i, 0] = (gi[i, 0] + depth) % m;
            }

            // If the region's X coordinate is 0, the geologic index is its Y coordinate times 48271.
            for (int j = 0; j <= y; j++)
            {
                gi[0, j] = j * 48271;
                el[0, j] = (gi[0, j] + depth) % m;
            }

            for (int i = 1; i <= x; i++)
                for (int j = 1; j <= y; j++)
                {
                    if (i == tx && j == ty)
                        // The region at the coordinates of the target has a geologic index of 0.
                        gi[i, j] = 0;
                    else
                        // Otherwise, the region's geologic index is the result of multiplying the erosion levels of the regions at X-1,Y and X,Y-1.
                        checked
                        {
                            gi[i, j] = el[i - 1, j] * el[i, j - 1];
                        }

                    el[i, j] = (gi[i, j] + depth) % m;
                }

            return el;
        }

        private static void Print(int[,] el, int tx, int ty, bool printCave = true)
        {
            var x = el.GetLength(0);
            var y = el.GetLength(1);

            int sum = 0;

            for (int j = 0; j < y; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    var mod = el[i, j] % 3;
                    if (i <= tx && j <= ty)
                        sum += mod;
                    if (printCave)
                    {
                        if (i <= tx && j <= ty)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        if (i == 0 && j == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write('M');
                        }
                        else if (i == tx && j == ty)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write('T');

                        }
                        else if (mod == 0) Console.Write(rocky);
                        else if (mod == 1) Console.Write(wet);
                        else if (mod == 2) Console.Write(narrow);
                        Console.ResetColor();
                    }
                }
                if (printCave)
                    Console.WriteLine();
            }
            Console.WriteLine(sum);
        }
    }
}