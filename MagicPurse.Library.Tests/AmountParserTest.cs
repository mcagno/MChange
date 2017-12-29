using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicPurse.Library;
using NUnit.Framework;

namespace MagicPurse.Tests
{
    [TestFixture]
    class AmountParserTest
    {
        [Test]
        [TestCase("1/-/-", 480)]
        [TestCase("5/-/-", 2400)]
        [TestCase("10/-/-", 4800)]
        [TestCase("1/1/-", 504)]
        [TestCase("1/1/1", 506)]
        [TestCase("1d", 2)]
        [TestCase("2d", 4)]
        [TestCase("1/1", 26)]
        [TestCase("2/2", 52)]
        public void Parse_ValidInput_Returns_ExpectedResult(string amount, long expected)
        {
            AmountParser parser = new AmountParser();
            var result = parser.ParseAndGetNumberOfHalfPence(amount);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("a")]
        [TestCase("b/b/c")]
        [TestCase("test")]
        [TestCase("2/a/2")]
        [TestCase("2/a/2cc")]
        [TestCase("2/2/2/3/5")]
        public void Parse_ValidInput_Throws_ArgumentException(string amount)
        {
            AmountParser parser = new AmountParser();
            Assert.Throws<ArgumentException>(() => parser.ParseAndGetNumberOfHalfPence(amount));
        }
    }
}
