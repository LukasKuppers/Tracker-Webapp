using Xunit;
using Tracker_Server.Services.Authorization;
using System;
using UnitTests.Mocks;

namespace UnitTests
{
    public class AuthHeaderReaderTest
    {
        private static readonly string HEADER_KEY = "Authorization";

        [Fact]
        public void Test_BadInput()
        {
            ISessionParser sessionParser = new AuthHeaderReader();

            Assert.Throws<ArgumentNullException>(() =>
                sessionParser.GetSessionID(null)    
            );
        }

        [Fact]
        public void Test_NoHeader()
        {
            ISessionParser sessionParser = new AuthHeaderReader();

            var mockHttp = new MockHttpHeaders()
                .MockGetHeader(HEADER_KEY, false, "");

            Guid sessionID = sessionParser.GetSessionID(mockHttp.Object);
            Assert.Equal(sessionID, Guid.Empty);
        }

        [Fact]
        public void Test_InvalidId()
        {
            ISessionParser sessionParser = new AuthHeaderReader();

            var mockHttp = new MockHttpHeaders()
                .MockGetHeader(HEADER_KEY, true, "asf");

            Guid sessionID = sessionParser.GetSessionID(mockHttp.Object);
            Assert.Equal(sessionID, Guid.Empty);
        }

        [Fact]
        public void Test_GoodInput()
        {
            Guid id = Guid.NewGuid();
            ISessionParser sessionParser = new AuthHeaderReader();

            var mockHttp = new MockHttpHeaders()
                .MockGetHeader(HEADER_KEY, true, id.ToString());

            Guid sessionID = sessionParser.GetSessionID(mockHttp.Object);
            Assert.Equal(sessionID, id);
        }
    }
}
