using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Tracker_Server.Services.Authorization;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests
{
    public class CookieManagerTest
    {
        [Fact]
        public void TestGetCookieBadInput()
        {
            var mockHttp = new MockHttpCookies();

            var mockResource = new MockResource().GetDefaultConfig();
            CookieManager cookieManager = new CookieManager(mockResource.Object);

            Assert.Throws<ArgumentNullException>(() =>
                cookieManager.GetCookie(mockHttp.Object, null)
            );

            Assert.Throws<ArgumentNullException>(() =>
                cookieManager.GetCookie(mockHttp.Object, "")
            );

            Assert.Throws<ArgumentNullException>(() =>
                cookieManager.GetCookie(null, "valid key")
            );
        }

        [Fact]
        public void TestGetCookieValidInput()
        {
            string key = "mycookie";
            string value = "the cookie value";

            var mockHttp = new MockHttpCookies()
                .MockGetCookie(key, value);
            var mockResource = new MockResource().GetDefaultConfig();

            CookieManager cookieManager = new CookieManager(mockResource.Object);

            string output = cookieManager.GetCookie(mockHttp.Object, key);

            Assert.Equal(value, output);
        }

        [Fact]
        public void TestGetSessionIDBadInput()
        {
            var mockResource = new MockResource().GetDefaultConfig();

            CookieManager cookieManager = new CookieManager(mockResource.Object);

            Assert.Throws<ArgumentNullException>(() =>
                cookieManager.GetSessionID(null)
            );
        }

        [Fact]
        public void TestGetSessionIDBadCookies()
        {
            var mockResource = new MockResource().GetDefaultConfig();
            var mockHttp = new MockHttpCookies()
                .MockGetCookie(mockResource.Object.GetString("cookies_session_key"), null);

            CookieManager cookieManager = new CookieManager(mockResource.Object);
            Guid output = cookieManager.GetSessionID(mockHttp.Object);

            Assert.Equal(Guid.Empty, output);

            mockHttp = new MockHttpCookies()
                .MockGetCookie(mockResource.Object.GetString("cookies_session_key"), "aslkdfj");

            Assert.Throws<FormatException>(() =>
                cookieManager.GetSessionID(mockHttp.Object)
            );
        }

        [Fact]
        public void TestGetSessionGoodCookies()
        {
            Guid id = Guid.NewGuid();

            var mockResource = new MockResource().GetDefaultConfig();
            var mockHttp = new MockHttpCookies()
                .MockGetCookie(mockResource.Object.GetString("cookies_session_key"), id.ToString());

            CookieManager cookieManager = new CookieManager(mockResource.Object);
            Guid output = cookieManager.GetSessionID(mockHttp.Object);

            Assert.Equal(id, output);
        }
    }
}
