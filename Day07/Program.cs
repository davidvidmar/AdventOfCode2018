using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 7\n");

            // part 1
            Console.WriteLine("Part 1\n");

            var example = new[]
            {
                "Step C must be finished before step A can begin.",
                "Step C must be finished before step F can begin.",
                "Step A must be finished before step B can begin.",
                "Step A must be finished before step D can begin.",
                "Step B must be finished before step E can begin.",
                "Step D must be finished before step E can begin.",
                "Step F must be finished before step E can begin."
            };
            var exampleData = ParseInput(example);
            var exampleSteps = CalcSteps(exampleData);
            Console.WriteLine("Example result: " + exampleSteps);

            var input = File.ReadAllLines("input.txt");
            var data = ParseInput(input);
            var steps = CalcSteps(data);
            Console.WriteLine("Result: " + steps);

            // part 2
            Console.WriteLine("\nPart 2\n");

            var exampleDuration = CalcStepsDuration(exampleData, 2);
            Console.WriteLine("Example duration " + exampleDuration);

            var duration = CalcStepsDuration(data, 5, 60);
            Console.WriteLine("Duration " + duration);
        }

        private static int CalcStepsDuration(string[] data, int numWorkers, int overhead = 0)
        {
            var workers = new Dictionary<int, WorkerState>();

            for (int i = 0; i < numWorkers; i++)
                workers.Add(i, new WorkerState());

            var seconds = 0;
            var done = false;
            var steps = "";

            while (!done)
            {
                for (int i = 0; i < numWorkers; i++)
                {                    
                    if (workers[i].SecondsLeft == 0)
                    {
                        data = data.Where(d => d[0] != workers[i].Name).ToArray();

                        var nextStep = data.Select(d => d[0]).OrderBy(a => a).FirstOrDefault(d => !data.Select(e => e[1]).Contains(d) && !workers.Any(w => w.Value.Name == d));
                        if (nextStep != '\0')
                        {
                            steps += nextStep;
                            workers[i].Name = nextStep;
                            workers[i].SecondsLeft = nextStep - 'A' + overhead;
                        }
                        else
                        {
                            workers[i].Name = '.';
                            workers[i].SecondsLeft = 0;
                        }
                    }                    
                    else
                    {
                        workers[i].SecondsLeft--;
                    }
                }
                seconds++;
                done = data.Length == 1 && workers.All(w => w.Value.SecondsLeft == 0);
            }
            seconds += data[0][1] - 'A' + 1 + overhead;
            Console.WriteLine(steps + '+' + data[0][1]);
            return seconds;
        }

        private static string CalcSteps(string[] data)
        {
            var result = "";
            while (data.Length > 1)
            {
                var nextStep = data.Select(d => d[0]).OrderBy(a => a).First(d => !data.Select(e => e[1]).Contains(d));
                result += nextStep;
                data = data.Where(d => d[0] != nextStep).ToArray();
            }
            result += data[0];
            return result;
        }

        private static string[] ParseInput(string[] input)
        {
            return input.Select(s => s.Substring(5, 1) + s.Substring(36, 1)).ToArray();            
        }
    }

    class WorkerState
    {
        public char Name;
        public int SecondsLeft;

        public override string ToString()
        {
            return $"{Name} - {SecondsLeft}s";
        }
    }
}
