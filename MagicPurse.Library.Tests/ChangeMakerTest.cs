using MagicPurse.Library;
using NUnit.Framework;

namespace MagicPurse.Tests
{
    [TestFixture]
    public class ChangeMakerTest
    {
        [Test]
        [TestCase(6, 4, Category = "Example test")]
        [TestCase(20, 77, Category = "Example test")]
        [TestCase(24, 141, Category = "Example test")]
        [TestCase(48, 2377, Category = "Example test")]
        [TestCase(240, 54802414, Category = "Real tests")]
        [TestCase(1 * 20 * 12 * 2, 22982398853, Category = "Real tests")]
        //[TestCase(10 * 20 * 12 * 2, 0, Category = "Real tests")]
        public void SyncChangeMake(long number, long expectedResult)
        {
            Library.ChangeMaker purse =new Library.ChangeMaker(new Splitter());
            long result = purse.MakeEvenChange(number);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(6, 4, Category = "Example test")]
        [TestCase(20, 77, Category = "Example test")]
        [TestCase(24, 141, Category = "Example test")]
        [TestCase(48, 2377, Category = "Example test")]
        [TestCase(240, 54802414, Category = "Real tests")]
        [TestCase(1 * 20 * 12 * 2, 22982398853, Category = "Real tests")]
        //[TestCase(10 * 20 * 12 * 2, 0, Category = "Real tests")]
        public void AsyncChangeMake(long number, long expectedResult)
        {
            ChangeMakerAsync purse = new ChangeMakerAsync(new Splitter());
            long result = purse.MakeEvenChange(number);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(6, 4, Category = "Example test")]
        [TestCase(20, 77, Category = "Example test")]
        [TestCase(24, 141, Category = "Example test")]
        [TestCase(48, 2377, Category = "Example test")]
        [TestCase(240, 54802414, Category = "Real tests")]
        [TestCase(1 * 20 * 12 * 2, 22982398853, Category = "Real tests")]
        //[TestCase(10 * 20 * 12 * 2, 0, Category = "Real tests")]
        public void QueueChangeMake(long number, long expectedResult)
        {
            ChangeMakerQueue purse = new ChangeMakerQueue(new Splitter());
            long result = purse.MakeEvenChange(number);
            Assert.AreEqual(expectedResult, result);
        }


    }
}
