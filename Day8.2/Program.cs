using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day8._2
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

                var mainLayer = new List<string>();
                int [,] picture = new int[25,6];
                int[] pic = new int[layer];

                for(int i = 0; i < layers.Count; i++)
                {
                    //Layers
                        var currentLayer = layers[i];
                        int[] currentLayerInt = Array.ConvertAll(currentLayer.Split(), s => int.Parse(s));
                    for(int j = 0; j < currentLayerInt.Length; j++)
                    {
                        if(currentLayer[j] == 0){
                            pic[j] = 0;
                        }else if(currentLayer[j] == 1){
                            pic[j] = 1;
                        }
                    }
                }




                Console.WriteLine("Multiplied 1 and 2: " + (Regex.Matches(layers[zeroIndex], "1").Count * Regex.Matches(layers[zeroIndex], "2").Count));
               
            }
        }

        
    }

}
