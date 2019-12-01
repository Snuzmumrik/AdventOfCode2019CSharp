using System;
using System.IO;

namespace Day1
{
    class Day1
    {
        static void Main(string[] args)
        {
            var fuelReq = 0;
            using (StreamReader input = File.OpenText("./input.txt"))
            {
                while (!input.EndOfStream)
                {
                    var line = input.ReadLine();
                    int.TryParse(line, out var mass);
                    fuelReq += (int)Math.Floor(mass / 3f) - 2;

                }
            }

            Console.WriteLine(fuelReq);
        }
    }
}
