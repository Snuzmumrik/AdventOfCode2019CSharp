using System;
using System.IO;
using System.Collections.Generic;

namespace Day10._1
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "./input.txt";
            if (File.Exists(input))
            {
                string[] asteroidBelt = File.ReadAllLines(input);
                char[,] asteroidField = new char[asteroidBelt[1].Length, asteroidBelt.Length];
               
                MapAsteroids(asteroidBelt, asteroidField);

                FindBaseLocation(asteroidField);
            }
        }

        private static void FindBaseLocation(char[,] asteroidField)
        {
            for (int y = 0; y < asteroidField.Length; y++)
            {
                for (int x = 0; x < asteroidField.Length; x++)
                {
                    if(asteroidField[x, y] == '#')
                    {
                        var stationX = x;
                        var stationY = y;
                        FindVisibleAsteroids(stationX, stationY, asteroidField);
                    }
                }
            }
        }

        private static void FindVisibleAsteroids(int stationX, int stationY, char[,] asteroidField)
        {
            var visibleAsteroids = 0;

            //Någon form av delta linje
        }

        private static void MapAsteroids(string[] asteroidBelt, char[,] asteroidField)
        {
            for (int y = 0; y < asteroidBelt.Length; y++)
            {
                for (int x = 0; x < asteroidBelt[y].Length; x++)
                {
                    var asteroid = asteroidBelt[y][x];
                    asteroidField[x, y] = asteroid;
                }
            }
        }
    }
}
