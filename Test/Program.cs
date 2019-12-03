using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3_1
{
    class Program
    {

        struct pos
        {
            public bool wire1;
            public bool wire2;
            public int wire1dist;
            public int wire2dist;
        }

        struct coord
        {
            public int x;
            public int y;
        }

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            string[] inputWire1 = input[0].Split(',');
            string[] inputWire2 = input[1].Split(',');

            pos[,] grid = new pos[40000, 40000];

            coord turtle = new coord();

            int totalw1steps = 0;
            int totalw2steps = 0;

            turtle.x = 20000;
            turtle.y = 20000;
            for (int i = 0; i < inputWire1.Length; i++)
            {
                string direction = inputWire1[i].Substring(0, 1);
                int d = Convert.ToInt32(inputWire1[i].Substring(1));

                for (int j = 0; j < d; j++)
                {
                    switch (direction)
                    {
                        case "R":
                            turtle.x++;
                            break;

                        case "L":
                            turtle.x--;
                            break;

                        case "U":
                            turtle.y++;
                            break;

                        case "D":
                            turtle.y--;
                            break;

                        default:
                            break;
                    }

                    totalw1steps++;
                    grid[turtle.x, turtle.y].wire1 = true;

                    if (grid[turtle.x, turtle.y].wire1dist == 0)
                    {
                        grid[turtle.x, turtle.y].wire1dist = totalw1steps;
                    }
                }
            }


            turtle.x = 20000;
            turtle.y = 20000;
            for (int i = 0; i < inputWire2.Length; i++)
            {
                string direction = inputWire2[i].Substring(0, 1);
                int d = Convert.ToInt32(inputWire2[i].Substring(1));

                for (int j = 0; j < d; j++)
                {
                    switch (direction)
                    {
                        case "R":
                            turtle.x++;
                            break;

                        case "L":
                            turtle.x--;
                            break;

                        case "U":
                            turtle.y++;
                            break;

                        case "D":
                            turtle.y--;
                            break;

                        default:
                            break;
                    }

                    totalw2steps++;
                    grid[turtle.x, turtle.y].wire2 = true;

                    if (grid[turtle.x, turtle.y].wire2dist == 0)
                    {
                        grid[turtle.x, turtle.y].wire2dist = totalw2steps;
                    }
                }
            }


            List<coord> intersections = new List<coord>();
            List<int> intersectiondist = new List<int>();
            for (int i = 0; i < 40000; i++)
            {
                for (int j = 0; j < 40000; j++)
                {
                    if (grid[i, j].wire1 == true && grid[i, j].wire2 == true)
                    {
                        intersections.Add(new coord() { x = i, y = j });
                        intersectiondist.Add(grid[i, j].wire1dist + grid[i, j].wire2dist);
                    }
                }
            }

            //List<int> distance = new List<int>();
            //foreach (var item in intersections)
            //{
            //    distance.Add(Math.Abs(25000 - item.x) + Math.Abs(25000 - item.y));
            //}

            var x = intersectiondist.Min();



        }
    }
}
