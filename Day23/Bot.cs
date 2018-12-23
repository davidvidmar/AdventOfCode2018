using System;
using System.Text.RegularExpressions;

namespace Day23
{
    class Bot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Radius { get; set; }

        public Bot(string s)
        {
            var regex = new Regex("^pos=<(.*),(.*),(.*)>, r=(.*)$");
            var g = regex.Match(s);
            if (g.Groups.Count == 5)
            {
                X = Int32.Parse(g.Groups[1].Value);
                Y = Int32.Parse(g.Groups[2].Value);
                Z = Int32.Parse(g.Groups[3].Value);
                Radius = Int32.Parse(g.Groups[4].Value);
            }
        }

        public bool InRange(Bot bot)
        {
            return DistanceTo(bot) <= bot.Radius;
        }

        public int DistanceTo(Bot bot)
        {
            return Math.Abs(bot.X - X) + Math.Abs(bot.Y - Y) + Math.Abs(bot.Z - Z);
        }

        public Bot(int x, int y, int z, int radius)
        {
            X = x;
            Y = y;
            Z = z;
            Radius = radius;
        }

        public override string ToString()
        {
            return $"pos=<{X},{Y},{Z}>, r={Radius}";
        }
    }
}
