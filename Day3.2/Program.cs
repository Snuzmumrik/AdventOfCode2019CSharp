using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3._2
{
    class Program
    {
        public struct position
        {
            public bool wire1;
            //public int stepsW1;
            public bool wire2;
            //public int stepsW2;

        }

        public struct Coord
        {
            public int x;
            public int y;
        }


        static void Main(string[] args)
        {
            var gridSize = 25000;
            position[,] grid = new position[gridSize, gridSize];

            string[] inputLines = new string[2];
            var line = "";
            using (var sr = File.OpenText("./input.txt"))
            {
                var i = 0;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    inputLines[i] = line;
                    i++;
                }

            }
            string[] inputWire1 = inputLines[0].Split(',');
            string[] inputWire2 = inputLines[1].Split(',');



            Coord coord = new Coord();

            coord.x = gridSize / 2;
            coord.y = gridSize / 2;

            var prevStep = 0;
            for (var i = 0; i < inputWire1.Length; i++)
            {
                var direction = inputWire1[i].Substring(0, 1);
                var distance = Int32.Parse(inputWire1[i].Substring(1));
                for (var j = 0; j < distance; j++)
                {
                    switch (direction)
                    {
                        case "R":
                            coord.x++;
                            break;
                        case "L":
                            coord.x--;
                            break;
                        case "U":
                            coord.y++;
                            break;
                        case "D":
                            coord.y--;
                            break;
                        default:
                            break;
                    }
                    grid[coord.x, coord.y].wire1 = true;
                    //grid[coord.x, coord.y].stepsW1 = prevStep + 1;
                    //prevStep = grid[coord.x, coord.y].stepsW1;
                    // Console.WriteLine(prevStep);
                }
            }

            coord.x = gridSize / 2;
            coord.y = gridSize / 2;
            prevStep = 0;
            for (var i = 0; i < inputWire2.Length; i++)
            {
                var direction = inputWire2[i].Substring(0, 1);
                var distance = Int32.Parse(inputWire2[i].Substring(1));

                for (var j = 0; j < distance; j++)
                {
                    switch (direction)
                    {
                        case "R":
                            coord.x++;
                            break;
                        case "L":
                            coord.x--;
                            break;
                        case "U":
                            coord.y++;
                            break;
                        case "D":
                            coord.y--;
                            break;
                        default:
                            break;
                    }
                    grid[coord.x, coord.y].wire2 = true;
                    //grid[coord.x, coord.y].stepsW2 = prevStep + 1;
                    // prevStep = grid[coord.x, coord.y].stepsW2;
                    Console.WriteLine(prevStep);
                }
            }

            Console.WriteLine("Answer: " + FindClosestIntersect(grid, gridSize));
        }

        private static int FindClosestIntersect(position[,] grid, int gridSize)
        {
            List<Coord> crossings = new List<Coord>();

            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    if (grid[i, j].wire1 == true && grid[i, j].wire2 == true)
                    {
                        crossings.Add(new Coord() { x = i, y = j });
                    }
                }
            }

            List<int> manDist = new List<int>();
            foreach (var item in crossings)
            {
                manDist.Add(Math.Abs(gridSize / 2 - item.x) + Math.Abs(gridSize / 2 - item.y));
            }
            //var x = manDist.
            return manDist.Min();
        }


    }
}

