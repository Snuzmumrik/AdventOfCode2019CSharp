using System;
using System.IO;
using System.Threading;

namespace Day9._1
{
    class Program
    {
        public static IntCodeComputer intCodeComputer { get; set; }
        public static long[] InProgram { get; set; }
        public static void Main(string[] args)
        {
            var inputFile = "./input.txt"; //Input test could be used to test, should return 584126
            

            if (File.Exists(inputFile))
            {
                InProgram = new long[100000];

                long[] programInput = Array.ConvertAll(File.ReadAllText(inputFile).Split(","), s => long.Parse(s));

                for(int i = 0; i < programInput.Length; i++)
                {
                    InProgram[i] = programInput[i];
                }
                intCodeComputer = new IntCodeComputer();
                intCodeComputer.InputQueue.Enqueue(2);
                //intCodeComputer.RunProgram(InProgram, 0);

                Thread T = new Thread(new ThreadStart(RunIntcodeComp), 1000000000);
                T.Start();

                
            }
        }
        public static void RunIntcodeComp()
        {
            Console.WriteLine("Hello");
            intCodeComputer.RunProgram(InProgram, 0);

            while (intCodeComputer.OutputQueue.Count > 0)
            {
                Console.WriteLine(intCodeComputer.OutputQueue.Dequeue());
            }
        }
    }
}
