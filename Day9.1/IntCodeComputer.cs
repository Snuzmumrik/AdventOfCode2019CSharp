using System;
using System.Collections.Generic;
using System.Text;

namespace Day9._1
{
    public class IntCodeComputer
    {
        public bool Exited { get; private set; }
        public bool WaitingForInput { get; set; }
        public long InputStartPosition { get; set; }
        public Queue<long> InputQueue { get; private set; }
        public Queue<long> OutputQueue { get; set; }
        public long RelativeBase { get; set; }

        public IntCodeComputer()
        {
            InputQueue = new Queue<long>();
        }

        //public static void longCodeComputerMain(long[] longCode, Func<long> inputFunc, Action<long> outputAct)
        //{
        //    longCode = RunProgram(longCode, 0, inputFunc, outputAct);
        //}

        public long[] RunProgram(long[] longCode)
        {
            RelativeBase = 0;
            return RunProgram(longCode, InputQueue.Dequeue());
        }
        public long[] RunProgram(long[] longCode, long pos)
        {
            WaitingForInput = false;
            
            long startValue = longCode[pos];
            long nextPos = 0;
            Parameter p = SetParameters(longCode[pos].ToString());

            if (p.OpCode == OpCode.Add)
            {
                longCode[longCode[pos + 3]] = Program1(longCode, pos + 1, pos + 2, p); 
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Multiply)
            {
                longCode[longCode[pos + 3]] = Program2(longCode, pos + 1, pos + 2, p);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Input)
            {
                if (InputQueue.Count > 0)
                {
                    longCode = Program3(longCode, pos + 1, p);
                    nextPos = pos + 2;
                }
                else
                {
                    WaitingForInput = true;
                    InputStartPosition = pos;
                    return longCode;
                }

            }
            else if (p.OpCode == OpCode.Output)
            {
                Program4(longCode, pos, p);
                nextPos = pos + 2;
            }
            else if (p.OpCode == OpCode.JumpToIfTrue)
            {
                nextPos = Program5(pos + 1, pos + 2, p, longCode, pos);

            }
            else if (p.OpCode == OpCode.JumpToIfFalse)
            {
                nextPos = Program6(pos + 1, pos + 2, p, longCode, pos);
            }
            else if (p.OpCode == OpCode.LessThan)
            {
                longCode = Program7(pos + 1, pos + 2, longCode[pos + 3], p, longCode);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Equals)
            {
                longCode = Program8(pos + 1, pos + 2, longCode[pos + 3], p, longCode);
                nextPos = pos + 4;
            }
            else if(p.OpCode == OpCode.ChangeRelBase)
            {
                Program9(longCode, longCode[pos + 1], p);
                nextPos = pos + 2;
            }
            else if (p.OpCode == OpCode.Exit)
            {
                Exited = true;
                return longCode;
            }
            else
            {
                Console.WriteLine("Exiting with error: ");
                Console.WriteLine("OpCode: " + p.OpCode);
                Console.WriteLine("Position: " + pos);
                throw new Exception();
            }

            if (startValue != longCode[pos])
            {
                nextPos = pos;
            }

            RunProgram(longCode, nextPos);
            return longCode;
        }


        private long HandleMode(long[] longCode, Mode mode, long param)
        {
            long returnParam = 0;
            switch (mode)
            {
                case Mode.Position:
                    long position = longCode[param];
                    while(position > longCode.Length)
                    {
                        position -= longCode.Length -1;
                    }
                    returnParam = longCode[position];
                    break;
                case Mode.Immidiate:
                    returnParam = longCode[param];
                    break;
                case Mode.Relative:
                    param = longCode[param];
                    param = RelativeBase + param;
                    returnParam = longCode[param];
                    
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
            p.OpCode = (OpCode)long.Parse(opCode);

            return p;
        }
        private long Program1(long[] longCode, long param1, long param2, Parameter p)
        {
            param1 = HandleMode(longCode, p.FirstParamMode, param1); 
            param2 = HandleMode(longCode, p.SecondParamMode, param2);
            return param1 + param2;
        }

        private long Program2(long[] longCode, long param1, long param2, Parameter p)
        {
            param1 = HandleMode(longCode, p.FirstParamMode, param1); 
            param2 = HandleMode(longCode, p.SecondParamMode, param2);
            return param1 * param2;
        }
        long[] Program3(long[] longCode, long pos, Parameter p)
        {
            var input = InputQueue.Dequeue();
            var inputValue = longCode[pos];
            longCode[inputValue] = input;
            //InputQueue.Enqueue(1);
            return longCode;
        }

        void Program4(long[] longCode, long pos, Parameter p)
        {
            var outputValue = HandleMode(longCode, p.FirstParamMode, pos + 1);
            OutputQueue.Enqueue(outputValue);
        }

        long Program5(long param1, long param2, Parameter p, long[] longCode, long currentPos)
        {
            param1 = HandleMode(longCode, p.FirstParamMode, param1);
            param2 = HandleMode(longCode, p.SecondParamMode, param2);
            if (param1 != 0) return param2;

            return currentPos + 3;
        }

        long Program6(long param1, long param2, Parameter p, long[] longCode, long currentPos)
        {
            param1 = HandleMode(longCode, p.FirstParamMode, param1);
            param2 = HandleMode(longCode, p.SecondParamMode, param2);

            if (param1 == 0) return param2;
            return currentPos + 3;
        }
        long[] Program7(long param1, long param2, long param3, Parameter p, long[] longCode)
        {
            param1 = HandleMode(longCode, p.FirstParamMode, param1);
            param2 = HandleMode(longCode, p.SecondParamMode, param2);

            if (param1 < param2) longCode[param3] = 1;
            else longCode[param3] = 0;

            return longCode;
        }
        long[] Program8(long param1, long param2, long param3, Parameter p, long[] longCode)
        {
            param1 = HandleMode(longCode, p.FirstParamMode, param1);
            param2 = HandleMode(longCode, p.SecondParamMode, param2);

            if (param1 == param2) longCode[param3] = 1;
            else longCode[param3] = 0;

            return longCode;
        }

        private void Program9(long[] longCode, long param, Parameter p)
        {
            param = HandleMode(longCode, p.FirstParamMode, param);
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

