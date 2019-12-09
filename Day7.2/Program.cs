using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day7._2
{
    class Program
    {

        static Queue<int> Queue { get; set; }
        static void Main(string[] args)
        {

            var inputFile = "./input.txt";

            if (File.Exists(inputFile))
            {
                int[] program = Array.ConvertAll(File.ReadAllText(inputFile).Split(","), s => int.Parse(s));
                var combinations = GetCombinations(new List<int> { 9, 8, 7, 6, 5 });
                var highestOutput = 0;

                foreach (var combination in combinations)
                {
                    var amp1 = new IntCodeComputer();
                    var amp2 = new IntCodeComputer();
                    var amp3 = new IntCodeComputer();
                    var amp4 = new IntCodeComputer();
                    var amp5 = new IntCodeComputer();

                    var amp1List = program;
                    var amp2List = program;
                    var amp3List = program;
                    var amp4List = program;
                    var amp5List = program;

                    amp1.OutputQueue = amp2.InputQueue;
                    amp2.OutputQueue = amp3.InputQueue;
                    amp3.OutputQueue = amp4.InputQueue;
                    amp4.OutputQueue = amp5.InputQueue;
                    amp5.OutputQueue = amp1.InputQueue;

                    amp1.InputQueue.Enqueue(combination[0]);
                    amp1.InputQueue.Enqueue(0);
                    amp2.InputQueue.Enqueue(combination[1]);
                    amp3.InputQueue.Enqueue(combination[2]);
                    amp4.InputQueue.Enqueue(combination[3]);
                    amp5.InputQueue.Enqueue(combination[4]);

                    while (!amp1.Exited
                        || !amp2.Exited
                        || !amp3.Exited
                        || !amp4.Exited
                        || !amp5.Exited)
                    {
                        amp1List = amp1.RunProgram(amp1List, amp1.WaitingForInput ? amp1.InputStartPosition : 0);
                        amp2List = amp2.RunProgram(amp2List, amp2.WaitingForInput ? amp2.InputStartPosition : 0);
                        amp3List = amp3.RunProgram(amp3List, amp3.WaitingForInput ? amp3.InputStartPosition : 0);
                        amp4List = amp4.RunProgram(amp4List, amp4.WaitingForInput ? amp4.InputStartPosition : 0);
                        amp5List = amp5.RunProgram(amp5List, amp5.WaitingForInput ? amp5.InputStartPosition : 0);
                    }
                    var output = amp1.InputQueue.Dequeue();
                    if (output > highestOutput)
                    {
                        highestOutput = output;
                    }
                }
                Console.WriteLine("Action!: " + highestOutput);
            }

        }

        static List<List<int>> GetCombinations(List<int> list)
        {
            var listlist = new List<List<int>>();
            foreach (var firstItem in list)
            {
                var firstList = list.Where(x => x != firstItem);
                foreach (var secondItem in firstList)
                {
                    var secondList = firstList.Where(x => x != secondItem);
                    foreach (var thirdItem in secondList)
                    {
                        var thirdList = secondList.Where(x => x != thirdItem);
                        foreach (var fourthItem in thirdList)
                        {
                            var fourthList = thirdList.Where(x => x != fourthItem);
                            foreach (var fifthItem in fourthList)
                            {
                                listlist.Add(new List<int> { firstItem, secondItem, thirdItem, fourthItem, fifthItem });
                            }
                        }
                    }
                }
            }
            return listlist;
        }

        static int AmplifierInput()
        {
            return Queue.Dequeue();
        }

    }
}
