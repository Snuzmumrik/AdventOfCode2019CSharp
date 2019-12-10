using System;
using System.Collections.Generic;
using System.Text;

namespace Day9._1
{
    public class IntCodeComputer
    {
        public bool Exited { get; private set; }
        public bool WaitingForInput { get; set; }
        public int InputStartPosition { get; set; }
        public Queue<int> InputQueue { get; private set; }
        public Queue<int> OutputQueue { get; set; }
        public int RelativeBase { get; set; }

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
            RelativeBase = 0;
            var startValue = intCode[pos];
            var nextPos = 0;
            Parameter p = SetParameters(intCode[pos].ToString());

            if (p.OpCode == OpCode.Add)
            {
                intCode[intCode[pos + 3]] = Program1(intCode, pos + 1, pos + 2, p); 
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Multiply)
            {
                intCode[intCode[pos + 3]] = Program2(intCode, pos + 1, pos + 2, p);
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
                nextPos = Program5(pos + 1, pos + 2, p, intCode, pos);

            }
            else if (p.OpCode == OpCode.JumpToIfFalse)
            {
                nextPos = Program6(pos + 1, pos + 2, p, intCode, pos);
            }
            else if (p.OpCode == OpCode.LessThan)
            {
                intCode = Program7(pos + 1, pos + 2, intCode[pos + 3], p, intCode);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Equals)
            {
                intCode = Program8(pos + 1, pos + 2, intCode[pos + 3], p, intCode);
                nextPos = pos + 4;
            }
            else if(p.OpCode == OpCode.ChangeRelBase)
            {
                Program9(intCode, intCode[pos + 1], p);
                nextPos = pos + 2;
            }
            else if (p.OpCode == OpCode.Exit)
            {
                Exited = true;
                return intCode;
            }
            else
            {
                Console.WriteLine("Exiting with error: ");
                Console.WriteLine("OpCode: " + p.OpCode);
                Console.WriteLine("Position: " + pos);
                throw new Exception();
            }

            if (startValue != intCode[pos])
            {
                nextPos = pos;
            }

            RunProgram(intCode, nextPos);
            return intCode;
        }


        private int HandleMode(int[] intCode, Mode mode, int param)
        {
            var returnParam = 0;
            switch (mode)
            {
                case Mode.Position:
                    returnParam = intCode[intCode[param]];
                    break;
                case Mode.Immidiate:
                    returnParam = intCode[param];
                    break;
                case Mode.Relative:
                    returnParam = intCode[intCode[RelativeBase + param]];
                    break;
                default:
                    Console.WriteLine("Something wrong in modehandeling");
                    break;
            }

            return returnParam;
             
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
                    if (opCode[opCode.Length - 3] == '2') p.FirstParamMode = Mode.Relative;
                    if (opCode.Length > 3)
                    {
                        if (opCode[opCode.Length - 4] == '1') p.SecondParamMode = Mode.Immidiate;
                        if (opCode[opCode.Length - 4] == '2') p.SecondParamMode = Mode.Relative;
                    }
                }
                opCode = opCode.Substring(opCode.Length - 2);
            }
            p.OpCode = (OpCode)int.Parse(opCode);

            return p;
        }
        private int Program1(int[] intCode, int param1, int param2, Parameter p)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1); 
            param2 = HandleMode(intCode, p.SecondParamMode, param2);
            return param1 + param2;
        }

        private int Program2(int[] intCode, int param1, int param2, Parameter p)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1); 
            param2 = HandleMode(intCode, p.SecondParamMode, param2);
            return param1 * param2;
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
            var outputValue = HandleMode(intCode, p.FirstParamMode, pos + 1);
            OutputQueue.Enqueue(outputValue);
        }

        int Program5(int param1, int param2, Parameter p, int[] intCode, int currentPos)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1);
            param2 = HandleMode(intCode, p.SecondParamMode, param2);
            if (param1 != 0) return param2;

            return currentPos + 3;
        }

        int Program6(int param1, int param2, Parameter p, int[] intCode, int currentPos)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1);
            param2 = HandleMode(intCode, p.SecondParamMode, param2);

            if (param1 == 0) return param2;
            return currentPos + 3;
        }
        int[] Program7(int param1, int param2, int param3, Parameter p, int[] intCode)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1);
            param2 = HandleMode(intCode, p.SecondParamMode, param2);

            if (param1 < param2) intCode[param3] = 1;
            else intCode[param3] = 0;

            return intCode;
        }
        int[] Program8(int param1, int param2, int param3, Parameter p, int[] intCode)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1);
            param2 = HandleMode(intCode, p.SecondParamMode, param2);

            if (param1 == param2) intCode[param3] = 1;
            else intCode[param3] = 0;

            return intCode;
        }

        private void Program9(int[] intCode, int param, Parameter p)
        {
            if (p.FirstParamMode == Mode.Position) param = intCode[param];
            if (p.FirstParamMode == Mode.Relative) param = intCode[RelativeBase + param];
            RelativeBase += param;
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
        Immidiate = 1,
        Relative = 2
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
        ChangeRelBase = 9,
        Exit = 99
    }
}

