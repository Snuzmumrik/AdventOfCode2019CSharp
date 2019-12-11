using System;
using System.Collections.Generic;
using System.Text;

namespace Day9._1
{
    public class IntCodeComputer
    {
        public Queue<long> InputQueue { get; private set; }
        public Queue<long> OutputQueue { get; set; }
        public long RelativeBase { get; set; }

        public IntCodeComputer()
        {
            InputQueue = new Queue<long>();
            OutputQueue = new Queue<long>();
        }

        public long[] RunProgram(long[] intCode)
        {
            RelativeBase = 0;
            return RunProgram(intCode, InputQueue.Dequeue());
        }
        public long[] RunProgram(long[] intCode, long pos)
        {
            long startValue = intCode[pos];
            long nextPos = 0;
            Parameter p = SetParameters(intCode[pos].ToString());

            long param1 = 0;
            long param2 = 0;
            long param3 = 0;
            if (p.OpCode == OpCode.Input)
            {
                param1 = HandleMode(intCode, p.FirstParamMode, pos + 1, true);
            }
            else
            {
                param1 = HandleMode(intCode, p.FirstParamMode, pos + 1, false);
                param2 = HandleMode(intCode, p.SecondParamMode, pos + 2, false);
                param3 = HandleMode(intCode, p.ThirdParamMode, pos + 3, true);
            }
            
            if (p.OpCode == OpCode.Add)
            {
                intCode = Program1(intCode, param1, param2, param3, p);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Multiply)
            {
                intCode = Program1(intCode, param1, param2, param3, p);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Input)
            {
                if (InputQueue.Count > 0)
                {
                    intCode = Program3(intCode, param1);
                    nextPos = pos + 2;
                }

            }
            else if (p.OpCode == OpCode.Output)
            {
                Program4(intCode, param1);
                nextPos = pos + 2;
            }
            else if (p.OpCode == OpCode.JumpToIfTrue)
            {
                nextPos = Program5(true, param1, param2, intCode, pos);
            }
            else if (p.OpCode == OpCode.JumpToIfFalse)
            {
                nextPos = Program5(false, param1, param2, intCode, pos);
            }
            else if (p.OpCode == OpCode.LessThan)
            {
                intCode = Program7(true, param1, param2, param3, intCode);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Equals)
            {
                intCode = Program7(false, param1, param2, param3, intCode);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.ChangeRelBase)
            {
                Program9(intCode, param1);
                nextPos = pos + 2;
            }
            else if (p.OpCode == OpCode.Exit)
            {
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


        private long HandleMode(long[] intCode, Mode mode, long param, bool posMode)
        {
            long returnParam = 0;
            switch (mode)
            {
                case Mode.Position:
                    returnParam = posMode ? intCode[param] : intCode[intCode[param]];
                    break;
                case Mode.Immidiate:
                    returnParam = posMode ? 0 : intCode[param];
                    break;
                case Mode.Relative:
                    param =  RelativeBase + intCode[param];
                    returnParam = posMode ? param : intCode[param];
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
            p.ThirdParamMode = Mode.Position;
            //Console.WriteLine("OPCODES: " + opCode);
            opCode = opCode.PadLeft(5, '0');

            if (opCode[2] == '1') p.FirstParamMode = Mode.Immidiate;
            if (opCode[2] == '2') p.FirstParamMode = Mode.Relative;

            if (opCode[1] == '1') p.SecondParamMode = Mode.Immidiate;
            if (opCode[1] == '2') p.SecondParamMode = Mode.Relative;

            // if (opCode[0] == '1') p.ThirdParamMode = Mode.Immidiate;
            if (opCode[0] == '2') p.ThirdParamMode = Mode.Relative;

            opCode = opCode.Substring(3);

            p.OpCode = (OpCode)long.Parse(opCode);

            return p;
        }
        long[] Program1(long[] intCode, long param1, long param2, long param3, Parameter p)
        {
            intCode[param3] = (p.OpCode == OpCode.Add) ? param1 + param2 : param1 * param2;
            return intCode;
        }

        long[] Program3(long[] intCode, long pos)
        {
            var input = InputQueue.Dequeue();
            intCode[pos] = input;
            //InputQueue.Enqueue(2);
            return intCode;
        }

        void Program4(long[] intCode, long param1)
        {
            var outputValue = param1;
            OutputQueue.Enqueue(outputValue);
        }

        long Program5(bool ifTrue, long param1, long param2, long[] intCode, long currentPos)
        {
            if (ifTrue)
            {
                if (param1 != 0) return param2;
            }
            else
            {
                if (param1 == 0) return param2;
            }

            return currentPos + 3;
        }

        long[] Program7(bool lessThan, long param1, long param2, long param3, long[] intCode)
        {
            intCode[param3] = 0;
            if (lessThan)
            {
                if (param1 < param2) intCode[param3] = 1;
            }
            else if (!lessThan)
            {
                if (param1 == param2) intCode[param3] = 1;
            }

            return intCode;
        }

        private void Program9(long[] intCode, long param)
        {
            RelativeBase += param;
        }

    }

    public class Parameter
    {
        public OpCode OpCode { get; set; }
        public Mode FirstParamMode { get; set; }
        public Mode SecondParamMode { get; set; }
        public Mode ThirdParamMode { get; set; }
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

