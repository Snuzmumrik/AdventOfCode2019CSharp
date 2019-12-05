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

            using (var sr = File.OpenText("./input.txt"))
            {
                var input = sr.ReadLine();

                int[] intCode = Array.ConvertAll(input.Split(","), s => int.Parse(s));

                IntCodeComputer(intCode, Int32.Parse(inputInstruction));
            }
        }

        private static void IntCodeComputer(int[] intCode, int inputInstruction)
        {
            var intCodeSize = 4;
            for (int i = 0; i < intCode.Length; i += intCodeSize)
            {
                intCodeSize = 4;
                var opCode = intCode[i].ToString();

                bool pos1ImmediateMode = false;
                bool pos2ImmediateMode = false;
                bool pos3ImmediateMode = false;

                if (opCode.Length > 1)
                {
                    if (opCode.Length > 2)
                    {
                        if (opCode[opCode.Length - 3] == '1') pos1ImmediateMode = true;
                        if (opCode.Length > 3)
                        {
                            if (opCode[opCode.Length - 4] == '1') pos2ImmediateMode = true;
                            if (opCode.Length > 4)
                            {
                                if (opCode[opCode.Length - 5] == '1') pos3ImmediateMode = true;
                            }
                        }
                    }
                    opCode = opCode.Substring(opCode.Length - 2);
                }

                if (opCode[opCode.Length - 1] == '1')
                {
                    if (pos3ImmediateMode)
                    {
                        intCode[i + 3] = ((pos1ImmediateMode ? intCode[i + 1] : intCode[intCode[i + 1]]) + (pos2ImmediateMode ? intCode[i + 2] : intCode[intCode[i + 2]]));
                    }
                    else
                    {
                        intCode[intCode[i + 3]] = ((pos1ImmediateMode ? intCode[i + 1] : intCode[intCode[i + 1]]) + (pos2ImmediateMode ? intCode[i + 2] : intCode[intCode[i + 2]]));
                    }
                }
                else if (opCode[opCode.Length - 1] == '2')
                {
                    if (pos3ImmediateMode)
                    {
                        intCode[i + 3] = ((pos1ImmediateMode ? intCode[i + 1] : intCode[intCode[i + 1]]) * (pos2ImmediateMode ? intCode[i + 2] : intCode[intCode[i + 2]]));
                    }
                    else
                    {
                        intCode[intCode[i + 3]] = ((pos1ImmediateMode ? intCode[i + 1] : intCode[intCode[i + 1]]) * (pos2ImmediateMode ? intCode[i + 2] : intCode[intCode[i + 2]]));
                    }
                }
                else if (opCode[opCode.Length - 1] == '3')
                {
                    intCode[intCode[i + 1]] = inputInstruction;

                    if (pos1ImmediateMode)
                    {
                        intCode[i + 1] = inputInstruction;
                    }
                    else
                    {

                        intCode[intCode[i + 1]] = inputInstruction;
                    }
                    intCodeSize = 2;
                }
                else if (opCode[opCode.Length - 1] == '4')
                {
                    if (pos1ImmediateMode)
                    {
                        Console.WriteLine(intCode[i+1]);
                    }
                    else
                    {
                        Console.WriteLine(intCode[intCode[i+1]]);
                    }
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
