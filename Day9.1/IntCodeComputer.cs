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
            OutputQueue = new Queue<long>();
        }

        //public static void intCodeComputerMain(long[] intCode, Func<long> inputFunc, Action<long> outputAct)
        //{
        //    intCode = RunProgram(intCode, 0, inputFunc, outputAct);
        //}

        public long[] RunProgram(long[] intCode)
        {
            RelativeBase = 0;
            return RunProgram(intCode, InputQueue.Dequeue());
        }
        public long[] RunProgram(long[] intCode, long pos)
        {
            WaitingForInput = false;
            
            long startValue = intCode[pos];
            long nextPos = 0;
            Parameter p = SetParameters(intCode[pos].ToString());

            if (p.OpCode == OpCode.Add)
            {
                //intCode[intCode[pos + 3]] = Program1(intCode, pos + 1, pos + 2, p);
                intCode = Program1(intCode, pos + 1, pos + 2, pos + 3, p);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Multiply)
            {
                //intCode[intCode[pos + 3]] = Program2(intCode, pos + 1, pos + 2, p);
                intCode = Program2(intCode, pos + 1, pos + 2, pos + 3, p);
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
                intCode = Program7(pos + 1, pos + 2, pos + 3, p, intCode);
                nextPos = pos + 4;
            }
            else if (p.OpCode == OpCode.Equals)
            {
                intCode = Program8(pos + 1, pos + 2, pos + 3, p, intCode);
                nextPos = pos + 4;
            }
            else if(p.OpCode == OpCode.ChangeRelBase)
            {
                Program9(intCode, pos + 1, p);
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


        private long HandleMode(long[] intCode, Mode mode, long param)
        {
            long returnParam = 0;
            switch (mode)
            {
                case Mode.Position:
                    returnParam = intCode[intCode[param]];
                    break;
                case Mode.Immidiate:
                    returnParam = intCode[param];
                    break;
                case Mode.Relative:
                    param = intCode[param];
                    param = RelativeBase + param;
                    returnParam = intCode[param];
                    break;
                default:
                    Console.WriteLine("Something wrong in modehandeling");
                    break;
            }

            return returnParam;
             
        }

        private long HandleModeInputs(long[] intCode, Mode mode, long param)
        {
            long returnParam = 0;
            switch (mode)
            {
                case Mode.Position:
                    returnParam = param;
                    break;
                case Mode.Immidiate:
                    returnParam = param;
                    break;
                case Mode.Relative:
                    param = intCode[param];
                    param = RelativeBase + param;
                    returnParam = intCode[param];
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
            Console.WriteLine("OPCODES: " + opCode);

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

                        if (opCode.Length > 4)
                        {
                            if (opCode[opCode.Length - 5] == '1') p.ThirdParamMode = Mode.Immidiate;
                            if (opCode[opCode.Length - 5] == '2') p.ThirdParamMode = Mode.Relative;
                        }
                    }
                }
                opCode = opCode.Substring(opCode.Length - 2);
            }
            p.OpCode = (OpCode)long.Parse(opCode);

            return p;
        }
        long[] Program1(long[] intCode, long param1, long param2, long param3, Parameter p)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1); 
            param2 = HandleMode(intCode, p.SecondParamMode, param2);
            
                param3 = HandleModeInputs(intCode, p.ThirdParamMode, param3);

            intCode[intCode[param3]] = param1 + param2;
            return intCode;
        }

        long[] Program2(long[] intCode, long param1, long param2, long param3, Parameter p)
        {
            param1 = HandleMode(intCode, p.SecondParamMode, param1); 
            param2 = HandleMode(intCode, p.SecondParamMode, param2);
            
            param3 = HandleModeInputs(intCode, p.ThirdParamMode, param3);


            intCode[intCode[param3]] = param1 * param2;
            return intCode;
        }
        long[] Program3(long[] intCode, long pos, Parameter p)
        {
            var input = InputQueue.Dequeue();
            pos = HandleMode(intCode, p.FirstParamMode, pos);
            var inputPosition = HandleMode(intCode, p.FirstParamMode, pos);
            intCode[RelativeBase + inputPosition] = input;
            //var input = InputQueue.Dequeue();
            //var inputValue = HandleMode(intCode, p.FirstParamMode, pos + 1);
            //intCode[inputValue] = input;
            InputQueue.Enqueue(1);
            return intCode;
        }

        void Program4(long[] intCode, long pos, Parameter p)
        {
            var outputValue = HandleMode(intCode, p.FirstParamMode, pos + 1);
            OutputQueue.Enqueue(outputValue);
        }

        long Program5(long param1, long param2, Parameter p, long[] intCode, long currentPos)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1);
            param2 = HandleMode(intCode, p.SecondParamMode, param2);
            if (param1 != 0) return param2;

            return currentPos + 3;
        }

        long Program6(long param1, long param2, Parameter p, long[] intCode, long currentPos)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1);
            param2 = HandleMode(intCode, p.SecondParamMode, param2);

            if (param1 == 0) return param2;
            return currentPos + 3;
        }
        long[] Program7(long param1, long param2, long param3, Parameter p, long[] intCode)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1);
            param2 = HandleMode(intCode, p.SecondParamMode, param2);
            
             param3 = HandleModeInputs(intCode, p.ThirdParamMode, param3);
            
            
            
            if (param1 < param2) intCode[intCode[param3]] = 1;
            else intCode[intCode[param3]] = 0;

            return intCode;
        }
        long[] Program8(long param1, long param2, long param3, Parameter p, long[] intCode)
        {
            param1 = HandleMode(intCode, p.FirstParamMode, param1);
            param2 = HandleMode(intCode, p.SecondParamMode, param2);
            param3 = HandleModeInputs(intCode, p.ThirdParamMode, param3);
            

            if (param1 == param2) intCode[intCode[param3]] = 1;
            else intCode[intCode[param3]] = 0;

            return intCode;
        }

        private void Program9(long[] intCode, long param, Parameter p)
        {
            param = HandleMode(intCode, p.FirstParamMode, param);
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

