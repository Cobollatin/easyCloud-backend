using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using Microsoft.Azure.Cosmos.Table;
using easyCloud.Models;
using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace extraAhorroTesting
{

    public class LoginSystemTest
    {
        [Fact]
        public async Task Register_With_Valid_Credentials()
        {
            var mockHttpRequest = new Mock<HttpRequest>();
            const string mockUsername = "username";
            const string mockPassword = "password";
            const string mockEmail = "test@extraahorro";
            var mockAccountCollector = new Mock<IAsyncCollector<AccountTable>>();
            var mockAccountTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable"), It.IsAny<TableClientConfiguration>());
            var loggerMock = new Mock<ILogger>();



            mockHttpRequest.Setup(x => x.Query["email"]).Returns(mockEmail);
            mockHttpRequest.Setup(x => x.Query["username"]).Returns(mockUsername);
            mockHttpRequest.Setup(x => x.Query["password"]).Returns(mockPassword);

            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            var jsonWriter = new JsonTextWriter(streamWriter);

            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, new
            {
                email = mockEmail,
                username = mockUsername,
                password = mockPassword,
            });
            jsonWriter.Flush();
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);


            mockHttpRequest.Setup(x => x.Body).Returns(stream);


            ConstructorInfo? ctor = typeof(TableQuerySegment<AccountTable>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);

            mockAccountTable
                .Setup(x => x.ExecuteQuerySegmentedAsync(It.IsAny<TableQuery<AccountTable>>(), null))
                .Returns(Task.FromResult(ctor.Invoke(new object[] { new List<AccountTable>() }) as TableQuerySegment<AccountTable>));


            mockAccountCollector
                .Setup(_ => _.AddAsync(It.IsAny<AccountTable>(), It.IsAny<System.Threading.CancellationToken>()))
                .Callback((AccountTable model, CancellationToken token) => { })
                .Returns((AccountTable model, CancellationToken token) => Task.FromResult((AccountTable)null));


            var result = await easyCloud.LoginSystem.Register(
               mockHttpRequest.Object,
               mockAccountCollector.Object,
               mockAccountTable.Object,
               loggerMock.Object);

            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var response = okObjectResult.Value as easyCloud.Models.Responses.Post<Account> ?? new easyCloud.Models.Responses.Post<Account>(new List<Account>());

            Assert.Equal("success", response.status);
            Assert.Equal("test@extraahorro", response.data[0].Email);
            Assert.Equal("username", response.data[0].Username);
            Assert.Equal("*", response.data[0].Password);
        }
        [Fact]
        public void Register_With_Invalid_Credentials()
        {

        }
        [Fact]
        public void Register_Already_Registered_Credentials()
        {

        }
        [Fact]
        public void Login_With_Valid_Credentials()
        {

        }
        [Fact]
        public void Login_With_Invalid_Credentials()
        {

        }
        [Fact]
        public void Login_With_Unregistered_Credentials()
        {

        }
        [Fact]
        public void Login_With_Active_Session()
        {

        }
        [Fact]
        public void Logout_With_Valid_Credentials()
        {

        }
        [Fact]
        public void Logout_With_Invalid_Credentials()
        {

        }
        [Fact]
        public void Logout_With_No_Active_Session()
        {

        }
    }
}
