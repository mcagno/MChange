using MagicPurse.Library;
using NUnit.Framework;

namespace MagicPurse.Tests
{
    [TestFixture]
    public class MagicPurseQueueTests : MagicPursesTestsBase
    {
        public override IMagicPurse CreatePurse()
        {
            return new MagicPurseQueue(new Splitter());
        }
    }
}