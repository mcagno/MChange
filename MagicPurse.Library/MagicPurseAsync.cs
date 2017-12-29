using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagicPurse.Library
{
    public class MagicPurseAsync : MagicPurseBase, IMagicPurse
    {
        public MagicPurseAsync(ISplitter splitter)
        {
            Splitter = splitter;
        }

        public long GetAllSplits(long number)
        {
            var combination = new long[Coins.Length];
            var combinations = GetCombinations(number, Coins.Length - 1, combination, true);
            combinations.RemoveAll(l => l.Sum() % 2 != 0);
            long sum = 0;
            Parallel.ForEach(combinations, combi =>
            {
                var halfCombis = Splitter.GetNumberOfEqualSplits(combi);
                Interlocked.Add(ref sum, halfCombis);
            });
            return sum;
        }

        private List<long[]> GetCombinations(long number, int index, long[] combination, bool parallel)
        {
            var result = new List<long[]>();
            var division = number / Coins[index];
            if (index == 0)
            {
                combination[index] = division;
                result.Add((long[])combination.Clone());
            }
            else
            {
                if (parallel)
                {
                    var lockedObj = new object();
                    Parallel.For(0, division + 1, k =>
                    {
                        var clonedCombination = (long[]) combination.Clone();
                        clonedCombination[index] = k;
                        var remainder = number - Coins[index] * k;
                        if (remainder == 0)
                        {
                            for (int i = index - 1; i >= 0; i--)
                            {
                                clonedCombination[i] = 0;
                            }

                            lock (lockedObj)
                            {
                                result.Add(clonedCombination);
                            }
                        }
                        else
                        {
                            var childCombinations = GetCombinations(remainder, index - 1, clonedCombination, false);
                            lock (lockedObj)
                            {
                                result.AddRange(childCombinations);
                            }

                        }
                    });
                }
                else
                {
                    for (var k = division; k >= 0; k--)
                    {
                        var clonedCombination = (long[]) combination.Clone();
                        clonedCombination[index] = k;
                        var remainder = number - Coins[index] * k;
                        if (remainder == 0)
                        {
                            for (int i = index - 1; i >= 0; i--)
                            {
                                clonedCombination[i] = 0;
                            }

                            result.Add(clonedCombination);
                        }
                        else
                        {
                            var childCombinations = GetCombinations(remainder, index - 1, clonedCombination, false);
                            result.AddRange(childCombinations);
                        }
                    }

                }
            }
            return result;
        }
    }
}