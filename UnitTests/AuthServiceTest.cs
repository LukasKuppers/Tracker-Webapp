using Autofac.Extras.Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class AuthServiceTest
    {
        [Fact]
        public void Test_IsValidUser_ValidUser()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<DBClien>
            }
        }
    }
}
