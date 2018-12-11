using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 11\n");

            Console.WriteLine("Part 1\n");

            var examples = new[] { "3,5,8,4", "122,79,57,-5", "217,196,39,0", "101,153,71,4" };

            foreach (var example in examples)
            {
                var i = example.Split(',').ToArray().Select(q => int.Parse(q)).ToArray();
                Console.WriteLine($"{GetCellPower(i[0], i[1], i[2])} == {i[3]}");
            }

            var example1 = GetGrid(300, 18);
            var (x, y, maxPower) = GetMaxSquare(example1);
            Console.WriteLine($"{x}, {y} -> {maxPower}");

            var example2 = GetGrid(300, 42);
            (x, y, maxPower) = GetMaxSquare(example2);
            Console.WriteLine($"{x}, {y} -> {maxPower}");

            var input = GetGrid(300, 5535);
            (x, y, maxPower) = GetMaxSquare(input);
            Console.WriteLine($"{x}, {y} -> {maxPower}");

            Console.WriteLine("\n\nPart 2\n");

            var maxSize = 0;

            //(x, y, maxSize, maxPower) = GetMaxSquareVar(example1);
            //Console.WriteLine($"{x}, {y}, {maxSize} -> {maxPower}");

            //(x, y, maxSize, maxPower) = GetMaxSquareVar(example2);
            //Console.WriteLine($"{x}, {y}, {maxSize} -> {maxPower}");

            (x, y, maxSize, maxPower) = GetMaxSquareVar(input);
            Console.WriteLine($"{x}, {y}, {maxSize} -> {maxPower}");
        }

        private static (int, int, int, int) GetMaxSquareVar(int[,] input)
        {
            var maxVarPower = 0;
            var (maxX, maxY) = (0, 0);
            var maxSize = 0;

            for (int size = 1; size <= 300; size++)
            //for (int size = 300; size >= 1; size--)
            {
                var (x, y, maxPower) = GetMaxSquare(input, size);
                if (maxPower > maxVarPower)
                {
                    maxVarPower = maxPower;
                    maxX = x;
                    maxY = y;
                    maxSize = size;
                }
                Console.WriteLine($"{size}: {maxX}, {maxY}, {maxSize} -> {maxVarPower}");
            }
            return (maxX, maxY, maxSize, maxVarPower);            
        }

        private static (int, int, int) GetMaxSquare(int[,] grid, int sumSize = 3)
        {
            var maxPower = 0;
            var x = 0;
            var y = 0;

            var size = grid.GetLength(0);
            
            for (int i = 0; i < size - sumSize; i++)
            {
                for (int j = 0; j < size - sumSize; j++)
                {
                    //if (grid[i, j] > 0)
                    {
                        var sum = SumXxX(grid, i, j, sumSize);
                        if (sum > maxPower) { maxPower = sum; x = i; y = j; }
                    }
                }
            }
            return (x, y, maxPower);
        }

        private static int SumXxX(int[,] grid, int x, int y, int sumSize)
        {
            var sum = 0;
            //Parallel.For<int>(0, sumSize,
            //    () => 0,
            //    (i, loop, subtotal) =>
            //    {
            //        for (int j = 0; j < sumSize; j++)
            //        {                        
            //            subtotal += grid[x + j, y + i];
            //        }                        
            //        return subtotal;
            //    },
            //    (st) => Interlocked.Add(ref sum, st)
            //);
            for (int i = 0; i < sumSize; i++)
            {                
                for (int j = 0; j < sumSize; j++)
                {
                    sum += grid[x + j, y + i];
                }
            }
            return sum;
        }

        private static int[,] GetGrid(int size, int serial)
        {
            int[,] grid = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = GetCellPower(i, j, serial);
                }
            }
            return grid;
        }

        private static int GetCellPower(int x, int y, int serial)
        {
            var rackID = x + 10;
            var step4 = ((rackID * y + serial) * rackID).ToString("000");
            return Int32.Parse(step4[step4.Length - 3] + "") - 5;
        }
    }
}

