using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicPurse.Library
{
    public class ChangeMaker : ChangeMakerBase, IChangeMaker
    {
        public ChangeMaker(ISplitter splitter)
        {
            Splitter = splitter;
        }

        public long[][] GetBasicCombinations()
        {
            long[][] basicCombi = new long[Coins.Length][];
            for (int i = Coins.Length - 1; i > 0; i--)
            {
                long[] combi = new long[Coins.Length];
                long remaining = Coins[i];
                for (var index = i - 1; index >= 0; index--)
                {
                    var coin = Coins[index];
                    long ratio = remaining / coin;
                    combi[index] = ratio;
                    remaining %= coin;

                    if (remaining == 0)
                        break;
                }

                basicCombi[i] = combi;

            }

            return basicCombi;
        }

        

        public long MakeEvenChange(long number)
        {
            return GetCombi(number, Coins.Length - 1, new long[Coins.Length], null);
        }


        private long GetCombi(long number, int index, long[] combination, List<long[]>[,] cachedValues)
        {
            //Console.WriteLine($"I:{index} N:{number}");
            long sum = 0;
            long division = number / Coins[index];

            if (index == 0)
            {
                combination[index] = division;
                //cachedValues[index, number].Add((long[]) combination.Clone());
                if (combination.Sum() % 2 == 0)
                {                    
                    return Splitter.GetNumberOfEqualSplits(combination);   
                }
                return 0;
            }

            //if (parallel)
            //{
            //    Parallel.For(0, division + 1, k =>
            //        {
            //            long[] nestedCombi = (long[])combination.Clone();
            //            nestedCombi[index] = k;
            //            long remainder = number - _coins[index] * k;
            //            if (remainder == 0)
            //            {
            //                for (int i = index - 1; i >= 0; i--)
            //                {
            //                    nestedCombi[i] = 0;
            //                }

            //                //cachedValues[index, remainder] = (long[]) combination.Clone();

            //                if (nestedCombi.Sum() % 2 == 0)
            //                {
            //                    var halfCombis = GetHalfCombis(nestedCombi);
            //                    Interlocked.Add(ref sum, halfCombis);
            //                    //sum += halfCombis;
            //                }
            //            }
            //            else
            //            {

            //                var subResult = GetCombi(remainder, index - 1, nestedCombi, cachedValues, false);
            //                Interlocked.Add(ref sum, subResult);
            //            }
            //        }
            //    );
            //}
            //else
            //{
                for (var k = division; k >= 0; k--)
                {
                    combination[index] = k;
                    long remainder = number - Coins[index] * k;
                    if (remainder == 0)
                    {
                        for (int i = index - 1; i >= 0; i--)
                        {
                            combination[i] = 0;
                        }



                        //cachedValues[index, remainder] = (long[]) combination.Clone();

                        if (combination.Sum() % 2 == 0)
                        {
                            sum += Splitter.GetNumberOfEqualSplits(combination);
                    }
                    }
                    else
                    {


                        sum += GetCombi(remainder, index - 1, combination, cachedValues);
                    }

                }
            //}
            
            
            return sum;
        }

        private void PrintCombination(long[] combi)
        {

            string res = combi.Aggregate(string.Empty, (current, l) => l.ToString("000") + "|" + current);
            Console.WriteLine(res);
        }

        



    }
}
