using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicPurse.Library
{
    public class ChangeMakerQueue : ChangeMakerBase, IChangeMaker
    {

        private readonly Queue<long[]> _combinationQueue = new Queue<long[]>();

        public ChangeMakerQueue(ISplitter splitter)
        {
            Splitter = splitter;
        }

        private bool _inProgress;
        private long _sum;
        private List<long[]>[,] _combinationCache;

        public long MakeEvenChange(long number)
        {
            _combinationQueue.Clear();
            _combinationCache = new List<long[]>[Coins.Length, number + 1];
            _inProgress = true;
            _sum = 0;




            Parallel.Invoke(() =>
                {
                    FillQueue(number, Coins.Length - 1, new long[Coins.Length], _combinationCache);
                    _inProgress = false;
                },
                () =>
                {
                    while (_inProgress || _combinationQueue.Count > 0)
                    {
                        if (_combinationQueue.Count > 0)
                        {
                            long[] combination;
                            lock (_combinationQueue)
                            {
                                combination = _combinationQueue.Dequeue();
                            }

                            _sum += Splitter.GetNumberOfEqualSplits(combination);
                        }
                    }
                }


                );
            return _sum;

        }


        private void FillQueue(long number, int index, long[] combination, List<long[]>[,] cachedValues)
        {
            //Console.WriteLine($"I:{index} N:{number}");
            
            //Try getting from cache
            List<long[]> cachedValue = cachedValues[index, number];
            if (cachedValue != null)
            {
                foreach (var combi in cachedValue)
                {
                    var clone = (long[]) combination.Clone();
                    Array.Copy(combi, 0, clone, 0, combi.Length);
                    if (clone.Sum() % 2 == 0)
                    {
                        lock (_combinationQueue)
                        {
                            _combinationQueue.Enqueue(clone);
                        }
                    }
                }
                return;
            }


            long division = number / Coins[index];
            if (cachedValues[index, number] == null)
                cachedValues[index, number] = new List<long[]>();
            if (index == 0)
            {
                combination[index] = division;

                cachedValues[index, number].Add(new []{division});
                if (combination.Sum() % 2 == 0)
                {
                    var clone = (long[]) combination.Clone();
                    lock (_combinationQueue)
                    {
                        _combinationQueue.Enqueue(clone);
                    }
                                       
                    //return Splitter.GetNumberOfEqualSplits(combination);   
                }
                return;
            }


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

                    long[] cached = new long[index + 1];
                    Array.Copy(combination, cached, index + 1);
                    cachedValues[index, number].Add(cached);



                    if (combination.Sum() % 2 == 0)
                    {
                        var clone = (long[])combination.Clone();
                        lock (_combinationQueue)
                        {
                            _combinationQueue.Enqueue(clone);
                        }
                    }
                }
                else
                {
                    FillQueue(remainder, index - 1, combination, cachedValues);
                    var longses = cachedValues[index - 1, remainder];
                    foreach (var longse in longses)
                    {
                        long[] cached = new long[index + 1];
                        Array.Copy(longse, cached, index);
                        cached[index] = k;
                        cachedValues[index, number].Add(cached);
                    }
                }







            }
            //}



        }

        private void PrintCombination(long[] combi)
        {

            string res = combi.Aggregate(string.Empty, (current, l) => l.ToString("000") + "|" + current);
            Console.WriteLine(res);
        }

        



    }
}
