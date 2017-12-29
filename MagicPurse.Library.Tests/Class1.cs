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
        
        
        [TestCase(6, 4, Category = "Example test")]
        [TestCase(20, 77, Category = "Example test")]
        [TestCase(24, 141, Category = "Example test")]
        [TestCase(48, 2377, Category = "Example test")]
        [TestCase(240, 54802414, Category = "Real tests")]
        [TestCase(1 * 20 * 12 * 2, 22982398853, Category = "Real tests")]
        [TestCase(10 * 20 * 12 * 2, 0, Category = "Real tests")]
        public void FirstTest(long number, long expectedResult)
        {
            Library.ChangeMaker purse =new Library.ChangeMaker();
            long result = purse.MakeEvenChange(number);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetBasicCombi()
        {
            Library.ChangeMaker purse = new Library.ChangeMaker();
            long[][] result = purse.GetBasicCombinations();
            
        }
    }
}
