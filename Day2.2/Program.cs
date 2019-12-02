using System;
using System.IO;

namespace Day2._2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sr = File.OpenText("./input.txt"))
            {
                var input = sr.ReadLine();


                int correctNoun = 0;
                int correctVerb = 0;


                for(int noun = 0; noun < 100; noun++)
                {
                    for(int verb = 0; verb < 100; verb++)
                    {
                        int[] opcode = Array.ConvertAll(input.Split(","), s => int.Parse(s));
                        opcode[1] = noun;
                        opcode[2] = verb;
                        if(loopList(opcode) == 19690720)
                        {
                            correctNoun = noun;
                            correctVerb = verb;
                            break;
                        }

                    }
                }

                int correctProgram = 100 * correctNoun + correctVerb;
                Console.WriteLine("Noun " + correctNoun + " Verb: " + correctVerb + " ProgramNo: " + correctProgram);
            }
        }

        static int loopList(int[] opcode)
        {
            int answer = 0;
            for (var i = 0; i < opcode.Length; i += 4)
            {

                if (opcode[i] == 1)
                {
                    opcode[opcode[i + 3]] = (opcode[opcode[i + 1]] + opcode[opcode[i + 2]]);
                }
                else if (opcode[i] == 2)
                {
                    opcode[opcode[i + 3]] = (opcode[opcode[i + 1]] * opcode[opcode[i + 2]]);
                }
                else if (opcode[i] == 99)
                {
                    Console.WriteLine("Answer: " + opcode[0]);
                    answer =  opcode[0];
                    break;
                }
                else
                {
                }

            }
                return answer;
        }
    }
}
