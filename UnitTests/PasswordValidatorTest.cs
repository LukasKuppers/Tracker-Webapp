using Xunit;
using Tracker_Server.Services.Authorization;
using Xunit.Sdk;
using System.Collections.Generic;
using System;

namespace UnitTests
{
    public class PasswordValidatorTest
    {
        [Fact]
        public void TestValidPassword()
        {
            PasswordValidator pv = new PasswordValidator();
            string password = "Password1";

            Assert.True(pv.IsValid(password));
        }

        [Fact]
        public void TestBadInput()
        {
            PasswordValidator pv = new PasswordValidator();

            Assert.False(pv.IsValid(""));

            Assert.Throws<ArgumentNullException>(() =>
                pv.IsValid(null)
            );
        }

        [Fact]
        public void TestShortPassword()
        {
            PasswordValidator pv = new PasswordValidator();
            string password = "Pass1#";

            Assert.False(pv.IsValid(password));
        }

        [Fact]
        public void TestNoLowerCase()
        {
            PasswordValidator pv = new PasswordValidator();
            string password = "PASSWORD1";

            Assert.False(pv.IsValid(password));
        }

        [Fact]
        public void TestNoUpperCase()
        {
            PasswordValidator pv = new PasswordValidator();
            string password = "password1";

            Assert.False(pv.IsValid(password));
        }

        [Fact]
        public void TestNoDigit()
        {
            PasswordValidator pv = new PasswordValidator();
            string password = "myawesomepassword";

            Assert.False(pv.IsValid(password));
        }

        [Fact]
        public void TestBadPasswords()
        {
            PasswordValidator pv = new PasswordValidator();
            List<string> passwords = new List<string>
            {
                "trashpanda", 
                "lsfj%^", 
                "AS;KFJEIEJ", 
                "1234", 
                "23465103701934875", 
                "THISISAGOODPASSWORDRIGHT"
            };

            foreach(string password in passwords)
            {
                Assert.False(pv.IsValid(password), "password: " + password + "passed validation");
            }
        }
    }
}
