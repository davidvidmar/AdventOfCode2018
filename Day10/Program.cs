using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 10\n");

            Console.WriteLine("Part1\n");

            var input = File.ReadAllLines("input.txt");
            var data = ParseInput(input);
            
            var step = 0;
            while (true)
            {
                data = Process(data);
                step++;
                if (step % 100 == 0)
                {
                    Console.WriteLine(step);
                }
                if (data.Max(d => d.X) - data.Min(d => d.X) < 100)
                {
                    Print(data, data.Min(d => d.X), data.Min(d => d.Y));
                    Console.WriteLine(step);
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                        return;
                }
            }
        }

        private static IEnumerable<Particle> Process(IEnumerable<Particle> particles)
        {            
            var list = new List<Particle>();
            foreach (var p in particles)
            {
                p.X += p.H;
                p.Y += p.V;
                list.Add(p);
            }
            return list;
        }

        private static IEnumerable<Particle> ParseInput(string[] text)
        {
            var regex = new Regex("position=<(.+), (.+)> velocity=<(.+), (.+)>");
            foreach (var line in text)
            {
                var g = regex.Match(line);
                yield return new Particle(g.Groups[1].Value, g.Groups[2].Value, g.Groups[3].Value, g.Groups[4].Value);
            }
        }   
        
        private static void Print(IEnumerable<Particle> particles, int minX, int minY)
        {
            Console.Clear();            
            foreach (var p in particles)
            {
                Console.SetCursorPosition(p.X - minX, 5 + p.Y - minY);
                Console.Write("#");                    
            }
            Console.SetCursorPosition(0, 0);
        }
    }

    class Particle
    {
        public Particle(int x, int y, int h, int v)
        {
            X = x;
            Y = Y;
            H = h;
            V = v;
        }

        public Particle(string x, string y, string h, string v)
        {
            X = Int32.Parse(x);
            Y = Int32.Parse(y);
            H = Int32.Parse(h);
            V = Int32.Parse(v);
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int H { get; private set; }
        public int V { get; private set; }
    }
}
