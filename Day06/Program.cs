using System;
using System.Collections.Generic;
using System.IO;

namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 4\n");

            // part 1
            Console.WriteLine("Part 1\n");

            var example = new[] { "1, 1", "1, 6", "8, 3", "3, 4", "5, 5", "8, 9" };
            var result = CalcPart1(example, new int[10, 10]);
            Console.WriteLine("Example result: " + result);

            
            var input = File.ReadAllLines("input.txt");
            // izklopim, ker je poooooooooooočaaaaaaaaaaaaaaaasi
            //result = CalcPart1(input, new int[350, 350]);
            //Console.WriteLine("Result: " + result);

            // part 2
            Console.WriteLine("Part 2\n");

            result = CalcPart2(example, new int[10, 10], 32);
            Console.WriteLine("Example result: " + result);

            result = CalcPart2(input, new int[350, 350], 10000);
            Console.WriteLine("Result: " + result);

        }

        private static int CalcPart2(string[] input, int[,] field, int limit)
        {
            //// mark coordinates
            //for (int i = 0; i < input.Length; i++)
            //{
            //    var coord = input[i].Split(',');
            //    field[Convert.ToInt32(coord[0]), Convert.ToInt32(coord[1])] = -i - 1;                
            //}

            var maxX = field.GetLength(0);
            var maxY = field.GetLength(1);

            // poooooooooooočaaaaaaaaaaaaaaaasi
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    foreach (var inp in input)
                    {
                        var coord = inp.Split(',');
                        field[i, j] += Math.Abs(i - Convert.ToInt32(coord[0])) + Math.Abs(j - Convert.ToInt32(coord[1]));
                    }
                }
            }

            // preštej ostale in najdi največjo
            int result = 0;           
            for (int i = 0; i < maxX; i++)
                for (int j = 0; j < maxY; j++)
                    if (field[i, j] < limit)
                            result++;                

            return result;
        }

        private static int CalcPart1(string[] input, int[,] field)
        {
            var candidates = new List<int>();

            // mark coordinates
            for (int i = 0; i < input.Length; i++)
            {
                var coord = input[i].Split(',');
                field[Convert.ToInt32(coord[0]), Convert.ToInt32(coord[1])] = -i - 1;
                candidates.Add(i);
            }

            var maxX = field.GetLength(0);
            var maxY = field.GetLength(1);

            // poooooooooooočaaaaaaaaaaaaaaaasi
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    if (field[i, j] < 0) continue;
                    field[i, j] = CalcClosestPoint(i, j, field);                    
                }
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"{i}   ");
            }
            
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"       ");
            Console.SetCursorPosition(0, Console.CursorTop);
            //Print(field);

            // na robu so tiste z neskončnimi površinami
            for (int i = 0; i < maxX; i++)
            {
                candidates.Remove(Math.Abs(field[0, i]));
                candidates.Remove(Math.Abs(field[maxX - 1, i]));
                candidates.Remove(Math.Abs(field[i, 0]));
                candidates.Remove(Math.Abs(field[i, maxX - 1])); // !!
            }

            // preštej ostale in najdi največjo
            int maxAppears = 0;
            foreach (var c in candidates)
            {
                var appears = 0;
                for (int i = 0; i < maxX; i++)
                    for (int j = 0; j < maxY; j++)
                        if (Math.Abs(field[i, j]) == c)
                            appears++;
                if (appears > maxAppears) maxAppears = appears;
            }   

            return maxAppears;
        }

        private static int CalcClosestPoint(int x, int y, int[,] field)
        {
            var closest = 0;
            var minDist = Int32.MaxValue;

            var maxX = field.GetLength(0);
            var maxY = field.GetLength(1);

            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    var dist = Math.Abs(x - i) + Math.Abs(y - j);
                    if (dist == 0) continue;
                    if (dist > minDist) continue;

                    if (field[i, j] >= 0) continue;

                    if (dist == minDist)
                    {
                        //Console.WriteLine($"Tie @ {x}x{y}: {closest} vs {field[i, j]}, dist {dist}");
                        closest = 0;
                    }
                    else
                    {
                        closest = field[i, j];
                    }
                    minDist = dist;
                }
            }

            return -closest;
        }

        private static void Print(int[,] field)
        {
            for (int j = 0; j <= field.GetUpperBound(1); j++)
            {
                for (int i = 0; i <= field.GetUpperBound(0); i++)
                {
                    Console.ForegroundColor = field[i, j] < 0 ? ConsoleColor.Yellow : ConsoleColor.Red;
                    //var x = Math.Abs(field[i, j]);
                    var x = field[i, j];
                    Console.Write(x == 0 ? " . " : x.ToString(" # ;-# "));
                }
                Console.WriteLine("");
            }
            Console.ResetColor();
            Console.WriteLine("");            
        }
    }
}
 