using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Day8._02
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

                for (var i = 0; i < inputString.Length; i++)
                {
                    layers.Add(inputString.Substring(0, layer));
                    var zeroCount = Regex.Matches(layers[i], "0").Count;
                    if (zeroCount < zeroCounter)
                    {
                        zeroCounter = zeroCount;
                        zeroIndex = i;
                    }
                    inputString = inputString.Substring(layer);
                }

                Console.WriteLine("Multiplied 1 and 2: " + (Regex.Matches(layers[zeroIndex], "1").Count * Regex.Matches(layers[zeroIndex], "2").Count));

                char[] pic = new char[layer];

                Bitmap bitmap = new Bitmap(length, height);
                for (int i = 0; i < layers.Count; i++)
                {
                    //Layers
                    var currentLayer = layers[i];
                    //int[] currentLayerInt = Array.ConvertAll(currentLayer.Split(""), s => int.Parse(s));
                    for (int j = 0; j < currentLayer.Length; j++)
                    {
                        if (currentLayer[j] == '0' && pic[j] != '0' && pic[j] != '1')
                        {
                            pic[j] = '0';
                        }
                        else if (currentLayer[j] == '1' && pic[j] != '0' && pic[j] != '1')
                        {
                            pic[j] = '1';
                        }
                    }
                }

                var charPos = 0;
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < length; x++)
                    {
                        bitmap.SetPixel(x, y, pic[charPos] == '0' ? Color.Black : Color.White);
                        charPos++;
                    }
                }

                bitmap.Save("out.bmp");

                

            }
        }

    }
}


