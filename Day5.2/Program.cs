using System;
using System.IO;
using System.Linq;

namespace Day5._2
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

        static int[] RunProgram(int[] intCode, int pos, int inputInstruction)
        {
            var startValue = intCode[pos];
            var nextPos = 0;
            Parameter p = SetParameters(intCode[pos].ToString());

            if (p.OpCode == OpCode.Add)
            {
                intCode[intCode[pos + 3]] = ((p.FirstParamMode == Mode.Immidiate ? intCode[pos + 1] : intCode[intCode[pos + 1]]) + (p.SecondParamMode == Mode.Immidiate ? intCode[pos + 2] : intCode[intCode[pos + 2]]));
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Multiply)
            {
                intCode[intCode[pos + 3]] = ((p.FirstParamMode == Mode.Immidiate ? intCode[pos + 1] : intCode[intCode[pos + 1]]) * (p.SecondParamMode == Mode.Immidiate ? intCode[pos + 2] : intCode[intCode[pos + 2]]));
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Input)
            {
                intCode[intCode[pos + 1]] = inputInstruction;

                if (p.FirstParamMode == Mode.Immidiate)
                {
                    intCode[pos + 1] = inputInstruction;
                }
                else
                {

                    intCode[intCode[pos + 1]] = inputInstruction;
                }
                nextPos = pos + 2;
            }
            else if (p.OpCode == OpCode.Output)
            {
                if (p.FirstParamMode == Mode.Immidiate)
                {
                    Console.WriteLine(intCode[pos + 1]);
                }
                else
                {
                    Console.WriteLine(intCode[intCode[pos + 1]]);
                }
                nextPos = pos + 2;
            }
            else if (p.OpCode == OpCode.JumpToIfTrue)
            {
                nextPos = Program5(intCode[pos + 1], intCode[pos + 2], p, intCode, pos);
                
            }
            else if (p.OpCode == OpCode.JumpToIfFalse)
            {
                nextPos = Program6(intCode[pos + 1], intCode[pos + 2], p, intCode, pos);
            }
            else if (p.OpCode == OpCode.LessThan)
            {
                intCode = Program7(intCode[pos + 1], intCode[pos + 2], intCode[pos + 3], p, intCode);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Equals)
            {
                intCode = Program8(intCode[pos + 1], intCode[pos + 2], intCode[pos + 3], p, intCode);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Exit)
            {
                Environment.Exit(0);
            }
            else
            {
                throw new Exception();
            }

            if(startValue != intCode[pos])
            {
                nextPos = pos;
            }

            RunProgram(intCode, nextPos, inputInstruction);
            return intCode;
        }

        private static Parameter SetParameters(string opCode)
        {
            Parameter p = new Parameter();
            p.FirstParamMode = Mode.Position;
            p.SecondParamMode = Mode.Position;

            if (opCode.Length > 1)
            {
                if (opCode.Length > 2)
                {
                    if (opCode[opCode.Length - 3] == '1') p.FirstParamMode = Mode.Immidiate;
                    if (opCode.Length > 3)
                    {
                        if (opCode[opCode.Length - 4] == '1') p.SecondParamMode = Mode.Immidiate;
                    }
                }
                opCode = opCode.Substring(opCode.Length - 2);
            }
            p.OpCode = (OpCode)int.Parse(opCode);

            return p;
        }

        private static void IntCodeComputer(int[] intCode, int inputInstruction)
        {
            intCode = RunProgram(intCode, 0, inputInstruction);
        }

        private static int Program5(int param1, int param2, Parameter p, int[] intCode, int currentPos)
        {
            if (p.FirstParamMode == Mode.Position) param1 = intCode[param1];


            if (p.SecondParamMode == Mode.Position) param2 = intCode[param2];
            if (param1 != 0) return param2;

            return currentPos + 3;
        }

        private static int Program6(int param1, int param2, Parameter p, int[] intCode, int currentPos)
        {
            if (p.FirstParamMode == Mode.Position) param1 = intCode[param1];


            if (p.SecondParamMode == Mode.Position) param2 = intCode[param2];

            if (param1 == 0) return param2;
            return currentPos + 3;
        }
        private static int[] Program7(int param1, int param2, int param3, Parameter p, int[] intCode)
        {
            if (p.FirstParamMode == Mode.Position) param1 = intCode[param1];
            if (p.SecondParamMode == Mode.Position) param2 = intCode[param2];

            if (param1 < param2) intCode[param3] = 1;
            else intCode[param3] = 0;

            return intCode;
        }
        private static int[] Program8(int param1, int param2, int param3, Parameter p, int[] intCode)
        {
            if (p.FirstParamMode == Mode.Position) param1 = intCode[param1];
            if (p.SecondParamMode == Mode.Position) param2 = intCode[param2];

            if (param1 == param2) intCode[param3] = 1;
            else intCode[param3] = 0;

            return intCode;
        }

    }

    public class Parameter
    {
        public OpCode OpCode { get; set; }
        public Mode FirstParamMode { get; set; }
        public Mode SecondParamMode { get; set; }
    }

    public enum Mode
    {
        Position = 0,
        Immidiate = 1
    }
    public enum OpCode
    {
        Add = 1,
        Multiply = 2,
        Input = 3,
        Output = 4,
        JumpToIfTrue = 5,
        JumpToIfFalse = 6,
        LessThan = 7,
        Equals = 8,
        Exit = 99
    }

}
