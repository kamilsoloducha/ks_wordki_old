﻿using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Wordki.Core.Dtos;
using Wordki.Tests.EndToEnd.Configuration;

namespace Wordki.Tests.EndToEnd.GetNextWords
{
    [TestFixture]
    public class UserGetNextWords : TestBase
    {
        public UserGetNextWords()
        {
            Request = new HttpRequestMessage(HttpMethod.Get, "getNextWords/2/0");
        }

        async Task GivenDatabaseContainData()
        {
            using (var dbContext = new EntityFramework(DapperSettings))
            {
                var user = new UserDto
                {
                    Id = 1,
                    Name = "user",
                    Password = Host.EncrypterMock.Object.GetSalt(string.Empty),
                    CreationDate = Host.TimeProviderMock.Object.GetTime(),
                    LastLoginDate = Host.TimeProviderMock.Object.GetTime()
                };
                dbContext.Users.Add(user);

                var group = new GroupDto
                {
                    UserId = 1,
                    GroupId = 1,
                    GroupLanguage1 = 1,
                    GroupLanguage2 = 2,
                    Name = "group",
                    GroupCreationDate = Host.TimeProviderMock.Object.GetTime()
                };
                dbContext.Groups.Add(group);

                for (int i = 1; i < 4; i++)
                {
                    var word = new WordDto
                    {
                        WordId = i,
                        GroupId = 1,
                        WordLanguage1 = $"word{i}",
                        WordLanguage2 = $"word{i}",
                        Drawer = 2,
                        IsVisible = true,
                        WordCreationDate = Host.TimeProviderMock.Object.GetTime(),
                        NextRepeat = new DateTime(2020, 01, 01).AddDays(i),
                    };
                    dbContext.Words.Add(word);
                }

                await dbContext.SaveChangesAsync();
            }
        }

        async Task WhenRequestReceived() => await SendRequest();

        void ThenResponseIsOk()
        {
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
        }

        async Task AndThenResponseContainProperMessage()
        {
            var message = await Response.Content.ReadAsStringAsync();
            object temp = null;
            var expectedJsonObj = new object[]
            {
                new
                {
                    groupName = "group",
                    groupLanguage1 = 1,
                    groupLanguage2 = 2,
                    id = 1,
                    language1 = "word1",
                    language2 = "word1",
                    example1 = temp,
                    example2 = temp,
                    drawer = 2,
                    creationDate = Host.TimeProviderMock.Object.GetTime(),
                    repeatsCount = 0,
                    lastRepeat = temp
                },
                new
                {
                    groupName = "group",
                    groupLanguage1 = 1,
                    groupLanguage2 = 2,
                    id = 2,
                    language1 = "word2",
                    language2 = "word2",
                    example1 = temp,
                    example2 = temp,
                    drawer = 2,
                    creationDate = Host.TimeProviderMock.Object.GetTime(),
                    repeatsCount = 0,
                    lastRepeat = temp
                }
            };
            var expectedJson = JsonSerializer.Serialize(expectedJsonObj);
            Assert.AreEqual(expectedJson, message);

        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}
