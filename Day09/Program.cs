using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 9\n");

            // part 1

            Console.WriteLine("Part 1\n");

            var examples = new[] {
                "9 players; last marble is worth 25 points: high score is 32",

                "10 players; last marble is worth 1618 points: high score is 8317",
                "13 players; last marble is worth 7999 points: high score is 146373",
                "17 players; last marble is worth 1104 points: high score is 2764",
                "21 players; last marble is worth 6111 points: high score is 54718",
                "30 players; last marble is worth 5807 points: high score is 37305"
            };

            foreach (var example in examples)
            {
                var (playersExample, lastMarbleExample, highScoreExample) = ParseInput(example);
                var scoreExample = Play(playersExample, lastMarbleExample, playersExample < 10);

                Console.Write($"Players: {playersExample,3} | Last Marble: {lastMarbleExample,7} | Score: {highScoreExample, 7}");
                Console.WriteLine($" | Calculated Score: {scoreExample,12} | Correct: {highScoreExample == scoreExample}");
            }

            var input = "468 players; last marble is worth 71843 points";
            var (players, lastMarble, highScore) = ParseInput(input);

            Console.Write($"\nPlayers: {players,3} | Last Marble: {lastMarble,7} | Score: {highScore,7}");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var score = Play(players, lastMarble);
            stopwatch.Stop();
            Console.WriteLine($" | Calculated Score: {score,12} | Elapsed: {stopwatch.ElapsedMilliseconds} ms.");            

            // part 2

            Console.WriteLine("\nPart 2\n");

            input = "468 players; last marble is worth 7184300 points";
            (players, lastMarble, highScore) = ParseInput(input);

            Console.Write($"Players: {players,3} | Last Marble: {lastMarble,7} | Score: {highScore,7}");
            
            stopwatch.Start();
            score = Play(players, lastMarble);
            stopwatch.Stop();
            Console.WriteLine($" | Calculated Score: {score,12} | Elapsed: {stopwatch.ElapsedMilliseconds} ms.");            
        }

        private static long Play(int players, int lastMarble, bool print = false)
        {
            var currentPlayer = 0;
            var nextMarble = 0;
            
            var score = new long[players];
            var list = new LinkedList<long>();

            var currentNode = list.AddFirst(nextMarble++);
            currentNode = list.AddAfter(currentNode, nextMarble++);

            currentPlayer++;

            if (print) PrintList(currentPlayer, currentNode.Value, list);

            while (nextMarble <= lastMarble)
            {
                if (nextMarble % 23 != 0)
                {
                    // move right or wrap around
                    currentNode = currentNode.Next ?? list.First;                    
                    // add new
                    currentNode = list.AddAfter(currentNode, nextMarble);                    
                }
                else
                {
                    // keep the marble
                    score[currentPlayer] += nextMarble;

                    // marble seven places to the left
                    for (int i = 0; i < 7; i++)
                        currentNode = currentNode.Previous ?? list.Last;

                    // add to score remove and keep seven places to the left                    
                    score[currentPlayer] += currentNode.Value;
                    var next = currentNode.Next ?? list.First;
                    list.Remove(currentNode);
                    currentNode = next;                    
                }

                if (print) PrintList(currentPlayer, currentNode.Value, list);

                nextMarble++;
                currentPlayer++;
                if (currentPlayer >= players) currentPlayer = currentPlayer - players;
            }

            if (print) Console.WriteLine("");

            var highScore = score.Max();
            //return (score.IndexOf(highScore), high/Score);
            return highScore;
        }

        private static void PrintList(int currentPlayer, long currentMarble, LinkedList<long> list)
        {
            //Console.Write($"{turn, 2} [{currentPlayer + 1}] ");
            Console.Write($"[{currentPlayer + 1}]");

            var lastCurrent = false;
            if (list.Count < 30)
                foreach (var item in list)
                {                
                    if (item == currentMarble)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        if (item < 10) Console.Write(" ");
                        Console.Write($"({item})");
                        Console.ResetColor();
                        lastCurrent = true;
                    }
                    else
                    {
                        if (!lastCurrent) Console.Write(" ");
                        lastCurrent = false;
                        Console.Write($"{item,2}");
                    }
                }

            Console.WriteLine();
        }

        private static (int, int, int) ParseInput(string s)
        {
            var player = s.Substring(0, s.IndexOf(' '));
            s = s.Remove(0, s.IndexOf("worth") + 6);
            var points = s.Substring(0, s.IndexOf(' '));
            if (s.IndexOf("score") >= 0)
                s = s.Remove(0, s.IndexOf("is") + 3);
            else
                s = "0";
            var score = s;
            return (int.Parse(player), int.Parse(points), int.Parse(score));
        }
    }

}