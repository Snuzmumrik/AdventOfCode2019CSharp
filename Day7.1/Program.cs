using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day7._1
{
    class Program
    {

        static Queue<int> Queue { get; set; }
        static void Main(string[] args)
        {

            var inputFile = "./input.txt";

            if (File.Exists(inputFile))
            {
                int[] program = Array.ConvertAll(File.ReadAllText(inputFile).Split(","), s => int.Parse(s));
                var combinations = GetCombinations(new List<int> { 4, 3, 2, 1, 0 });
                var highestOutput = 0;

                foreach(var combination in combinations)
                {

                var output = 0;
                    for(var i = 0; i < combination.Count; i++)
                    {

                Queue = new Queue<int>(  new List<int> { combination[i], output } );
                IntCodeComputer.IntCodeComputerMain(program, AmplifierInput, (num) => output = num);
                    }

                    if(output > highestOutput)
                    {
                        highestOutput = output;
                    }
                }
                Console.WriteLine("Action!: " + highestOutput);
            }
            
        }

        static List<List<int>> GetCombinations(List<int> list)
        {
            var listlist = new List<List<int>>();
            foreach (var firstItem in list)
            {
                var firstList = list.Where(x => x != firstItem);
                foreach (var secondItem in firstList)
                {
                    var secondList = firstList.Where(x => x != secondItem);
                    foreach (var thirdItem in secondList)
                    {
                        var thirdList = secondList.Where(x => x != thirdItem);
                        foreach (var fourthItem in thirdList)
                        {
                            var fourthList = thirdList.Where(x => x != fourthItem);
                            foreach (var fifthItem in fourthList)
                            {
                                listlist.Add(new List<int> { firstItem, secondItem, thirdItem, fourthItem, fifthItem });
                            }
                        }
                    }
                }
            }
            return listlist;
        }

        static int AmplifierInput()
        {
            return Queue.Dequeue();
        }

    }
}
