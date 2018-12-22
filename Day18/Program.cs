using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 18\n");

            var example = File.ReadAllLines("input.txt");

            var end = false;
            var minute = 0;

            Console.Clear();
            //Print(example, minute);
            //end = Console.ReadKey().Key == ConsoleKey.Escape;

            var history = new List<string>();

            while (!end)
            {
                minute++;
                Process(example);
                //if (minute % 1000 == 0)
                //Console.Clear();
                //Print(example, minute);
                //end = Console.ReadKey().Key == ConsoleKey.Escape;
                //end = minute == 1000;
                end = minute == 440;
                //Log(example, minute);
                var current = string.Join("*", example);
                var index = history.IndexOf(current);
                //Console.WriteLine($"{minute}: {index}");
                if (index < 0)
                {
                    history.Add(current);                    
                }
                //    end = true;
            }
            Print(example, minute);
            Console.WriteLine($"minute: {minute}, duplicated at: {history.IndexOf(string.Join("*", example))}");
            
        }

        private static void Log(string[] area, int minute)
        {
            var wood = Count(area, '|');
            var lumber = Count(area, '#');
            File.AppendAllText("log.txt", "minute: " + minute + " | "  + string.Join("*", area)+"\n");
        }

        private static void Process(string[] area)
        {
            var y = area.Length;
            var x = area[0].Length;

            var currentArea = (string[])area.Clone();

            for (int row = 0; row < x; row++)
            {
                var newRow = "";
                for (int col = 0; col < y; col++)
                {
                    newRow += Progress(row, col, currentArea);
                }
                area[row] = newRow;
            }            
        }

        private static char Progress(int row, int col, string[] area)
        {
            var now = area[row][col];

            // An open acre will become filled with trees if three or more adjacent acres contained trees. Otherwise, nothing happens.
            if (now == '.') // open
                return Count(row, col, area, '|') >= 3 ? '|' : now;

            // An acre filled with trees will become a lumberyard if three or more adjacent acres were lumberyards. Otherwise, nothing happens.
            if (now == '|') // tree
                return Count(row, col, area, '#') >= 3 ? '#' : now;

            // An acre containing a lumberyard will remain a lumberyard if it was adjacent to at least one other lumberyard and at least one acre containing trees. Otherwise, it becomes open.
            if (now == '#')
                return Count(row, col, area, '#') >= 1 && Count(row, col, area, '|') >= 1 ? now : '.';

            throw new Exception("invalid 'now'");
        }

        private static int Count(int row, int col, string[] area, char v)
        {
            var count = 0;
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    // center
                    if (i == 0 && j == 0) continue; 
                    // out of bounds
                    if (row + i < 0 || row + i >= area.Length) continue; 
                    if (col + j < 0 || col + j >= area[0].Length) continue;
                    // count
                    if (area[row + i][col + j] == v) count++;
                }
            return count;
        }

        private static int Count(string[] area, char v)
        {
            var count = 0;
            foreach (var row in area)
            {
                count += row.Count(c => c == v);
            }
            return count;
        }

        private static void Print(string[] area, int minute)
        {                        
            Console.WriteLine($"Minute: {minute}\n");
            foreach (var s in area)
            {
                Console.WriteLine(s);
            }
            var wood = Count(area, '|');
            var lumber = Count(area, '#');
            Console.WriteLine($"\n|: {wood}, #:{lumber}, result: {wood * lumber}");
        }
    }
}
