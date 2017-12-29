using System;

namespace MagicPurse.Library
{

    public class AmountParser
    {
        private readonly long[] _partMultipliers = { 2, 24, 480 };

        public long ParseAndGetNumberOfHalfPence(string amountString)
        {
            var arguments = amountString.Split('/');
            if (arguments.Length > 3)
            {
                throw new ArgumentException("Not a valid amount");
            }

            long totalHalfPence = 0;
            for (int i = 0; i < arguments.Length; i++)
            {
                var s = arguments[arguments.Length - 1 - i];
                if (i == 0 && s.EndsWith("d"))
                {
                    s = s.Remove(s.Length - 1);
                }

                if (s == "-")
                {
                    continue;
                }

                if (int.TryParse(s, out var parsed))
                {
                    totalHalfPence += parsed * _partMultipliers[i];
                }
                else
                {
                    throw new ArgumentException("Not a valid amount");
                }

            }

            return totalHalfPence;
        }
    }
}
