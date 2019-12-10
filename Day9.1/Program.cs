using System;
using System.IO;

namespace Day9._1
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = "./inputTest.txt"; //Input test could be used to test, should return 584126

            if (File.Exists(inputFile))
            {
                long[] program = Array.ConvertAll(File.ReadAllText(inputFile).Split(","), s => long.Parse(s));
                var intCodeComputer = new IntCodeComputer();
                //intCodeComputer.InputQueue.Enqueue(1);//Teststuff
                intCodeComputer.OutputQueue = intCodeComputer.InputQueue;
                intCodeComputer.RunProgram(program, 0);
                
                while(intCodeComputer.OutputQueue.Count > 0)
                {
                    Console.WriteLine(intCodeComputer.OutputQueue.Dequeue());
                }
            }
        }
    }
}
