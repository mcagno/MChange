using System.Linq;

namespace MagicPurse.Library
{
    public class Splitter : ISplitter
    {
        public long GetNumberOfEqualSplits(long[] combination)
        {
            var combinationSum = combination.Sum();
            if (combinationSum == 0 || combinationSum % 2 != 0)
                return 0;
            return GetNumberOfEqualSplits(combination, 0, combinationSum / 2, combinationSum, new long[combination.Length, combinationSum]);
            
        }

        private long GetNumberOfEqualSplits(long[] combination, int index, long remaining, long remainingInNextIndexes, long[,] cache)
        {

            long result = 0;

            var cachedValue = cache[index, remaining];
            if (cachedValue != 0)
            {
                return cachedValue;
            }

            while (combination[index] == 0)
            {
                index++;
            }

            long upperLimit;
            if (combination[index] >= remaining)
            {
                upperLimit = remaining - 1;
                result++;
            }
            else
            {
                upperLimit = combination[index];
            }

            if (index < combination.Length - 1)
            {
                remainingInNextIndexes -= combination[index];
                var lowerLimit = remaining - remainingInNextIndexes;
                if (lowerLimit < 0)
                {
                    lowerLimit = 0;
                }

                for (long j = upperLimit; j >= lowerLimit; j--)
                {
                    var halfCombis = GetNumberOfEqualSplits(combination, index + 1, remaining - j, remainingInNextIndexes, cache);
                    result += halfCombis;
                }

            }
            cache[index, remaining] = result;
            return result;
        }
    }
}
