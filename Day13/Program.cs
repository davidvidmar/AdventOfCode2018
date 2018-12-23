using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Day13
{
    class Program
    {
        static bool _print = false;
        static readonly bool _auto = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 13\n");

            var input = File.ReadAllLines("input.txt");
            var result = Play(input, true);

            Console.WriteLine($"\nResult: {result}");
            Console.ReadLine();
        }

        private static (int, int) Play(string[] field, bool getLast = false)
        {
            var tick = 0;
            
            // store original tracks without carts
            var trackOnly = (string[])field.Clone();
            for (int i = 0; i < trackOnly.Length; i++)            
                trackOnly[i] = trackOnly[i].Replace(">", "-").Replace("<", "-").Replace("v", "|").Replace("^", "|");

            var carts = new List<Cart>();

            Print(trackOnly, carts, tick);
            Debug.WriteLine($"{tick} - {NumberOfCarts(field)}");

            var moves = new List<Move>();
            int? row = null;
            int? col = null;                     

            while (true)
            {
                tick++;

                Print(field, carts, tick);

                moves.Clear();

                for (int rownNum = 0; rownNum < field.Length; rownNum++)
                {
                    string line = field[rownNum];
                    for (int colNum = 0; colNum < line.Length; colNum++)
                    {
                        var ch = line[colNum];
                        if (ch == '>')
                            moves.Add(MoveRight(trackOnly, carts, rownNum, colNum));
                        else if (ch == '<')
                            moves.Add(MoveLeft(trackOnly, carts, rownNum, colNum));
                        else if (ch == '^')
                            moves.Add(MoveUp(trackOnly, carts, rownNum, colNum));
                        else if (ch == 'v')
                            moves.Add(MoveDown(trackOnly, carts, rownNum, colNum));                        
                    }
                }
                var (r, c) = ProcessMoves(field, carts, moves, trackOnly, tick, getLast);                
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

        private static (int?, int?) ProcessMoves(string[] field, List<Cart> carts, List<Move> moves, string[] origField, int tick, bool getLast)
        {
            //if (_saveMoves)
            //    File.AppendAllLines("moves.txt", moves.Select(m => m.ToString()));

            //if (tick >= 4640 && tick <= 4650)
            //{
            //    Debug.WriteLine($"Tick: {tick} - left: {NumberOfCarts(field)}");
            //    File.WriteAllLines(tick + ".txt", field);
            //    File.AppendAllText("moves.txt", $"Tick: {tick}\n\n");
            //    File.AppendAllLines("moves.txt", moves.Select(m => m.ToString()));
            //}
            var crashes = new List<Move>();

            foreach (var move in moves)
            {
                if (crashes.Any(m => move.OldRow == m.NewRow && move.OldCol == m.NewCol))
                    continue;

                //if (tick > 1000 && move.ToString() == "v @ 81,46")
                //{
                //    _print = true;
                //    Print(field, carts, tick, 46);
                //}

                Debug.WriteLine(move);

                var newChar = move.NewChar;
                var newRow = field[move.NewRow].ToCharArray();
                var crash = false;
                
                // colission detection
                if ("<>^v".Contains(newRow[move.NewCol]))
                {
                    crashes.Add(move);

                    newChar = 'X'; 
                    crash = true;
                    if (getLast)
                    {
                        newChar = origField[move.NewRow][move.NewCol];
                        //Debug.WriteLine($"Tick: {tick} - crash @ {move.NewRow},{move.NewCol} left: {NumberOfCarts(field)}");
                        //Print(field, carts, tick, move.NewRow);
                        //File.WriteAllLines(tick + "-1.txt", field);                        
                    }
                }
                
                newRow[move.NewCol] = newChar;
                field[move.NewRow] = new string(newRow);

                var oldRow = field[move.OldRow].ToCharArray();
                oldRow[move.OldCol] = origField[move.OldRow][move.OldCol];
                field[move.OldRow] = new string(oldRow);

                Print(field, carts, tick);
                
                if (newChar == 'X') return (move.NewRow, move.NewCol);

                if (crash)
                {
                    Debug.WriteLine($"Tick: {tick} - crash @ {move.NewRow},{move.NewCol} left: {NumberOfCarts(field)}\n");
                    //File.WriteAllLines(tick + "-2.txt", field);
                    crash = false;
                }
                else
                {
                    var cart = FindCart(carts, move.OldRow, move.OldCol);
                    cart.Row = move.NewRow;
                    cart.Col = move.NewCol;
                }
            }

            if (NumberOfCarts(field) == 1)
            {
                File.WriteAllLines(tick + "-final.txt", field);
                var lastCart = FindFirstCart(field);
                Debug.WriteLine("Last cart: " + lastCart);
                return lastCart;
            }

            return (null, null);
        }

        private static int NumberOfCarts(string[] field)
        {
            return string.Join("", field).Count(c => "<>^v".Contains(c));
        }

        private static (int, int) FindFirstCart(string[] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                string row = field[i];
                var j = row.IndexOfAny("<>^v".ToCharArray());
                if (j >= 0) return (i, j);
            }
            return (-1, -1);
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

        private static void Print(string[] field, List<Cart> carts, int? tick, int highlightRow = -1)
        {
            //if (!_print && highlightRow == -1) return;
            if (!_print) return;

            Console.Clear();
            Console.WriteLine("Tick: " + tick);

            for (int i = 0; i < field.Length; i++)
            {
                string line = field[i];
                if (i == highlightRow) Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{i,3}: {line}");
                Console.ResetColor();
            }

            for (int i = 0; i < carts.Count; i++)
            {
                Console.WriteLine($"{i}: {carts[i]}");
            }

            if (_auto)
                Thread.Sleep(10);
            else
                Console.ReadKey();
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
            if (cart.Intersect == 4) cart.Intersect = 1;
            if (cart.Intersect == 1) // turn left
            {
                if (dir == 'L') return 'v';
                if (dir == 'U') return '<';
                if (dir == 'R') return '^';
                if (dir == 'D') return '>';
            }
            if (cart.Intersect == 2) // go straigh
            {
                if (dir == 'L') return '<';
                if (dir == 'U') return '^';
                if (dir == 'R') return '>';
                if (dir == 'D') return 'v';
            }
            if (cart.Intersect == 3) // turn right
            {
                if (dir == 'L') return '^';
                if (dir == 'U') return '>';
                if (dir == 'R') return 'v';
                if (dir == 'D') return '<';
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
            return $"{Col}, {Row} # {Intersect}";
        }        
    }

    class Move
    {        
        public int OldRow { get; set; }
        public int OldCol { get; set; }
                
        public int NewRow { get; set;  }
        public int NewCol { get; set; }        
        public char NewChar { get; set; }

        public Move(int oldRow, int oldCol, int newRow, int newCol, char newChar)
        {
            OldCol = oldCol;
            OldRow = oldRow;
            NewCol = newCol;
            NewRow = newRow;            
            NewChar = newChar;
        }

        public override string ToString()
        {
            return $"{NewChar} @ {NewCol},{NewRow}";
        }

    }
}