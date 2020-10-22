using Moq;
using Tracker_Server.Services.Constants;

namespace UnitTests
{
    class MockResource : Mock<IResource>
    {
        public MockResource GetDefaultConfig()
        {
            return new MockResource()
                .MockGetString("db_base_path", "tracker")
                .MockGetString("db_users_path", "users")
                .MockGetString("db_sessions_path", "sessions");
        }

        public MockResource MockGetString(string key, string output)
        {
            Setup(x => x.GetString(
                key
            )).Returns(output);
            return this;
        }
    }
}
