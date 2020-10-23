using Autofac.Extras.Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker_Server.Models.Users;
using Tracker_Server.Services.Authorization;
using Tracker_Server.Services.Constants;
using Xunit;
using Xunit.Sdk;

namespace UnitTests
{
    public class AuthServiceTest
    {
        [Fact]
        public void Test_IsValidUser_ValidUser()
        {
            // arrange

            HashManager manager = new HashManager();

            string email = "Jon@bones.com";
            string password = "ThaChamp123";
            Guid pwdSalt = new Guid();
            List<User> users = new List<User> 
            { 
                new User {  
                    Id = new Guid(),
                    Credentials = new UserCredentials 
                        { 
                            PwdHash = manager.GetHash(pwdSalt.ToString(), password, 0), 
                            PwdSalt = pwdSalt
                        }, 
                    Email = email, 
                    Username = "JonnyBonesJones",
                    Projects = new List<Guid>()
                } 
            };

            var mockDbClient = new MockDBClient()
                .MockContains<User, string>("users", "Email", email, true)
                .MockFindByField<User, string>("users", "Email", email, users);

            var mockResource = new MockResource().GetDefaultConfig();

            // act
            AuthService authService = new AuthService(mockDbClient.Object, mockResource.Object);
            bool result = authService.IsValidUser(email, password);

            // assert
            Assert.True(result);
            mockDbClient.VerifyAll();
        }

        //[Fact]
        public void Test_IsValidUser_NullInput()
        {
            var mockDbClient = new MockDBClient()
                .MockContains<User, string>("users", "Email", null, false)
                .MockFindByField<User, string>("users", "Email", null, null);

            var mockResource = new MockResource().GetDefaultConfig();

            AuthService authService = new AuthService(mockDbClient.Object, mockResource.Object);

            Assert.Throws<ArgumentNullException>(() =>
                authService.IsValidUser(null, null)
            );

            Assert.Throws<ArgumentNullException>(() =>
                authService.IsValidUser("Jon@Bones.com", null)
            );

            Assert.Throws<ArgumentNullException>(() =>
                authService.IsValidUser(null, "MyAwesomePassword")
            );
        }

        [Fact]
        public void Test_IsValidUser_UserNotFound()
        {
            string email = "Jon@Jones.com";

            var mockDbClient = new MockDBClient()
                .MockContains<User, string>("users", "Email", email, false)
                .MockFindByField<User, string>("users", "Email", email, new List<User>());

            var mockResource = new MockResource().GetDefaultConfig();

            AuthService authService = new AuthService(mockDbClient.Object, mockResource.Object);
            bool result = authService.IsValidUser(email, "password123");

            Assert.True(result == false);
        }

        [Fact]
        public void Test_IsValidUser_InvalidPassword()
        {
            HashManager manager = new HashManager();

            string email = "Jon@bones.com";
            string password = "ThaChamp123";
            Guid pwdSalt = new Guid();
            List<User> users = new List<User>
            {
                new User {
                    Id = new Guid(),
                    Credentials = new UserCredentials
                        {
                            PwdHash = manager.GetHash(pwdSalt.ToString(), "Dope123", 0),
                            PwdSalt = pwdSalt
                        },
                    Email = email,
                    Username = "JonnyBonesJones",
                    Projects = new List<Guid>()
                }
            };

            var mockDbClient = new MockDBClient()
                .MockContains<User, string>("users", "Email", email, true)
                .MockFindByField<User, string>("users", "Email", email, users);

            var mockResource = new MockResource().GetDefaultConfig();

            AuthService authService = new AuthService(mockDbClient.Object, mockResource.Object);
            bool result = authService.IsValidUser(email, password);

            Assert.True(result == false);
        }
    }
}
