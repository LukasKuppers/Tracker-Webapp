using System;
using Tracker_Server.Services.Authorization;
using Xunit;

namespace UnitTests
{
    public class HashManagerTest
    {
        [Fact]
        public void TestValidInput()
        {
            IHashManager hashManager = new HashManager();

            string hash1 = hashManager.GetHash("saltStr", "passwordStr", 0);
            
            Assert.True(hash1 != null);
            Assert.True(hash1 != "");

            string hash2 = hashManager.GetHash("saltStr2", "passwordStr2", 0);

            Assert.False(hash1 == hash2);
        }

        [Fact]
        public void TestErroneousInput()
        {
            IHashManager hashManager = new HashManager();

            string hash = hashManager.GetHash("", "", 0);

            Assert.True(hash != null);
            Assert.True(hash != "");

            hash = hashManager.GetHash("!(&#E%&)&", "!)(*E^(~`?", 0);

            Assert.True(hash != null);
            Assert.True(hash != "");
        }

        [Fact]
        public void TestInvalidInput()
        {
            IHashManager hashManager = new HashManager();

            Assert.Throws<ArgumentNullException>(() =>
                hashManager.GetHash(null, null, 0)
            );

            Assert.Throws<ArgumentNullException>(() =>
                hashManager.GetHash("teststring", null, 0)
            );

            Assert.Throws<ArgumentNullException>(() =>
                hashManager.GetHash(null, "teststring", 0)
            );

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                hashManager.GetHash("teststring", "teststring", -1)
            );
        }
    }
}
