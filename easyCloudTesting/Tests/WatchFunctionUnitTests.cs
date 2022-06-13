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
using System.IO;
using Newtonsoft.Json;
using easyCloud.Models.Shared;
using easyCloud;
using easyCloud.Models.Shared.Responses;

namespace extraAhorroTesting {

    public class LoginSystemTest {
        [Fact]
        public async Task Register_With_Valid_Credentials() {
            var mockHttpRequest = new Mock<HttpRequest>();
            const string mockName = "Mock Name";
            const string mockPassword = "password";
            const string mockEmail = "test@easycloud";
            const string mockCompany = "Mock Company";

            var mockAccountCollector = new Mock<IAsyncCollector<AccountTable>>();
            var mockAccountTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable"), It.IsAny<TableClientConfiguration>());
            var loggerMock = new Mock<ILogger>();

            mockHttpRequest.Setup(x => x.Query["email"]).Returns(mockEmail);
            mockHttpRequest.Setup(x => x.Query["name"]).Returns(mockName);
            mockHttpRequest.Setup(x => x.Query["password"]).Returns(mockPassword);
            mockHttpRequest.Setup(x => x.Query["company"]).Returns(mockCompany);

            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            var jsonWriter = new JsonTextWriter(streamWriter);

            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, new {
                email = mockEmail,
                name = mockName,
                password = mockPassword,
                company = mockCompany
            });
            jsonWriter.Flush();
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            mockHttpRequest.Setup(x => x.Body).Returns(stream);

            ConstructorInfo? ctor = typeof(TableQuerySegment<AccountTable>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);

            _ = mockAccountTable
                .Setup(x => x.ExecuteQuerySegmentedAsync(It.IsAny<TableQuery<AccountTable>>(), null))
                .Returns(Task.FromResult(ctor.Invoke(new object[] { new List<AccountTable>() }) as TableQuerySegment<AccountTable>));

            mockAccountCollector
                .Setup(_ => _.AddAsync(It.IsAny<AccountTable>(), It.IsAny<System.Threading.CancellationToken>()))
                .Callback((AccountTable model, CancellationToken token) => { })
                .Returns((AccountTable model, CancellationToken token) => Task.FromResult((AccountTable)null));

            var result = await LoginSystem.Register(
               mockHttpRequest.Object,
               mockAccountCollector.Object,
               mockAccountTable.Object,
               loggerMock.Object);

            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var response = (RequestResponse)okObjectResult.Value;

            Assert.Equal(new RequestResponse("success", new Dictionary<string, dynamic>
                {
                    { "message", "User with email " + mockEmail + " registered" }
                }).ToString(),
                response?.ToString());
        }

        [Fact]
        public async Task Register_With_Invalid_Password() {
            var mockHttpRequest = new Mock<HttpRequest>();
            const string mockName = "Mock Name";
            const string mockPassword = "password";
            const string mockEmail = "test@easycloud";
            const string mockCompany = "Mock Company";

            var mockAccountCollector = new Mock<IAsyncCollector<AccountTable>>();
            var mockAccountTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable"), It.IsAny<TableClientConfiguration>());
            var loggerMock = new Mock<ILogger>();

            mockHttpRequest.Setup(x => x.Query["email"]).Returns(mockEmail);
            mockHttpRequest.Setup(x => x.Query["name"]).Returns(mockName);
            mockHttpRequest.Setup(x => x.Query["password"]).Returns(mockPassword);
            mockHttpRequest.Setup(x => x.Query["company"]).Returns(mockCompany);

            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            var jsonWriter = new JsonTextWriter(streamWriter);

            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, new {
                email = mockEmail,
                name = mockName,
                password = mockPassword,
                company = mockCompany
            });
            jsonWriter.Flush();
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            mockHttpRequest.Setup(x => x.Body).Returns(stream);

            ConstructorInfo? ctor = typeof(TableQuerySegment<AccountTable>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);

            _ = mockAccountTable
                .Setup(x => x.ExecuteQuerySegmentedAsync(It.IsAny<TableQuery<AccountTable>>(), null))
                .Returns(Task.FromResult(ctor.Invoke(new object[] { new List<AccountTable>() }) as TableQuerySegment<AccountTable>));

            mockAccountCollector
                .Setup(_ => _.AddAsync(It.IsAny<AccountTable>(), It.IsAny<System.Threading.CancellationToken>()))
                .Callback((AccountTable model, CancellationToken token) => { })
                .Returns((AccountTable model, CancellationToken token) => Task.FromResult((AccountTable)null));

            var result = await LoginSystem.Register(
               mockHttpRequest.Object,
               mockAccountCollector.Object,
               mockAccountTable.Object,
               loggerMock.Object);

            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var response = (RequestResponse)okObjectResult.Value;

            Assert.Equal(new RequestResponse("success", new Dictionary<string, dynamic>
                {
                    { "message", "User with email " + mockEmail + " registered" }
                }).ToString(),
                response?.ToString());
        }

        [Fact]
        public async Task Register_With_Invalid_Email() {
            var mockHttpRequest = new Mock<HttpRequest>();
            const string mockName = "Mock Name";
            const string mockPassword = "password";
            const string mockEmail = "test@easycloud";
            const string mockCompany = "Mock Company";

            var mockAccountCollector = new Mock<IAsyncCollector<AccountTable>>();
            var mockAccountTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable"), It.IsAny<TableClientConfiguration>());
            var loggerMock = new Mock<ILogger>();

            mockHttpRequest.Setup(x => x.Query["email"]).Returns(mockEmail);
            mockHttpRequest.Setup(x => x.Query["name"]).Returns(mockName);
            mockHttpRequest.Setup(x => x.Query["password"]).Returns(mockPassword);
            mockHttpRequest.Setup(x => x.Query["company"]).Returns(mockCompany);

            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            var jsonWriter = new JsonTextWriter(streamWriter);

            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, new {
                email = mockEmail,
                name = mockName,
                password = mockPassword,
                company = mockCompany
            });
            jsonWriter.Flush();
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            mockHttpRequest.Setup(x => x.Body).Returns(stream);

            ConstructorInfo? ctor = typeof(TableQuerySegment<AccountTable>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);

            _ = mockAccountTable
                .Setup(x => x.ExecuteQuerySegmentedAsync(It.IsAny<TableQuery<AccountTable>>(), null))
                .Returns(Task.FromResult(ctor.Invoke(new object[] { new List<AccountTable>() }) as TableQuerySegment<AccountTable>));

            mockAccountCollector
                .Setup(_ => _.AddAsync(It.IsAny<AccountTable>(), It.IsAny<System.Threading.CancellationToken>()))
                .Callback((AccountTable model, CancellationToken token) => { })
                .Returns((AccountTable model, CancellationToken token) => Task.FromResult((AccountTable)null));

            var result = await LoginSystem.Register(
               mockHttpRequest.Object,
               mockAccountCollector.Object,
               mockAccountTable.Object,
               loggerMock.Object);

            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var response = (RequestResponse)okObjectResult.Value;

            Assert.Equal(new RequestResponse("success", new Dictionary<string, dynamic>
                {
                    { "message", "User with email " + mockEmail + " registered" }
                }).ToString(),
                response?.ToString());
        }

        [Fact]
        public async Task Register_Already_Registered_Email() {
            var mockHttpRequest = new Mock<HttpRequest>();
            const string mockName = "Mock Name";
            const string mockPassword = "password";
            const string mockEmail = "test@easycloud";
            const string mockCompany = "Mock Company";

            var mockAccountCollector = new Mock<IAsyncCollector<AccountTable>>();
            var mockAccountTable = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable"), It.IsAny<TableClientConfiguration>());
            var loggerMock = new Mock<ILogger>();

            mockHttpRequest.Setup(x => x.Query["email"]).Returns(mockEmail);
            mockHttpRequest.Setup(x => x.Query["name"]).Returns(mockName);
            mockHttpRequest.Setup(x => x.Query["password"]).Returns(mockPassword);
            mockHttpRequest.Setup(x => x.Query["company"]).Returns(mockCompany);

            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            var jsonWriter = new JsonTextWriter(streamWriter);

            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, new {
                email = mockEmail,
                name = mockName,
                password = mockPassword,
                company = mockCompany
            });
            jsonWriter.Flush();
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            mockHttpRequest.Setup(x => x.Body).Returns(stream);

            ConstructorInfo? ctor = typeof(TableQuerySegment<AccountTable>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);

            _ = mockAccountTable
                .Setup(x => x.ExecuteQuerySegmentedAsync(It.IsAny<TableQuery<AccountTable>>(), null))
                .Returns(Task.FromResult(ctor.Invoke(new object[] { new List<AccountTable>() }) as TableQuerySegment<AccountTable>));

            mockAccountCollector
                .Setup(_ => _.AddAsync(It.IsAny<AccountTable>(), It.IsAny<System.Threading.CancellationToken>()))
                .Callback((AccountTable model, CancellationToken token) => { })
                .Returns((AccountTable model, CancellationToken token) => Task.FromResult((AccountTable)null));

            var result = await LoginSystem.Register(
               mockHttpRequest.Object,
               mockAccountCollector.Object,
               mockAccountTable.Object,
               loggerMock.Object);

            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var response = (RequestResponse)okObjectResult.Value;

            Assert.Equal(new RequestResponse("success", new Dictionary<string, dynamic>
                {
                    { "message", "User with email " + mockEmail + " registered" }
                }).ToString(),
                response?.ToString());
        }
    }
}