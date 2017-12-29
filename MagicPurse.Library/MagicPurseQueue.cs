using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagicPurse.Library
{
    
    public class MagicPurseQueue : MagicPurseBase, IMagicPurse
    {
        private bool _fillQueueInProgress;
        private long _numberOfSplits;
        private readonly CombinationCache _combinationCache = new CombinationCache();
        private readonly Queue<long[]> _combinationQueue = new Queue<long[]>();

        public MagicPurseQueue(ISplitter splitter)
        {
            Splitter = splitter;
        }

        public long GetAllSplits(long number)
        {
            InitializeForCalculation(number);
            Calculate(number);
            return _numberOfSplits;
        }

        private void Calculate(long number)
        {
            Parallel.Invoke(
                () => { FillQueue(number); },
                ProcessQueue
            );
        }

        private void ProcessQueue()
        {
            while (true)
            {
                
                lock (_combinationQueue)
                {
                    if (!_fillQueueInProgress && _combinationQueue.Count <= 0) break;
                }

                lock (_combinationQueue)
                {
                    if (_combinationQueue.Count <= 0) continue;
                }

                long[][] array;
                lock (_combinationQueue)
                {
                    array = _combinationQueue.ToArray();
                    _combinationQueue.Clear();
                }
                
                var orderablePartitioner = Partitioner.Create(array);
                

                Parallel.ForEach(orderablePartitioner, combination =>
                {
                    var numberOfEqualSplits = Splitter.GetNumberOfEqualSplits(combination);
                    Interlocked.Add(ref _numberOfSplits, numberOfEqualSplits);
                });

            }
        }

        private void FillQueue(long number)
        {
            _fillQueueInProgress = true;
            FillQueue(number, Coins.Length - 1, new long[Coins.Length]);
            _fillQueueInProgress = false;
        }

        private void InitializeForCalculation(long number)
        {
            lock (_combinationQueue)
            {
                _combinationQueue.Clear();
            }

            _combinationCache.Initialize(Coins.Length, number + 1);
            _fillQueueInProgress = true;
            _numberOfSplits = 0;
        }


        private void FillQueue(long number, int index, long[] combination)
        {
            var cachedValue = _combinationCache.Get(index, number);
            if (cachedValue != null)
            {
                foreach (var combi in cachedValue)
                {
                    var clonedCombination = (long[]) combination.Clone();
                    Array.Copy(combi, 0, clonedCombination, 0, combi.Length);
                    Enqueue(clonedCombination);
                }
                return;
            }

            var maxNumberOfCoins = number / Coins[index];
            
            if (index == 0)
            {
                combination[index] = maxNumberOfCoins;

                _combinationCache.Add(index, number, new []{maxNumberOfCoins});
                Enqueue((long[]) combination.Clone());
                return;
            }


            for (var k = maxNumberOfCoins; k >= 0; k--)
            {
                combination[index] = k;
                var remainder = number - Coins[index] * k;
                if (remainder == 0)
                {
                    for (int i = index - 1; i >= 0; i--)
                    {
                        combination[i] = 0;
                    }

                    var cached = new long[index + 1];
                    Array.Copy(combination, cached, index + 1);
                    _combinationCache.Add(index, number, cached);

                    Enqueue((long[]) combination.Clone());
                }
                else
                {
                    FillQueue(remainder, index - 1, combination);
                    var cachedList = _combinationCache.Get(index - 1, remainder);
                    foreach (var longse in cachedList)
                    {
                        var newCombination = new long[index + 1];
                        Array.Copy(longse, newCombination, index);
                        newCombination[index] = k;
                        _combinationCache.Add(index, number, newCombination);
                    }
                }
            }
        }

        private void Enqueue(long[] clone)
        {
            lock (_combinationQueue)
            {
                _combinationQueue.Enqueue(clone);
            }
        }
    }
}
