﻿using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Wordki.Tests.EndToEnd.Configuration;

namespace Wordki.Tests.EndToEnd.Register
{
    [TestFixture]
    public class RequestHaveToUserNameParameter : TestBase
    {

        public RequestHaveToUserNameParameter()
        {
            Request = new HttpRequestMessage(HttpMethod.Post, "/register");
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
            using(var dbContext = new EntityFramework(DapperSettings)){
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
