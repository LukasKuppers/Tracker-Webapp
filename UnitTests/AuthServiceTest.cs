using Autofac.Extras.Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker_Server.Models.Users;
using Tracker_Server.Services.Authorization;
using Tracker_Server.Services.Constants;
using Tracker_Server.Services.DataAccess;
using Xunit;

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
                .MockContains<User, string>(Resource.getString("db_users_path"), "Email", email, true)
                .MockFindByField<User, string>(Resource.getString("db_users_path"), "Email", email, users);

            // act
            AuthService authService = new AuthService(mockDbClient.Object);
            bool result = authService.IsValidUser(email, password);

            // assert
            Assert.True(result);
            mockDbClient.VerifyAll();
        }
    }
}
