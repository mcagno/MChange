using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagicPurse.Library
{
    public class ChangeMakerAsync : ChangeMakerBase, IChangeMaker
    {

        public ChangeMakerAsync(ISplitter splitter)
        {
            Splitter = splitter;
        }

        public long MakeEvenChange(long number)
        {

            long[] combination = new long[Coins.Length];

            List<long[]> combinations = GetCombi2(number, Coins.Length - 1, combination, null, true);
            combinations.RemoveAll(l => l.Sum() % 2 != 0);
            long sum = 0;
            Parallel.ForEach(combinations, combi =>
            {
                var halfCombis = Splitter.GetNumberOfEqualSplits(combi);
                Interlocked.Add(ref sum, halfCombis);
                //sum += halfCombis;
            });
            //foreach (var longse in combinations)
            //{
            //    sum += GetHalfCombis(longse);
            //}
            return sum;
        }





        private List<long[]> GetCombi2(long number, int index, long[] combination, List<long[]>[,] cachedValues, bool parallel)
        {
            List<long[]> result = new List<long[]>();
            //Console.WriteLine($"I:{index} N:{number}");
            long division = number / Coins[index];

            //if (cachedValues[index, number] != null)
            //{
            //    return GetHalfCombis(cachedValues[index, number]);
            //}

            if (index == 0)
            {
                combination[index] = division;

                result.Add((long[])combination.Clone());

            }
            else
            {
                if (parallel)
                {
                    Object obj = new Object();
                    Parallel.For(0, division + 1, k =>
                    {
                        var clone = (long[])combination.Clone();
                        clone[index] = k;
                        long remainder = number - Coins[index] * k;
                        if (remainder == 0)
                        {
                            for (int i = index - 1; i >= 0; i--)
                            {
                                clone[i] = 0;
                            }

                            lock (obj)
                            {
                                result.Add(clone);
                            }

                        }
                        else
                        {
                            var collection = GetCombi2(remainder, index - 1, clone, cachedValues, false);
                            lock (obj)
                            {
                                result.AddRange(collection);
                            }

                        }
                    });
                }
                else
                {
                    for (var k = division; k >= 0; k--)
                    {
                        var clone = (long[])combination.Clone();
                        clone[index] = k;
                        long remainder = number - Coins[index] * k;
                        if (remainder == 0)
                        {
                            for (int i = index - 1; i >= 0; i--)
                            {
                                clone[i] = 0;
                            }


                            result.Add(clone);


                        }
                        else
                        {
                            var collection = GetCombi2(remainder, index - 1, clone, cachedValues, false);
                            result.AddRange(collection);


                        }
                    }

                }
            }

            return result;


        }
    }
}