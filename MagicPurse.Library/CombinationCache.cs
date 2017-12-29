using System.Collections.Generic;

namespace MagicPurse.Library
{
    internal class CombinationCache
    {
        private List<long[]>[,] _cache;

        public void Initialize(long elements, long number)
        {
            _cache = new List<long[]>[elements, number];
        }

        public List<long[]> Get(int index, long number)
        {
            return _cache[index, number];
        }

        public void Add(int index, long number, long[] combination)
        {
            if (_cache[index, number] == null)
            {
                _cache[index, number] = new List<long[]>();
            }
            _cache[index, number].Add(combination);
        }
    }
}