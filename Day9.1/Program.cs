using System;
using System.IO;

namespace Day9._1
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = "./input.txt"; //Input test could be used to test, should return 584126

            if (File.Exists(inputFile))
            {
                long[] program = new long[500000000];

                long[] programInput = Array.ConvertAll(File.ReadAllText(inputFile).Split(","), s => long.Parse(s));

                for(int i = 0; i < programInput.Length; i++)
                {
                    program[i] = programInput[i];
                }
                var intCodeComputer = new IntCodeComputer();
                intCodeComputer.InputQueue.Enqueue(1);

                intCodeComputer.RunProgram(program, 0);
                
                while(intCodeComputer.OutputQueue.Count > 0)
                {
                    Console.WriteLine(intCodeComputer.OutputQueue.Dequeue());
                }
            }
        }
    }
}
