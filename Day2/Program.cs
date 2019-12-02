using System;
using System.IO;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sr = File.OpenText("./input.txt"))
            {
                var input = sr.ReadLine();
                
                int[] opcode = Array.ConvertAll(input.Split(","), s => int.Parse(s));
                opcode[1] = 12;
                opcode[2] = 2;

                for(var i = 0; i < opcode.Length; i += 4)
                {
                    
                    if (opcode[i] == 1)
                    {
                        opcode[opcode[i + 3]] = (opcode[opcode[i + 1]] + opcode[opcode[i + 2]]);
                    }
                    else if(opcode[i] == 2)
                    {
                        opcode[opcode[i + 3]] = (opcode[opcode[i + 1]] * opcode[opcode[i + 2]]);
                    }
                    else if(opcode[i] == 99)
                    {
                        Console.WriteLine("Answer: " + opcode[0]);
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
}
