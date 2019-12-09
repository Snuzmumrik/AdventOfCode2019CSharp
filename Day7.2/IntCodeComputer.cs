using System;
using System.Collections.Generic;
using System.Text;

namespace Day7._2
{
    public class IntCodeComputer
    {
        public bool Exited { get; private set; }
        public bool WaitingForInput { get; set; }
        public int InputStartPosition { get; set; }
        public Queue<int> InputQueue { get; private set; }
        public Queue<int> OutputQueue { get; set; }

        public IntCodeComputer()
        {
            InputQueue = new Queue<int>();
        }

        //public static void IntCodeComputerMain(int[] intCode, Func<int> inputFunc, Action<int> outputAct)
        //{
        //    intCode = RunProgram(intCode, 0, inputFunc, outputAct);
        //}

        public int[] RunProgram(int[] intCode)
        {
            return RunProgram(intCode, InputQueue.Dequeue());
        }
        public int[] RunProgram(int[] intCode, int pos)
        {
            WaitingForInput = false;
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
                if (InputQueue.Count > 0)
                {
                    intCode = Program3(intCode, pos + 1, p);
                    nextPos = pos + 2;
                }
                else
                {
                    WaitingForInput = true;
                    InputStartPosition = pos;
                    return intCode;
                }

            }
            else if (p.OpCode == OpCode.Output)
            {
                Program4(intCode, pos, p);
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
                Exited = true;
                return intCode;
            }
            else
            {
                throw new Exception();
            }

            if (startValue != intCode[pos])
            {
                nextPos = pos;
            }

            RunProgram(intCode, nextPos);
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
        int[] Program3(int[] intCode, int pos, Parameter p)
        {
            var input = InputQueue.Dequeue();
            var inputValue = intCode[pos];
            intCode[inputValue] = input;
            return intCode;
        }

        void Program4(int[] intCode, int pos, Parameter p)
        {
            if (p.FirstParamMode == Mode.Immidiate)
            {
                var outputValue = (intCode[pos + 1]);
                OutputQueue.Enqueue(outputValue);
            }
            else
            {
                var outputValue = (intCode[intCode[pos + 1]]);
                OutputQueue.Enqueue(outputValue);
            }

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

