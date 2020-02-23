﻿using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Wordki.Tests.EndToEnd.Configuration;

namespace Wordki.Tests.EndToEnd.Login
{
    [TestFixture]
    public class Register : TestBase
    {

        public Register()
        {
            Request = new HttpRequestMessage(HttpMethod.Post, "/register");
            Host = new TestServerMock();
        }

        void GivenRequest()
        {
            Request.Content = new StringContent("{\"userName\":\"user\", \"password\":\"pass\", \"passwordRepeat\":\"pass\"}", Encoding.UTF8, "application/json");
        }

        async Task WhenRequestReceived() => await SendRequest();

        void ThenResponseIsOk()
        {
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
        }

        async Task AndThenResponseIsEmpty()
        {
            var responseContent = await Response.Content.ReadAsStringAsync();
            Assert.AreEqual(string.Empty, responseContent);
        }

        async Task AndThenUserAdded()
        {
            using (var dbContext = new EntityFramework())
            {
                var user = await dbContext.Users.SingleAsync();
                Assert.AreEqual("user", user.Name);
                Assert.AreEqual(1, user.Id);
                Assert.AreEqual(Host.EncrypterMock.Object.Md5Hash("pass"), user.Password);
                Assert.IsNull(user.LastLoginDate);
                Assert.AreEqual(Host.TimeProviderMock.Object.GetTime(), user.CreationDate);
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }

    [TestFixture]
    public class Register_Fail : TestBase
    {

        public Register_Fail()
        {
            Request = new HttpRequestMessage(HttpMethod.Post, "/register");
            Host = new TestServerMock();
        }

        void GivenRequest()
        {
            Request.Content = new StringContent("{\"userName\":\"\", \"password\":\"pass\", \"passwordRepeat\":\"pass\"}", Encoding.UTF8, "application/json");
        }

        async Task WhenRequestReceived() => await SendRequest();

        void ThenResponseIsInternalServerError()
        {
            Assert.AreEqual(HttpStatusCode.InternalServerError, Response.StatusCode);
        }

        async Task AndThenResponseContainError()
        {
            var responseContent = await Response.Content.ReadAsStringAsync();
            Assert.IsNotEmpty(responseContent);
        }

        async Task AndThenDatabaseIsEmpty(){
            using(var dbContext = new EntityFramework()){
                var user = await dbContext.Users.SingleOrDefaultAsync();
                Assert.IsNull(user);
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }

    [TestFixture]
    public class Register_Fail2 : TestBase
    {

        public Register_Fail2()
        {
            Request = new HttpRequestMessage(HttpMethod.Post, "/register");
            Host = new TestServerMock();
        }

        void GivenRequest()
        {
            Request.Content = new StringContent("{\"userName\":\"user\", \"password\":\"pass\", \"passwordRepeat\":\"pass1\"}", Encoding.UTF8, "application/json");
        }

        async Task WhenRequestReceived() => await SendRequest();

        void ThenResponseIsInternalServerError()
        {
            Assert.AreEqual(HttpStatusCode.InternalServerError, Response.StatusCode);
        }

        async Task AndThenResponseContainError()
        {
            var responseContent = await Response.Content.ReadAsStringAsync();
            Assert.IsNotEmpty(responseContent);
        }

        async Task AndThenDatabaseIsEmpty(){
            using(var dbContext = new EntityFramework()){
                var user = await dbContext.Users.SingleOrDefaultAsync();
                Assert.IsNull(user);
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}
