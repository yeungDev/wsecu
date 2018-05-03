using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using John.Yeung.WsecuApplication;
using John.Yeung.WsecuApplication.Controllers;
using John.Yeung.WsecuApplication.Models;
using Moq;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace John.Yeung.WsecuApplication.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        /// <summary>
        /// Mock up a dataset
        /// </summary>
        /// <returns></returns>
        private Models.TestContext LoadContext()
        {
            var context = new Models.TestContext
            {
                Users =
                {
                    new User
                    {
                        Email = "email1@test.com",
                        Name = "Person1",
                        UserId = 1,
                        UserName = "Person1"
                    },
                    new User
                    {
                        Email = "email2@test.com",
                        Name = "Person2",
                        UserId = 2,
                        UserName = "Person2"
                    },
                    new User
                    {
                        Email = "email3@test.com",
                        Name = "Person3",
                        UserId = 3,
                        UserName = "Person3"
                    },
                    new User
                    {
                        Email = "email4@test.com",
                        Name = "Person4",
                        UserId = 4,
                        UserName = "Person4"
                    }
                }
            };

            return context;
        }


        [TestMethod]
        public void Get()
        {
            var context = LoadContext();

            UsersController controller = new UsersController(context);

            IEnumerable<User> result = controller.Get();

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count()); // Since we know there are 4
        }

        [TestMethod]
        public void GetByUserName()
        {
            var context = LoadContext();

            UsersController controller = new UsersController(context);

            var result = controller.Get("Person3");
            var userThatDoesntExist = controller.Get("idThatDoesntExist");

            Assert.AreEqual(3 , result.UserId);
            Assert.AreEqual("email3@test.com", result.Email);
            Assert.AreEqual("Person3", result.Name);
            
            Assert.IsNull(userThatDoesntExist);
        }

        [TestMethod]
        public void Post()
        {
            var context = LoadContext();

            var countBefore = context.Users.Count();

            UsersController controller = new UsersController(context);
            var user = new User
            {
                Email = "someEmail@email.com",
                UserName = "someUserName",
                Name = "Some Person"
            };

            controller.Post(user);

            var countAfter = context.Users.Count();

            Assert.AreNotEqual(countBefore, countAfter);
        }

        [TestMethod]
        public void PostDuplicate()
        {
            var context = LoadContext();

            var countBefore = context.Users.Count();

            UsersController controller = new UsersController(context);
            var user = new User
            {
                Email = "email1@test.com",
                UserName = "Person1",
                Name = "Some Person"
            };

            controller.Post(user);

            var countAfter = context.Users.Count();

            // Since the email already exists, the count before and after should be the same.
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod]
        public void Put()
        {
            var context = LoadContext();

            UsersController controller = new UsersController(context);

            var updatedName = "updatedName";
            var updatedEmail = "updatedEmail@email.com";

            var user = new User
            {
                Email = updatedEmail,
                Name = updatedName,
                UserName = "Person1"
            };

            var originalUser = controller.Get(user.UserName);

            controller.Put(1, user);

            var updatedUser = controller.Get(user.UserName);

            Assert.AreEqual(updatedName, updatedUser.Name);
            Assert.AreEqual(updatedEmail, updatedUser.Email);
        }

        [TestMethod]
        public void Delete()
        {
            var context = LoadContext();

            UsersController controller = new UsersController(context);

            controller.Delete(1);

            var deleted = controller.Get("Person1");

            Assert.IsNull(deleted);
        }

        [TestMethod]
        public void IsUserValid()
        {
            var controller = new UsersController(LoadContext());

            var invalidUser = new User();
            var validUser = new User { Email = "test@email.com", Name = "TestName", UserName = "TestUserName" };

            var errors = new List<ValidationResult>();

            var hasErrors = Validator.TryValidateObject(invalidUser, new ValidationContext(invalidUser), errors);
            var noErrors = Validator.TryValidateObject(validUser, new ValidationContext(validUser), errors);

            Assert.IsFalse(hasErrors);
            Assert.IsTrue(noErrors);
        }

        [TestMethod]
        public void IsUserNameUsed()
        {
            var controller = new UsersController(LoadContext());

            Assert.IsFalse(controller.IsUserNameUsed("SomeUnusedUserName"));
            Assert.IsFalse(controller.IsUserNameUsed(null));
            Assert.IsTrue(controller.IsUserNameUsed("Person1"));
        }
    }
}
