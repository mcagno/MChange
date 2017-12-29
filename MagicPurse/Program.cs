using System;
using MagicPurse.Library;

namespace MagicPurse
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Calculate(240);
            Calculate(480);
            //Calculate(4800);
            Console.ReadKey();
        }

        private static void Calculate(int value)
        {
            ChangeMaker purse = new ChangeMaker();
            var now = DateTime.Now;
            long makeEvenChange = purse.MakeEvenChange(value);
            var timeSpan = DateTime.Now - now;
            Console.WriteLine(makeEvenChange);
            Console.WriteLine($"Time: {timeSpan.TotalMilliseconds}ms");
        }
    }
}
