using MagicPurse.Library;
using NUnit.Framework;

namespace MagicPurse.Tests
{
    [TestFixture]
    public class MagicPurseAsyncTests : MagicPursesTestsBase
    {
        public override IMagicPurse CreatePurse()
        {
            return new MagicPurseAsync(new Splitter());
        }
    }
}