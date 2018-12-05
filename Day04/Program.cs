using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 4\n");

            // part 1
            Console.WriteLine("Part 1\n");

            string[] example = new[] {
                "[1518-11-01 00:00] Guard #10 begins shift",
                "[1518-11-01 00:05] falls asleep",
                "[1518-11-01 00:25] wakes up",
                "[1518-11-01 00:30] falls asleep",
                "[1518-11-01 00:55] wakes up",
                "[1518-11-01 23:58] Guard #99 begins shift",
                "[1518-11-02 00:40] falls asleep",
                "[1518-11-02 00:50] wakes up",
                "[1518-11-03 00:05] Guard #10 begins shift",
                "[1518-11-03 00:24] falls asleep",
                "[1518-11-03 00:29] wakes up",
                "[1518-11-04 00:02] Guard #99 begins shift",
                "[1518-11-04 00:36] falls asleep",
                "[1518-11-04 00:46] wakes up",
                "[1518-11-05 00:03] Guard #99 begins shift",
                "[1518-11-05 00:45] falls asleep",
                "[1518-11-05 00:55] wakes up"
            };

            var guardTimesExample = ParseInput(example);

            //foreach (var gt in guardTimes)
            //{
            //    Console.WriteLine(gt.ToString());
            //}

            var id = CalcGuardMostAssleep(guardTimesExample);
            Console.WriteLine($"Example guard id most asleep: {id}");

            var mins = CalcMinuteMostAsspleep(guardTimesExample, id);
            Console.WriteLine($"Example guard most asleep minutes: {mins}");
            Console.WriteLine($"Result: {id*mins}");

            var input = File.ReadAllLines("input.txt").OrderBy(s => s).ToArray(); ;

            var guardTimes = ParseInput(input);

            //foreach (var gt in guardTimes)
            //{
            //    Console.WriteLine(gt.ToString());
            //}

            id = CalcGuardMostAssleep(guardTimes);

            Console.WriteLine($"Guard most asleep: {id}");

            mins = CalcMinuteMostAsspleep(guardTimes, id);
            Console.WriteLine($"Example guard most asleep minutes: {mins}");
            Console.WriteLine($"Result: {id * mins}");

            // part 2

            Console.WriteLine("\nPart 1\n");

            var mma = CalcMinuteMostAssleep(guardTimesExample);
            Console.WriteLine($"Example guard #{mma[0]} has slept most in min {mma[1]}. {mma[2]} times.");
            Console.WriteLine($"Result: {mma[0] * mma[1]}");

            mma = CalcMinuteMostAssleep(guardTimes);
            Console.WriteLine($"Guard #{mma[0]} has slept most in min {mma[1]}. {mma[2]} times.");
            Console.WriteLine($"Result: {mma[0] * mma[1]}");

        }

        private static int CalcGuardMostAssleep(List<GuardTime> guardTimes)
        {
            var r = from gt in guardTimes
                    group gt.Minutes by gt.ID into m
                    select new { ID = m.Key, Minutes = m.Select(q => q.Count(c => c == '#')).Sum() };

            var guards = r.OrderByDescending(g => g.Minutes);
            var topGuard = guards.First();

            return topGuard.ID;
        }

        private static int CalcMinuteMostAsspleep(List<GuardTime> guardTimes, int id)
        {
            // Strategy 1: Find the guard that has the most minutes asleep. What minute does that guard spend asleep the most?

            var ma = from gt in guardTimes
                     where gt.ID == id
                     select gt.MinutesArray;

            //var r2 = r.Zip(c => c).GroupBy(c => c.).OrderByDescending(c => c.Key).ToDictionary(grp => grp.Key, grp => grp.Count()).First();
            var minutes = new List<int>();
            foreach (var m in ma)
                minutes.AddRange(m);

            return minutes.GroupBy(m => m).ToDictionary(grp => grp.Key, grp => grp.Count()).OrderByDescending(m => m.Value).Select(m => m.Key).First();
        }

        private static int[] CalcMinuteMostAssleep(List<GuardTime> guardTimes)
        {
            // Strategy 2: Of all guards, which guard is most frequently asleep on the same minute?

            var gm = new List<GuardMinutes>();

            foreach (var gt in guardTimes)
            {
                var existing = gm.FirstOrDefault(g => g.ID == gt.ID);
                if (existing == null)
                {
                    existing = new GuardMinutes(gt.ID);
                    gm.Add(existing);
                }
                existing.Minutes.AddRange(gt.MinutesArray);
            }

            var topCount = 0;
            var topMin = 0;
            var topID = 0;

            foreach (var gt in gm)
            {
                if (gt.Minutes.Count == 0) continue;
                var r = gt.Minutes.GroupBy(m => m).ToDictionary(grp => grp.Key, grp => grp.Count()).OrderByDescending(m => m.Value).First();
                if (r.Value > topCount)
                {
                    topID = gt.ID;                    
                    topMin = r.Key;
                    topCount = r.Value;
                }
            }

            return new[] { topID, topMin, topCount };
        }        

        private static List<GuardTime> ParseInput(string[] example)
        {
            var list = new List<GuardTime>();
            GuardTime item = null;

            //DateTime date = DateTime.MinValue;
            string guard = "";
            DateTime date = DateTime.MinValue;
            int firstMinute = 0;

            foreach (var line in example)
            {
                var nextDate = DateTime.Parse(line.Substring(1, 16));
                if (nextDate.Hour == 23) nextDate = nextDate.AddDays(1); // ??

                var s = line.Substring(19);
                if (s.StartsWith("Guard #"))
                {
                    var nextGuard = s.Substring(7);
                    nextGuard = nextGuard.Substring(0, nextGuard.IndexOf(" "));

                    if (guard != nextGuard || date != nextDate)
                    {
                        item = new GuardTime(nextDate, int.Parse(nextGuard));
                        guard = nextGuard;
                        date = nextDate;
                        list.Add(item);
                    }
                }

                if (guard == "") throw new Exception("Unknown guard");
                if (item == null) throw new Exception("No item");
                
                if (s.StartsWith("falls asleep")) {
                    item.Minutes[nextDate.Minute] = '#';
                    firstMinute = nextDate.Minute;
                }
                if (s.StartsWith("wakes up"))
                {
                    for (int i = firstMinute; i < nextDate.Minute; i++)
                        item.Minutes[i] = '#';                
                };
            }

            return list;
        }

        private class GuardTime
        {
            public DateTime Date { get; private set; }
            public int ID { get; private set; }
            public char[] Minutes { get; private set; }

            public int SleepMinutes
            {
                get
                {
                    return Minutes.Count(m => m == '#');
                }
            }

            public IEnumerable<int> MinutesArray
            {
                get
                {
                    for (int i = 0; i < Minutes.Length; i++)
                    {
                        if (Minutes[i] != '.')
                            yield return i;
                    }
                }
            }


            public GuardTime(DateTime date, int id)
            {
                Date = date;
                ID = id;
                Minutes = new string('.', 60).ToCharArray();                
            }

            public override string ToString()
            {
                return $"[{Date.ToString("MM-dd")}]   {ID:0000}  {new string(Minutes)} ({SleepMinutes})";
            }
        }

        class GuardMinutes
        {
            public int ID { get; private set; }
            public List<int> Minutes { get; private set; }
            public int TopMinute { get; set; }

            public GuardMinutes(int ID)
            {
                this.ID = ID;
                Minutes = new List<int>();
            }

        }

    }
}
