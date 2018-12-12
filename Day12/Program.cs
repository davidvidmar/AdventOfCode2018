using System;
using System.Collections.Generic;
using System.Linq;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 12\n");

            Console.WriteLine("Part 1\n");

            var initialState = "#..#.#..##......###...###";
            var rulesExample = new[] { "...## => #",
                                "..#.. => #",
                                ".#... => #",
                                ".#.#. => #",
                                ".#.## => #",
                                ".##.. => #",
                                ".#### => #",
                                "#.#.# => #",
                                "#.### => #",
                                "##.#. => #",
                                "##.## => #",
                                "###.. => #",
                                "###.# => #",
                                "####. => #"
                                };

            var rules = Parse(rulesExample);
            var gens = Generate(initialState, rules, 20);
            var result = Count(gens.Last(), (gens.Last().Length  - initialState.Length) / 2);
            Console.WriteLine($"Example result: {result}");

            initialState = "###....#..#..#......####.#..##..#..###......##.##..#...#.##.###.##.###.....#.###..#.#.##.#..#.#";
            var rulesInput = new[] { "..### => #",
                            //"..... => .",
                            //"..#.. => .",
                            //".###. => .",
                            "...## => #",
                            //"#.### => .",
                            "#.#.# => #",
                            //"##..# => .",
                            "##.## => #",
                            //"#...# => .",
                            //"..##. => .",
                            //"##.#. => .",
                            //"...#. => .",
                            "#..#. => #",
                            ".#### => #",
                            ".#..# => #",
                            "##... => #",
                            //".##.# => .",
                            //"....# => .",
                            //"#.... => .",
                            ".#.#. => #",
                            //".##.. => .",
                            "###.# => #",
                            //"####. => .",
                            "##### => #",
                            "#.##. => #",
                            ".#... => #",
                            ".#.## => #",
                            "###.. => #",
                            //"#..## => .",
                            "#.#.. => #",
                            //"..#.# => ."
            };

            rules = Parse(rulesInput);
            gens = Generate(initialState, rules, 500);
            result = Count(gens.Last(), (gens.Last().Length - initialState.Length) / 2);
            Console.WriteLine($"Result: {result}");
        }

        private static int Count(string state, int pad)
        {
            var result = 0;
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] == '#') result += i - pad;
            }
            return result;
        }

        private static List<string> Generate(string initialState, Dictionary<string, string> rules, long generations)
        {
            var gens = new List<string>
            {
                initialState
            };

            var state = ".." + new string('.', (int)generations) + initialState + ".." + new string('.', (int)generations);
            //Console.WriteLine($"{0,3}: {state}");
            for (int i = 0; i < generations; i++)
            {                               
                var newState = "..";
                var pos = 0;
                while (pos < state.Length - 4)
                {
                    var plant = ".";
                    foreach (var r in rules)
                    {
                        if (state.Substring(pos, r.Key.Length) == r.Key)
                        {
                            plant = r.Value;
                            //Console.WriteLine($"{r.Key} => {r.Value}");
                            break;
                        }
                    }
                    newState += plant;
                    pos++;
                }
                state = newState + "..";
                gens.Add(state);
                int start = state.IndexOf('#');
                int end = state.LastIndexOf("#");
                Console.WriteLine($"{i+1,3}: {Count(state, (int)generations + 2)}; {start}; {state.Substring(start, end - start)}");
                //Console.WriteLine($"{i+1,3}: {state}");
                //Console.WriteLine($"{i+1,3}: ");
            }
            return gens;
        }

        private static Dictionary<string, string> Parse(string[] rules)
        {
            return rules.Select(r => r.Split(new[] { " => " }, StringSplitOptions.RemoveEmptyEntries)).ToDictionary(r => r[0], r => r[1]);
        }
    }
}
