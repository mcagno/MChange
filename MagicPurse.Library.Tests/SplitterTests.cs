using MagicPurse.Library;
using NUnit.Framework;

namespace MagicPurse.Tests
{
    [TestFixture]
    class SplitterTests
    {
        [Test]
        [TestCase(new long[]{0, 6}, 1)]
        [TestCase(new long[]{2, 0}, 1)]
        [TestCase(new long[]{3, 1}, 2)]
        [TestCase(new long[]{2, 2}, 3)]
        public void GetNumberOfEqualSplits(long[] combination, long expectedValue)
        {
            Splitter splitter = new Splitter();
            var numberOfEqualSplits = splitter.GetNumberOfEqualSplits(combination);
            Assert.AreEqual(expectedValue, numberOfEqualSplits);
        }

    }
}
