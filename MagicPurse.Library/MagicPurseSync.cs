namespace MagicPurse.Library
{
    public class MagicPurseSync : MagicPurseBase, IMagicPurse
    {
        public MagicPurseSync(ISplitter splitter)
        {
            Splitter = splitter;
        }        

        public long GetAllSplits(long number)
        {
            return GetAllSplits(number, Coins.Length - 1, new long[Coins.Length]);
        }


        private long GetAllSplits(long number, int currentIndex, long[] combination)
        {
            var division = number / Coins[currentIndex];
            if (currentIndex == 0)
            {
                combination[currentIndex] = division;
                return Splitter.GetNumberOfEqualSplits(combination);   
                
            }

            long result = 0;
            for (var k = division; k >= 0; k--)
            {
                combination[currentIndex] = k;
                long remainder = number - k * Coins[currentIndex];
                if (remainder == 0)
                {
                    for (var i = currentIndex - 1; i >= 0; i--)
                    {
                        combination[i] = 0;
                    }

                    result += Splitter.GetNumberOfEqualSplits(combination);
                }
                else
                {
                    result += GetAllSplits(remainder, currentIndex - 1, combination);
                }
            }
            return result;
        }

    }
}
