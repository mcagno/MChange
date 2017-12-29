using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagicPurse.Library
{
    public class ChangeMaker
    {
        private readonly long[] _coins =
        {
            1, //Halfpenny 0.5d
            2, //Penny 1d
            6, //Threepence 3d
            12, //Sixpence 6d
            24, //Shilling 1/-
            48, //Two shillings 
            60 //Two shillings and sixpence

        };

        private List<long[]> _combinations = new List<long[]>();

        public long[][] GetBasicCombinations()
        {
            long[][] basicCombi = new long[_coins.Length][];
            for (int i = _coins.Length - 1; i > 0; i--)
            {
                long[] combi = new long[_coins.Length];
                long remaining = _coins[i];
                for (var index = i - 1; index >= 0; index--)
                {
                    var coin = _coins[index];
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
            return GetCombi(number, _coins.Length - 1, new long[_coins.Length], null);
        }

        public long MakeEvenChange_ALT(long number)
        {

            long[] combination = new long[_coins.Length];

            List<long[]> combinations = GetCombi2(number, _coins.Length - 1, combination, null, true);
            combinations.RemoveAll(l => l.Sum() % 2 != 0);
            long sum = 0;
            Parallel.ForEach(combinations, combi =>
            {
                var halfCombis = GetHalfCombis(combi);
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
            long division = number / _coins[index];

            //if (cachedValues[index, number] != null)
            //{
            //    return GetHalfCombis(cachedValues[index, number]);
            //}

            if (index == 0)
            {
                combination[index] = division;

                result.Add((long[]) combination.Clone());

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
                        long remainder = number - _coins[index] * k;
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
                        long remainder = number - _coins[index] * k;
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


        private long GetHalfCombis(long[] combi, int index, long remaining, long remainingInCombi, long[,] cachedValues, bool parallel)
        {
            
            long result = 0;

            var cachedValue = cachedValues[index, remaining];
            if (cachedValue != 0)
            {
#if VERBOSE
                Console.Write($"I:{index} R:{remaining}");
                Console.WriteLine(" cached.");
#endif
                return cachedValue;
            }
            
            while (combi[index] == 0)
            {
                index++;
            }
            long upper;
            if (combi[index] >= remaining)
            {
                upper = remaining - 1;
                result++;
            }
            else
            {
                upper = combi[index];
            }
            
            if (index < combi.Length - 1)
            {
                remainingInCombi -= combi[index];
                var limit = remaining - remainingInCombi;
                if (limit < 0)
                {
                    limit = 0;
                }

                //if (parallel)
                //{
                //    Parallel.For(limit, upper + 1, j =>
                //    {
                //        var halfCombis = GetHalfCombis(combi, index + 1, remaining - j, remainingInCombi, cachedValues, false);
                //        Interlocked.Add(ref result, halfCombis);
                        
                //    });
                    
                //}
                //else
                //{
                    for (long j = upper; j >= limit; j--)
                    {
                        var halfCombis = GetHalfCombis(combi, index + 1, remaining - j, remainingInCombi, cachedValues, false);
                        result += halfCombis;
                    }
                //}

                /*
                if (limit < upper + 1)
                {


                    var partitioner = Partitioner.Create(limit, upper + 1);

                    Parallel.ForEach(partitioner, (range, loopState) =>
                        {
                            for (long j = range.Item1; j < range.Item2; j++)
                            {
                                var halfCombis = GetHalfCombis(combi, index + 1, remaining - j, remainingInCombi,
                                    cachedValues);
                                Interlocked.Add(ref result, halfCombis);
                            }

                        }
                    );
                }*/

                
                
            }

            cachedValues[index, remaining] = result;
#if VERBOSE
            Console.Write($"I:{index} R:{remaining}");
            Console.WriteLine(" calculated.");
#endif
            return result;
        }

        private void PrintCombination(long[] combi)
        {

            string res = combi.Aggregate(string.Empty, (current, l) => l.ToString("000") + "|" + current);
            Console.WriteLine(res);
        }


        private long GetCombi(long number, int index, long[] combination, List<long[]>[,] cachedValues)
        {
            //Console.WriteLine($"I:{index} N:{number}");
            long sum = 0;
            long division = number / _coins[index];

            //if (cachedValues[index, number] != null)
            //{
            //    return GetHalfCombis(cachedValues[index, number]);
            //}

            if (index == 0)
            {
                combination[index] = division;
                //cachedValues[index, number].Add((long[]) combination.Clone());
                if (combination.Sum() % 2 == 0)
                {
                    
                    //return 0;
                    
                    return GetHalfCombis(combination);
                    
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
                    long remainder = number - _coins[index] * k;
                    if (remainder == 0)
                    {
                        for (int i = index - 1; i >= 0; i--)
                        {
                            combination[i] = 0;
                        }



                        //cachedValues[index, remainder] = (long[]) combination.Clone();

                        if (combination.Sum() % 2 == 0)
                        {
                            sum += GetHalfCombis(combination);
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

        private long GetHalfCombis(long[] combination)
        {
#if VERBOSE
            

            PrintCombination(combination);
#endif
            long[,] storedValues = new long[combination.Length, combination.Sum()];
            return GetHalfCombis(combination, 0, combination.Sum() / 2, combination.Sum(), storedValues, false);
        }

        
    }

}
