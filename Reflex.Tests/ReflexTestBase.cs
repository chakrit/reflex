
using Reflex.Tests.Stubs;

namespace Reflex.Tests
{
    public class ReflexTestBase
    {
        private StubFactory _factory;

        public StubFactory Stubs
        {
            get { return _factory = _factory ?? new StubFactory(); }
        }
    }
}
