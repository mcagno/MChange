
using System;
using System.Collections.Generic;
using System.Linq;


namespace MagicPurse
{
    public class MagicPurse
    {
        private long[] coins =
        {
            1, //Halfpenny 0.5d
            2, //Penny 1d
            6, //Threepence 3d
            12, //Sixpence 6d
            24, //Shilling 1/-
            48, //Two shillings 
            60  //Two shillings and sixpence
            
        };

        private List<long[]> _combinations = new List<long[]>();

        public long MakeEvenChange(long number)
        {
            long[] combination = new long[coins.Length];

            GetCombi(number, coins.Length - 1, combination);
            long sum = 0;
            foreach (var combi in _combinations)
            {
                //sum += PermutationWithRepetitions(combi);
                sum += GetHalfCombis(combi, 0, combi.Sum() / 2);
            }

            return sum;
        }

        private long GetHalfCombis(long[] combi, int index, long remaining)
        {
            long result = 0;
            
            long upper = Math.Min(combi[index], remaining);
            for (long j = upper; j >= 0; j--)
            {
                var actualRemaining = remaining - j;
                if (actualRemaining == 0)
                {
                    result++;
                }
                else
                {
                    if ((index < combi.Length - 1)
                      && (combi.Skip(index + 1).Sum() >= actualRemaining))
                    {
                        result += GetHalfCombis(combi, index + 1, actualRemaining);
                    }
                        
                }
                    
            }
            
            return result;
        }

        private void GetCombi(long number, int i, long[] combination)
        {

            long division = number / coins[i];

            if (i == 0)
            {
                combination[i] = division;
                if (combination.Sum() % 2 == 0)
                {
                    _combinations.Add((long[]) combination.Clone());
                }
                return;
            }

            for (int k = 0; k <= division; k++)
            {
                combination[i] = k;
                long remainder = number - coins[i] * k;
                if (remainder > 0)
                {
                    GetCombi(remainder, i - 1, combination);
                }

            }
        }

        private long Factorial(long x)
        {
            if (x <= 1)
            {
                return 1;
            }
            return x * Factorial(x - 1);
        }

        private long PermutationWithRepetitions(long[] combinations)
        {
            long denominator = 1;
            long numerator = Factorial(combinations.Sum());
            for (int i = 0; i < combinations.Length; i++)
            {
                denominator *= Factorial(combinations[i]);
            }
            return numerator / denominator;
        }
    }
}
