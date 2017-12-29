using MagicPurse.Library;
using NUnit.Framework;

namespace MagicPurse.Tests
{
    [TestFixture]
    public class MagicPurseSyncTests : MagicPursesTestsBase
    {
        public override IMagicPurse CreatePurse()
        {
            return new MagicPurseSync(new Splitter());
        }
    }
}