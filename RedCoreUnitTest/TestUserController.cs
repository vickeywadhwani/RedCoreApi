using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Results;
using System.Net;
using RedCoreApi.Models;
using RedCoreApi.Controllers;
using System.Web.Http.OData;

namespace RedCoreUnitTest
{
    [TestClass]
    public class TestUserController
    {
        [TestMethod]
        public void GetUsers_ShouldReturnAllUsers()
        {
            var context = new TestRedCoreContext();
            context.user.Add(new user { userid = 1, name = "Demo1", email = "demo1@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm"});
            context.user.Add(new user { userid = 2, name = "Demo2", email = "demo2@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" });
            context.user.Add(new user { userid = 3, name = "Demo3", email = "demo3@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" });

            var controller = new UsersController(context);
            var result = controller.Getuser() as TestUserDbSet;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Local.Count);
        }

        [TestMethod]
        public void GetUser_ShouldReturnUserWithSameID()
        {
            var context = new TestRedCoreContext();
            context.user.Add(GetDemoUser());

            var controller = new UsersController(context);
            var result = controller.Getuser(1) as OkNegotiatedContentResult<user>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.userid);
        }

        [TestMethod]
        public void PostUser_ShouldValidateEmailBeforeAdding()
        {
            var controller = new UsersController(new TestRedCoreContext());

            var item = GetFailUser();

            var result =
                controller.Postuser(item) as CreatedAtRouteNegotiatedContentResult<user>;

            
            Assert.AreEqual(result.Content.name, item.name);
        }

        [TestMethod]
        public void PutUser_ShouldFail_WhenDifferentID()
        {
            var controller = new UsersController(new TestRedCoreContext());

            var badresult = controller.Putuser(97, GetDemoUser());
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostUser_ShouldHaveUniqueEmail()
        {
            var context = new TestRedCoreContext();
            
            context.user.Add(new user { userid = 2, name = "Demo2", email = "demo1@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" });
            var controller = new UsersController(context);
            var item = GetUserWithSameEmail();
            dynamic result = controller.Postuser(item);
            dynamic content = result.Content;
            dynamic statusCode = result.StatusCode;

            Assert.AreEqual(HttpStatusCode.Conflict, statusCode);
            Assert.AreEqual(content.message, "An existing record with the email was already found.");

        }

        [TestMethod]
        public void PutUser_ShouldHaveUniqueEmail()
        {
            var context = new TestRedCoreContext();

            context.user.Add(new user { userid = 1, name = "Demo1", email = "demo1@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" });
            context.user.Add(new user { userid = 2, name = "Demo2", email = "demo2@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" });
            var controller = new UsersController(context);
            var item = GetDemoUser();
            dynamic result = controller.Putuser(1, item);
            dynamic content = result.Content;
            dynamic statusCode = result.StatusCode;

            Assert.AreEqual(HttpStatusCode.Conflict, statusCode);
            Assert.AreEqual(content.message, "An existing record with the email was already found.");
        }

        [TestMethod]
        public void PutUser_ShouldReturnStatusCode()
        {
            var controller = new UsersController(new TestRedCoreContext());

            var item = GetDemoUser();

            var result = controller.Putuser(item.userid, item) as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public void PatchUser_ShouldReturnStatusCode()
        {
            var context = new TestRedCoreContext();

            context.user.Add(new user { userid = 1, name = "Demo1", email = "demo1@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" });
            var controller = new UsersController(context);


            var delta = new Delta<user>(typeof(user));
            delta.TrySetPropertyValue("name", "Demo2");
            delta.TrySetPropertyValue("userid", 1);

            var result = controller.Patchuser(1, delta) as StatusCodeResult;
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public void PatchUser_ShouldHaveUniqueEmail()
        {
            var context = new TestRedCoreContext();

            context.user.Add(new user { userid = 1, name = "Demo1", email = "demo1@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" });
            context.user.Add(new user { userid = 2, name = "Demo2", email = "demo2@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" });
            var controller = new UsersController(context);

            var delta = new Delta<user>(typeof(user));
            delta.TrySetPropertyValue("name", "Demo1");
            delta.TrySetPropertyValue("email", "demo2@yopmail.com");
            delta.TrySetPropertyValue("userid", 1);

            dynamic result = controller.Patchuser(1, delta);
            dynamic content = result.Content;
            dynamic statusCode = result.StatusCode;

            Assert.AreEqual(HttpStatusCode.Conflict, statusCode);
            Assert.AreEqual(content.message, "An existing record with the email was already found.");
        }

        [TestMethod]
        public void DeleteUser_ShouldReturnOK()
        {
            var context = new TestRedCoreContext();
            var item = GetDemoUser();
            context.user.Add(item);
            var controller = new UsersController(context);
            var result = controller.Deleteuser(1) as OkNegotiatedContentResult<user>;

            Assert.IsNotNull(result);
            Assert.AreEqual(item.userid, result.Content.userid);
        }

        user GetDemoUser()
        {
            return new user() { userid = 1, name = "Demo1", email = "demo2@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" };
        }

        user GetUserWithSameEmail()
        {
            return new user() { userid = 1, name = "Demo1", email = "demo1@yopmail.com", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" };
        }

        user GetFailUser()
        {
            return new user() { userid = 1, name = "Demo1", email = "demo1@yopmail", telephone = "0761670000", address = "testgatan 123", post_code = "11111", city = "Stockholm" };
        }

    }


}
