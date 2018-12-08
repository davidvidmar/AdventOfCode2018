using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2018 - Day 8\n");


            // part 1
            Console.WriteLine("Part 1\n");

            var example = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
            var exampleData = example.Split(new[] { ' ' }).Select(s => Int32.Parse(s)).ToArray();

            int checkSum = 0;
            CheckLicence1(ref exampleData, ref checkSum);
            Console.WriteLine("Example result: " + checkSum);

            checkSum = 0;
            var input = File.ReadAllText("input.txt");
            var inputData = input.Split(new[] { ' ' }).Select(s => Int32.Parse(s)).ToArray(); ;            
            CheckLicence1(ref inputData, ref checkSum);
            Console.WriteLine("Result: " + checkSum);

            // part 2
            Console.WriteLine("\nPart 2\n");
            
            exampleData = example.Split(new[] { ' ' }).Select(s => Int32.Parse(s)).ToArray();
            checkSum = CheckLicence2(ref exampleData);
            Console.WriteLine("Example result: " + checkSum);

            inputData = input.Split(new[] { ' ' }).Select(s => Int32.Parse(s)).ToArray(); ;
            checkSum = CheckLicence2(ref inputData);
            Console.WriteLine("Result: " + checkSum);
        }

        private static void CheckLicence1(ref int[] data, ref int metadataSum)
        {
            var child = data[0];
            var metadata = data[1];

            data = data.Skip(2).ToArray();
            for (int i = 0; i < child; i++)
            {                
                CheckLicence1(ref data, ref metadataSum);
            }

            for (int j = 0; j < metadata; j++)
            {
                metadataSum += data[j];                
            }
            data = data.Skip(metadata).ToArray();
        }

        private static int CheckLicence2(ref int[] data)
        {
            var child = data[0];
            var metadata = data[1];
            var nodeValues = new List<int>();
            var nodeValue = 0;

            data = data.Skip(2).ToArray();                       
                        
            for (int i = 0; i < child; i++)
            {                                
                nodeValues.Add(CheckLicence2(ref data));
            }

            if (child == 0)
            {
                for (int j = 0; j < metadata; j++)
                {
                    nodeValue += data[j];
                }
            }
            else
            {
                for (int j = 0; j < metadata; j++)
                {
                    if (data[j] == 0) continue;
                    if (data[j] > nodeValues.Count) continue;
                    nodeValue += nodeValues[data[j]-1];
                }
            }

            data = data.Skip(metadata).ToArray();

            return nodeValue;
        }
    }
}
