using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTests.Mocks
{
    class MockHttpHeaders : Mock<HttpContext>
    {
        public MockHttpHeaders MockGetHeader(string key, bool retStatus, Microsoft.Extensions.Primitives.StringValues output)
        {
            Setup(x => x.Request.Headers.TryGetValue(key, out output))
                .Returns(retStatus);
            return this;
        }
    }
}
