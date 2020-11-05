using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTests.Mocks
{
    class MockHttpCookies : Mock<HttpContext>
    {
        public MockHttpCookies MockGetCookie(string key, string output)
        {
            Setup(x => x.Request.Cookies[key])
                .Returns(output);
            return this;
        }
    }
}
