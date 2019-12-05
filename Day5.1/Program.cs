using System;
using System.IO;
namespace Day5._1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input instruction: ");
            var inputInstruction = Console.ReadLine();

            using(var sr = File.OpenText("./input.txt"))
            {
                var input = sr.ReadLine();

                int[] intCode = Array.ConvertAll(input.Split(","), s => int.Parse(s));

                IntCodeComputer(intCode, inputInstruction);
            }
        }

        private static void IntCodeComputer(int[] intCode, int inputInstruction)
        {
            var intCodeSize = 4;
            for(int i = 0; i<intCode.Length; i += intCodeSize)
            {
                intCodeSize = 4;
                var opCode = intCode[i].ToString();
                Console.WriteLine(opCode);

                bool pos1ImmediateMode = false;
                bool pos2ImmediateMode = false;
                bool pos3ImmediateMode = false;

                if(opCode.Length > 1)
                {
                    if(opCode.Length > 2)
                    {
                        if (opCode[opCode.Length - 2] == '1') pos1ImmediateMode = true;
                        if(opCode.Length > 3)
                        {
                            if (opCode[opCode.Length - 3] == '1') pos2ImmediateMode = true;
                            if (opCode.Length > 4)
                            {
                                if (opCode[opCode.Length - 4] == '1') pos3ImmediateMode = true;
                            }
                        }
                    }

                    opCode = opCode.Substring(opCode.Length - 2);

                    
                }
                Console.WriteLine(opCode);
                //Console.WriteLine(opCode[opCode.Length - 1]);
                if (opCode[opCode.Length -1]  == '1')
                {
                    Console.WriteLine(intCode[i] + " " + intCode[i+1] + " " + intCode[i+2] + " " + intCode[i+3]);
                    Console.WriteLine(intCode[i + 1] + intCode[i + 2]);
                    intCode[intCode[i + 3]] = ((pos1ImmediateMode ? intCode[intCode[i + 1]] : intCode[i + 1]) + (pos2ImmediateMode ? intCode[intCode[i + 2]] : intCode[i+2])); //Krashar eftersom immediate mode ej är implementerat
                }
                else if (opCode[opCode.Length - 1] == '2')
                {
                    intCode[intCode[i + 3]] = ((pos1ImmediateMode ? intCode[intCode[i + 1]] : intCode[i + 1]) * (pos2ImmediateMode ? intCode[intCode[i + 2]] : intCode[i + 2]));
                }
                else if(opCode[opCode.Length -1] == '3')
                {
                    intCodeSize = 2;
                }
                else if (opCode[opCode.Length - 1] == '4')
                {
                    intCodeSize = 2;
                }
                else if (intCode[i] == 99)
                {
                    Console.WriteLine("Answer: " + intCode[0]);
                    break;
                }
                else
                {
                    Console.WriteLine("Something wrong");
                }
            }
        }
    }
}
