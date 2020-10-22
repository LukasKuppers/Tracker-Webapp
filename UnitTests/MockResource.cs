using Moq;
using Tracker_Server.Services.Constants;

namespace UnitTests
{
    class MockResource : Mock<IResource>
    {
        public MockResource MockGetString(string key, string output)
        {
            Setup(x => x.GetString(
                key
            )).Returns(output);
            return this;
        }
    }
}
