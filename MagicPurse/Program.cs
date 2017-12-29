using System;
using MagicPurse.Library;

namespace MagicPurse
{
    class Program
    {
        static void Main(string[] args)
        {
            string argument = args.Length > 0 ? args[0] : string.Empty;
            IMagicPurse purse = null;
            switch (argument)
            {
                case "-a":
                    purse = new MagicPurseAsync(new Splitter());
                    break;
                case "-q":
                    purse = new MagicPurseQueue(new Splitter());
                    break;
                case "-s":
                case "":
                    purse = new MagicPurseSync(new Splitter());
                    break;
                default:
                    PrintInvalidArguments();
                    return;
            }

            Console.WriteLine("Welcome to the MagicPurse");
            var terminate = false;
            var parser = new AmountParser();
            while (!terminate)
            {
                terminate = ReadAmountFromConsole(out var amount);
                if (!terminate)
                {
                    try
                    {
                        var halfPence = parser.ParseAndGetNumberOfHalfPence(amount);
                        
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

        private static void PrintInvalidArguments()
        {
            Console.WriteLine("Invalid arguments");
            Console.WriteLine("Syntax: MagicPurse [option]");
            Console.WriteLine("Options:");
            Console.WriteLine("-s   Execute in single thread mode (default)");
            Console.WriteLine("-a   Execute in async parallel mode");
            Console.WriteLine("-q   Execute in queued async parallel mode");
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
