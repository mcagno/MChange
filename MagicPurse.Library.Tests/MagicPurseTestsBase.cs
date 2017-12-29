using MagicPurse.Library;
using NUnit.Framework;

namespace MagicPurse.Tests
{
    public class MagicPurseTestsBase
    {
        protected void Test(IMagicPurse purse, long number, long expectedResult)
        {
            var result = purse.GetAllSplits(number);
            Assert.AreEqual(expectedResult, result);
        }
    }
}