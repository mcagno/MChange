using System;
using MagicPurse.Library;

namespace MagicPurse
{
    class Program
    {
        static void Main()
        {
           Console.WriteLine("Welcome to the MagicPurse");
            bool terminate = false;
            AmountParser parser = new AmountParser();
            while (!terminate)
            {
                terminate = ReadAmountFromConsole(out var amount);
                if (!terminate)
                {
                    try
                    {
                        var halfPence = parser.ParseAndGetNumberOfHalfPence(amount);
                        IMagicPurse purse = new MagicPurseQueue(new Splitter());
                        Console.WriteLine("Calculation in progress, please wait.");
                        var startTime = DateTime.Now;
                        var makeEvenChange = purse.GetAllSplits(halfPence);
                        var duration = DateTime.Now - startTime;
                        Console.WriteLine($"Number of possibilities: {makeEvenChange} (took {duration.TotalMilliseconds}ms)");
                    }
                    catch (ArgumentException a)
                    {
                        Console.WriteLine(a.Message);
                    }
                    
                }
            }
        }

        private static bool ReadAmountFromConsole(out string amount)
        {
            Console.Write("Please enter an amount (press ENTER to terminate): ");
            string readLine = Console.ReadLine();
            
            if (!string.IsNullOrEmpty(readLine))
            {
                amount = readLine;
                return false;
            }
            amount = string.Empty;
            return true;
        }
    }
}
