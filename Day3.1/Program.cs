using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3._1
{
    class Program
    {
        public struct position
        {
            public bool wire1;
            public int stepsW1;
            public bool wire2;
            public int stepsW2;

        }

        public struct Coord
        {
            public int x;
            public int y;
        }


        static void Main(string[] args)
        {
            position[,] grid = new position[25000, 25000];

            string[] input = File.ReadAllLines("input.txt");

            string[] inputWire1 = input[0].Split(',');
            string[] inputWire2 = input[1].Split(',');

            Coord coord = new Coord();

            coord.x = 12500;
            coord.y = 12500;

            for (var i = 0; i < inputWire1.Length; i++)
            {
                var direction = inputWire1[i].Substring(0, 1);
                var distance = Int64.Parse(inputWire1[i].Substring(1));

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
                    grid[coord.x, coord.y].stepsW1++;
                }
            }

            coord.x = 12500;
            coord.y = 12500;
            for (var i = 0; i < inputWire2.Length; i++)
            {
                var direction = inputWire2[i].Substring(0, 1);
                var distance = Int64.Parse(inputWire2[i].Substring(1));

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
                    grid[coord.x, coord.y].stepsW2++;
                    //Console.WriteLine(grid[coord.x, coord.y].stepsW2++);
                }
            }
           
            Console.WriteLine("Answer: " + FindClosestIntersect(grid));
        }

        private static int FindClosestIntersect(position[,] grid)
        {
            List<Coord> crossings = new List<Coord>();

            for (var i = 0; i < 25000; i++)
            {
                for (var j = 0; j < 25000; j++)
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
                manDist.Add(Math.Abs(12500 - item.x) + Math.Abs(12500 - item.y));
            }
            //var x = manDist.
            return manDist.Min();
        }


    }
}

