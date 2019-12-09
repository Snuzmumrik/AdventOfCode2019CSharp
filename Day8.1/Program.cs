using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day8._1
{
    class Program
    {
        public static void Main(string[] args)
        {
            var inputFile = "./input.txt";

            if (File.Exists(inputFile))
            {
                var inputString = File.ReadAllText(inputFile);

                var height = 6;
                var length = 25;
                var layer = height * length;

                var layers = new List<string>();
                var zeroCounter = int.MaxValue;
                var zeroIndex = 0;

                for(var i = 0; i < inputString.Length; i++)
                {
                    layers.Add(inputString.Substring(0, layer));
                    var zeroCount = Regex.Matches(layers[i], "0").Count;
                    if (zeroCount < zeroCounter)
                    {
                        zeroCounter = zeroCount;
                        zeroIndex = i;
                    }
                    inputString= inputString.Substring(layer);
                }


                Console.WriteLine("Multiplied 1 and 2: " + (Regex.Matches(layers[zeroIndex], "1").Count * Regex.Matches(layers[zeroIndex], "2").Count));
               
            }
        }

        
    }

}
