using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MagicPurse.Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        [TestCase(6, 4)]
        [TestCase(20, 77)]
        [TestCase(24, 141)]
        [TestCase(48, 2377)]
        public void FirstTest(long number, long expectedResult)
        {
            MagicPurse purse =new MagicPurse();
            long result = purse.MakeEvenChange(number);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
