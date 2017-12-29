using MagicPurse.Library;
using NUnit.Framework;

namespace MagicPurse.Tests
{
    [TestFixture]
    public class MagicPurseAsync : MagicPursesTestsBase
    {
        public override IMagicPurse CreatePurse()
        {
            return new Library.MagicPurseAsync(new Splitter());
        }
    }
}