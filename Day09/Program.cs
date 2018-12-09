using System;
using System.Collections.Generic;
using System.Linq;

namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 8\n");

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
                var scoreExample = Play(playersExample, lastMarbleExample);

                Console.Write($"Players: {playersExample,3} | Last Marble: {lastMarbleExample,7} | Score: {highScoreExample,8}");
                Console.WriteLine($" | Calculated Score: {scoreExample,8} | Correct: {highScoreExample == scoreExample}");
            }

            var input = "468 players; last marble is worth 71843 points";
            var (players, lastMarble, highScore) = ParseInput(input);

            Console.Write($"\nPlayers: {players,3} | Last Marble: {lastMarble,7} | Score: {highScore,8}");
            var score = Play(players, lastMarble);
            Console.WriteLine($" | Calculated Score: {score,8} | Correct: {highScore == score}");

            // part 2
            
            Console.WriteLine("\nPart 2\n");

            input = "468 players; last marble is worth 7184300 points";
            (players, lastMarble, highScore) = ParseInput(input);

            Console.Write($"Players: {players,3} | Last Marble: {lastMarble,7} | Score: {highScore,8}");
            score = Play(players, lastMarble);
            Console.WriteLine($" | Calculated Score: {score,8} | Correct: {highScore == score}");
        }

        private static long Play(int players, int lastMarble)
        {
            long currentMarble = 1;
            var currentIndex = 1;
            var currentPlayer = 0;
            var nextMarble = 2;
            var turn = 2;
            var score = new long[players];
            var list = new List<long>(lastMarble) { 0, 1 };

            //PrintList(turn, currentPlayer, currentMarble, list);
            currentPlayer++;
            turn++;

            while (nextMarble <= lastMarble)
            {
                //var i = list.IndexOf(currentMarble);                

                if (nextMarble % 23 != 0)
                {
                    // between places 1 and 2 to the right
                    currentIndex = currentIndex + 2;
                    if (currentIndex > list.Count) currentIndex = currentIndex - list.Count;

                    // add and it becomes current marble
                    list.Insert(currentIndex, nextMarble);
                    currentMarble = nextMarble;
                }
                else
                {
                    // keep the marble
                    score[currentPlayer] += nextMarble;

                    // marble seven places to the left
                    int indexSevenLeft = currentIndex - 7;
                    if (indexSevenLeft < 0) indexSevenLeft += list.Count;
                    
                    // remove and keep seven places to the left                    
                    score[currentPlayer] += list[indexSevenLeft];
                    list.RemoveAt(indexSevenLeft);

                    currentMarble = list[indexSevenLeft];
                    currentIndex = indexSevenLeft;
                }

                //PrintList(turn, currentPlayer, currentMarble, list);

                turn++;                
                nextMarble++;

                currentPlayer++;
                if (currentPlayer >= players) currentPlayer = currentPlayer - players;

                if (nextMarble % 100000 == 0) Console.WriteLine(nextMarble);
            }

            var highScore = score.Max();
            //return (score.IndexOf(highScore), high/Score);
            return highScore;
        }

        private static void PrintList(int turn, int currentPlayer, long currentMarble, List<long> list)
        {
            //Console.Write($"{turn, 2} [{currentPlayer + 1}] ");
            Console.Write($"[{currentPlayer + 1}]");

            var lastCurrent = false;
            if (list.Count < 30) 
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == currentMarble)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        if (list[i] < 10) Console.Write(" ");                        
                        Console.Write($"({list[i]})");
                        Console.ResetColor();
                        lastCurrent = true;
                    }
                    else
                    {
                        if (!lastCurrent) Console.Write(" ");
                        lastCurrent = false;
                        Console.Write($"{list[i],2}");
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
