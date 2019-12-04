using System;
using System.Collections.Generic;
using System.Linq;

namespace Day4._2
{
    class Program
    {
        static void Main(string[] args)
        {
            int inputLow = 246540;
            int inputHigh = 787419;
            List<int> possibleCodes = new List<int>();

            for (int i = inputLow; i <= inputHigh; i++)
            {
                bool adjacent = checkAdjacent(i);
                bool noDecrese = checkDecrease(i);

                if (adjacent && noDecrese)
                {
                    possibleCodes.Add(i);
                }
            }

            Console.WriteLine("Answer: " + possibleCodes.Count);
        }

        private static bool checkDecrease(int i)
        {
            string codeString = i.ToString();
            for (int j = 1; j < codeString.Length; j++)
            {
                if (codeString[j - 1] > codeString[j])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool checkAdjacent(int i)
        {
            string codeString = i.ToString();

            for (int j = 1; j < codeString.Length; j++)
            {

                var occurence = codeString.Count(x => x == codeString[j - 1]);

                if (codeString[j - 1] == codeString[j] && occurence == 2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
