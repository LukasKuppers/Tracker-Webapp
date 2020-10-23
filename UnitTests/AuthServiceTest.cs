using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using System;
using System.Collections.Generic;
using System.IO;
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

        [Fact]
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

        [Fact]
        public void Test_IsValidUser_MultipleUsers()
        {
            IHashManager manager = new HashManager();
            string email = "Jon@Bones.com";
            string password = "Password";
            Guid pwdSalt = new Guid();
            User user = new User
            {
                Id = new Guid(),
                Credentials = new UserCredentials
                {
                    PwdHash = manager.GetHash(pwdSalt.ToString(), password, 0),
                    PwdSalt = pwdSalt
                },
                Email = email,
                Username = "JonnyBonesJones",
                Projects = new List<Guid>()
            };
            List<User> users = new List<User>();
            users.Add(user);
            users.Add(user);

            var mockDbClient = new MockDBClient()
                .MockContains<User, string>("users", "Email", email, true)
                .MockFindByField<User, string>("users", "Email", email, users);

            var mockResource = new MockResource().GetDefaultConfig();

            AuthService authService = new AuthService(mockDbClient.Object, mockResource.Object);

            Assert.Throws<InvalidDataException>(() =>
                authService.IsValidUser(email, password)
            );
        }

        [Fact]
        public void Test_CreateSession_ValidEmailNoSession()
        {
            string email = "Jon@Jones.com";
            List<User> users = new List<User>
            {
                new User
                {
                    Id = new Guid(), 
                    Email = email, 
                    Username = "jonny", 
                    Credentials = new UserCredentials
                    {
                        PwdSalt = new Guid(), 
                        PwdHash = "testhash"
                    }, 
                    Projects = new List<Guid>()
                }
            };

            var mockDbClient = new MockDBClient()
                .MockContains<User, string>("users", "Email", email, true)
                .MockFindByField<User, string>("users", "Email", email, users)
                .MockContains<Session, Guid>("sessions", "UserId", users[0].Id, false);

            var mockResource = new MockResource().GetDefaultConfig();

            AuthService authService = new AuthService(mockDbClient.Object, mockResource.Object);
            Guid sessionID = authService.CreateSession(email);

            Assert.True(sessionID != null);
            Assert.True(sessionID != Guid.Empty);
        }

        [Fact]
        public void Test_CreateSessoin_ValidEmailExistingSession()
        {
            string email = "Jon@Jones.com";
            List<User> users = new List<User>
            {
                new User
                {
                    Id = new Guid(),
                    Email = email,
                    Username = "jonny",
                    Credentials = new UserCredentials
                    {
                        PwdSalt = new Guid(),
                        PwdHash = "testhash"
                    },
                    Projects = new List<Guid>()
                }
            };

            Session session = new Session
            {
                UserId = users[0].Id,
                Id = new Guid()
            };

            var mockDbClient = new MockDBClient()
                .MockContains<User, string>("users", "Email", email, true)
                .MockFindByField<User, string>("users", "Email", email, users)
                .MockContains<Session, Guid>("sessions", "UserId", users[0].Id, true)
                .MockFindByField<Session, Guid>("sessions", "UserId", users[0].Id, new List<Session> { session });

            var mockResource = new MockResource().GetDefaultConfig();

            AuthService authService = new AuthService(mockDbClient.Object, mockResource.Object);
            Guid sessionId = authService.CreateSession(email);

            Assert.True(sessionId == session.Id);
        }

        [Fact]
        public void Test_CreateSession_InvalidEmail()
        {
            string email = "Jon@Jones.com";

            var mockDbClient = new MockDBClient()
                .MockContains<User, string>("users", "Email", email, false);

            var mockResource = new MockResource().GetDefaultConfig();

            AuthService authService = new AuthService(mockDbClient.Object, mockResource.Object);
            Guid sessionId = authService.CreateSession(email);

            Assert.True(sessionId != null);
            Assert.True(sessionId == Guid.Empty);
        }

        [Fact]
        public void Test_CreateSession_NullInput()
        {
            string email = null;

            var mockDbClient = new MockDBClient();
            var mockResource = new MockResource().GetDefaultConfig();

            AuthService authService = new AuthService(mockDbClient.Object, mockResource.Object);

            Assert.Throws<ArgumentNullException>(() =>
                authService.CreateSession(email)
            );
        }

        [Fact]
        public void Test_CreateSession_MultipleUsers()
        {
            string email = "Jon@Bones.com";
            User user = new User
            {
                Id = new Guid(),
                Email = email,
                Username = "Jonny",
                Credentials = new UserCredentials
                {
                    PwdSalt = new Guid(),
                    PwdHash = "testhash"
                },
                Projects = new List<Guid>()
            };
            List<User> users = new List<User>
            {
                user, user
            };

            var mockDbClient = new MockDBClient()
                .MockContains<User, string>("users", "Email", email, true)
                .MockFindByField<User, string>("users", "Email", email, users);

            var mockResource = new MockResource().GetDefaultConfig();

            AuthService authService = new AuthService(mockDbClient.Object, mockResource.Object);

            Assert.Throws<InvalidDataException>(() =>
                authService.CreateSession(email)
            );
        }
    }
}
