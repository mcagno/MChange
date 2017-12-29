using System.Linq;

namespace MagicPurse.Library
{
    public interface ISplitter
    {
        long GetNumberOfEqualSplits(long[] combination);
    }

    public class Splitter : ISplitter
    {
        public long GetNumberOfEqualSplits(long[] combination)
        {
            var sum = combination.Sum();
            return GetNumberOfEqualSplits(combination, 0, sum / 2, sum, new long[combination.Length, sum], false);
        }

        private long GetNumberOfEqualSplits(long[] combi, int index, long remaining, long remainingInCombi, long[,] cachedValues, bool parallel)
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

                for (long j = upper; j >= limit; j--)
                {
                    var halfCombis = GetNumberOfEqualSplits(combi, index + 1, remaining - j, remainingInCombi, cachedValues, false);
                    result += halfCombis;
                }

            }

            cachedValues[index, remaining] = result;
#if VERBOSE
            Console.Write($"I:{index} R:{remaining}");
            Console.WriteLine(" calculated.");
#endif
            return result;
        }
    }
}
