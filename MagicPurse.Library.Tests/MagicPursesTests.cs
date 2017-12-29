using MagicPurse.Library;
using NUnit.Framework;

namespace MagicPurse.Tests
{
    [TestFixture]
    public abstract class MagicPursesTestsBase
    {
        public abstract IMagicPurse CreatePurse();

        protected IMagicPurse Purse { get; set; }

        [SetUp]
        public void Initialize()
        {
            Purse = CreatePurse();
        }

        [Test]
        [TestCase(6, 4, Category = "Example test")]
        [TestCase(20, 77, Category = "Example test")]
        [TestCase(24, 141, Category = "Example test")]
        [TestCase(48, 2377, Category = "Example test")]
        [TestCase(240, 54802414, Category = "Real tests")]
        public void GetAllSplits_Returns_ExpectedResult(long number, long expectedResult)
        {
            var result = Purse.GetAllSplits(number);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
