using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Day13
{
    class Program
    {
        static readonly bool _print = true;
        static readonly bool _auto = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 13\n");

            Console.WriteLine("Part 1\n");

            var input = File.ReadAllLines("example4.txt");
            var result = Play(input);

            Console.WriteLine($"\nResult: {result}");

            //Console.WriteLine("Part 2\n");

            //result = 0;

            //Console.WriteLine($"Result: {result}");

        }

        private static (int, int) Play(string[] field)
        {
            var tick = 0;
            // store original tracks without carts
            var origField = (string[])field.Clone();
            for (int i = 0; i < origField.Length; i++)            
                origField[i] = origField[i].Replace(">", "-").Replace("<", "-").Replace("v", "|").Replace("^", "|");

            var carts = new List<Cart>();

            Print(origField, carts, tick);            

            var moves = new List<Move>();
            int? row = null;
            int? col = null;                     

            while (true)
            {
                Print(field, carts, tick++);
                moves.Clear();
                for (int i = 0; i < field.Length; i++)
                {
                    string line = field[i];
                    for (int j = 0; j < line.Length; j++)
                    {
                        var ch = line[j];
                        if (ch == '>')
                            moves.Add(MoveRight(origField, carts, i, j));
                        else if (ch == '<')
                            moves.Add(MoveLeft(origField, carts, i, j));
                        else if (ch == '^')
                            moves.Add(MoveUp(origField, carts, i, j));
                        else if (ch == 'v')
                            moves.Add(MoveDown(origField, carts, i, j));                        
                    }
                }
                var (r, c) = ProcessMoves(field, carts, moves, origField, tick);                
                if (r != null)
                {
                    row = r;
                    col = c;
                    break;
                }             
            }
            Print(field, carts, tick);
            return (col.Value, row.Value);
        }

        private static (int?, int?) ProcessMoves(string[] field, List<Cart> carts, List<Move> moves, string[] origField, int tick)
        {
            foreach (var move in moves)
            {                
                var newChar = move.NewChar;
                var newRow = field[move.NewRow].ToCharArray();

                // colission detection
                if ("<>^v".Contains(newRow[move.NewCol]))
                    newChar = 'X';
                
                newRow[move.NewCol] = newChar;
                field[move.NewRow] = new string(newRow);

                var oldRow = field[move.OldRow].ToCharArray();
                oldRow[move.OldCol] = origField[move.OldRow][move.OldCol];
                field[move.OldRow] = new string(oldRow);

                Print(field, carts, tick);
                
                if (newChar == 'X') return (move.NewRow, move.NewCol);

                var cart = FindCart(carts, move.OldRow, move.OldCol);
                cart.Row = move.NewRow;
                cart.Col = move.NewCol;
            }

            return (null, null);
        }

        private static Move MoveDown(string[] origField, List<Cart> carts, int rowNum, int colNum)
        {
            var nextField = origField[rowNum + 1][colNum];
            var nextMove = 'v';
            if (nextField == '\\') nextMove = '>';
            if (nextField == '/') nextMove = '<';
            if (nextField == '+') nextMove = NextIntersect('D', FindCart(carts, rowNum, colNum));
            return new Move(rowNum, colNum, rowNum + 1, colNum, nextMove);
        }

        private static Move MoveUp(string[] origField, List<Cart> carts, int rowNum, int colNum)
        {
            var nextField = origField[rowNum - 1][colNum];
            var nextMove = '^';
            if (nextField == '\\') nextMove = '<';
            if (nextField == '/') nextMove = '>';
            if (nextField == '+') nextMove = NextIntersect('U', FindCart(carts, rowNum, colNum));
            return new Move(rowNum, colNum, rowNum - 1, colNum, nextMove);
        }

        private static Move MoveLeft(string[] origField, List<Cart> carts, int rowNum, int colNum)
        {
            var nextField = origField[rowNum][colNum - 1];
            var nextMove = '<';
            if (nextField == '\\') nextMove = '^';
            if (nextField == '/') nextMove = 'v';
            if (nextField == '+') nextMove = NextIntersect('L', FindCart(carts, rowNum, colNum)); 
            return new Move(rowNum, colNum, rowNum, colNum - 1, nextMove);
        }

        private static Move MoveRight(string[] origField, List<Cart> carts, int rowNum, int colNum)
        {
            var nextField = origField[rowNum][colNum + 1];
            var nextMove = '>';
            if (nextField == '\\') nextMove = 'v';
            if (nextField == '/') nextMove = '^';
            if (nextField == '+') nextMove = NextIntersect('R', FindCart(carts, rowNum, colNum));
            return new Move(rowNum, colNum, rowNum, colNum + 1, nextMove);
        }

        private static void Print(string[] field, List<Cart> carts, int? tick)
        {            
            Console.SetCursorPosition(0, 2);

            Console.WriteLine("Tick: " + tick + "\n");

            foreach (var cart in carts)
                Console.WriteLine(cart + "  ");

            Console.WriteLine(new String(' ', Console.WindowWidth));                                

            if (_print)
            {
                foreach (var line in field)
                {
                    Console.WriteLine(line + new string(' ', Console.WindowWidth - line.Length - 1));
                }

                if (_auto)
                    Thread.Sleep(10);
                else
                    Console.ReadKey();
            }
        }
       
        private static Cart FindCart(List<Cart> carts, int row, int col)
        {
            foreach (var cart in carts)
            {
                if (cart.Row == row && cart.Col == col)
                    return cart;                    
            }
            var newCart = new Cart
            {
                Row = row,
                Col = col
            };
            carts.Add(newCart);
            return newCart;
        }

        private static char NextIntersect(char dir, Cart cart)
        {
            cart.Intersect++;
            if (cart.Intersect == 5) cart.Intersect = 1;
            if (cart.Intersect == 1) // left
            {
                if (dir == 'L') return 'v';
                if (dir == 'U') return '<';
                if (dir == 'R') return '^';
                if (dir == 'D') return '>';
            }
            if (cart.Intersect == 2) // straigh
            {
                if (dir == 'L') return '<';
                if (dir == 'U') return '^';
                if (dir == 'R') return '>';
                if (dir == 'D') return 'v';
            }
            if (cart.Intersect == 3) // right
            {
                if (dir == 'L') return '^';
                if (dir == 'U') return '>';
                if (dir == 'R') return 'v';
                if (dir == 'D') return '<';
            }
            if (cart.Intersect == 4) // straight
            {
                if (dir == 'L') return '<';
                if (dir == 'U') return '^';
                if (dir == 'R') return '>';
                if (dir == 'D') return 'v';
            }
            throw new Exception();
        }
    }

    class Cart
    {
        public int Id { get; set; }

        public int Row { get; set; }
        public int Col { get; set; }

        public int Intersect { get; set; }

        public override string ToString()
        {
            return $"@{Col}x{Row}; {Intersect}";
        }
    }

    class Move
    {        
        public int OldRow { get; set; }
        public int OldCol { get; set; }
        //public char OldChar { get; set; }
                
        public int NewRow { get; set;  }
        public int NewCol { get; set; }        
        public char NewChar { get; set; }

        public Move(int oldRow, int oldCol, /*char oldChar, */int newRow, int newCol, char newChar)
        {
            OldCol = oldCol;
            OldRow = oldRow;
            //OldChar = oldChar;
            NewCol = newCol;
            NewRow = newRow;            
            NewChar = newChar;
        }

        public override string ToString()
        {
            return $"@{OldRow}, {OldCol} | {NewRow}, {NewCol} @ {NewChar}";
        }

    }
}