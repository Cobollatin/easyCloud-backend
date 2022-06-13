using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using easyCloud.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.IO;
using easyCloud.Models.Shared;

namespace easyCloud {
    /// <summary>
    /// 
    /// </summary>
    public static class LoginSystem {
        /// <summary>
        /// </summary>
        /// <param name="req"></param>
        /// <param name="accountTableCollector"></param>
        /// <param name="accountTable"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Register")]
        public static async Task<IActionResult> Register(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "register")]
            HttpRequest req,
            [Table("Users", Connection = "AzureWebJobsStorage")]
            IAsyncCollector<AccountTable> accountTableCollector,
            [Table("Users", Connection = "AzureWebJobsStorage")]
            CloudTable accountTable,
            ILogger log) {
            log.LogInformation("Attempting to register an user");

            string password = req.Query["password"];
            string email = req.Query["email"];
            string name = req.Query["name"];
            string company = req.Query["company"];

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            password = password ?? data?.password;
            email = email ?? data?.email;
            name = name ?? data?.name;
            company = company ?? data?.company;

            if (password is null || password.Length is > 32 or < 8) {
                log.LogError("Invalid password");
                var fail = new RequestResponse("error", new Dictionary<string, dynamic>
                    {
                        { "message", "password must have from 8 to 32 characters" }
                    }
                );
                return new OkObjectResult(fail);
            }

            try {
                if (email != null) {
                    _ = new MailAddress(email);
                }
            } catch (FormatException) {
                log.LogError("Invalid email");
                var fail = new RequestResponse("error", new Dictionary<string, dynamic>
                    {
                        { "message", "invalid email" }
                    }
                );
                return new OkObjectResult(fail);
            }

            var query = new TableQuery<AccountTable>().Where(
                TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));
            var queryResult = await accountTable.ExecuteQuerySegmentedAsync(query, null);

            if (queryResult is null || queryResult.Results.Count != 0) {
                log.LogError("Email already taken");
                var fail = new RequestResponse("error", new Dictionary<string, dynamic>
                    {
                        { "message", "email already taken" }
                    }
                );
                return new OkObjectResult(fail);
            }

            await accountTableCollector.AddAsync(new Account {
                Password = Security.ComputeHash(email + password, SHA512.Create()),
                Email = email,
                Name = name,
                Company = company
            }.ToTable());

            log.LogTrace("User registered");

            return new OkObjectResult(new RequestResponse("success", new Dictionary<string, dynamic>
            {
                { "message", "User with email " + email + " registered" }
            }));
        }

        /// <summary>
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userTable"></param>
        /// <param name="sessionTable"></param>
        /// <param name="sessionTableCollector"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Login")]
        public static async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")]
            HttpRequest req,
            [Table("Users", Connection = "AzureWebJobsStorage")]
            CloudTable userTable,
            [Table("Sessions", Connection = "AzureWebJobsStorage")]
            CloudTable sessionTable,
            [Table("Sessions", Connection = "AzureWebJobsStorage")]
            IAsyncCollector<SessionTable> sessionTableCollector,
            ILogger log) {
            string email = req.Query["email"];
            string password = req.Query["password"];
            string accessToken = req.Query["accessToken"];

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            email = email ?? data?.email;
            password = password ?? data?.password;
            accessToken = accessToken ?? data?.accessToken;

            try {
                if (email != null) {
                    _ = new MailAddress(email);
                }
            } catch (FormatException) {
                log.LogError("Invalid email");
                var fail = new RequestResponse("error", new Dictionary<string, dynamic>
                    {
                        { "message", "invalid email" }
                    }
                );
                return new OkObjectResult(fail);
            }

            if (password is null || password.Length > 32 || password.Length < 1) {
                log.LogError("Invalid username");
                var fail = new RequestResponse("error", new Dictionary<string, dynamic>
                    {
                        { "message", "invalid password" }
                    }
                );
                return new OkObjectResult(fail);
            }

            if (accessToken is null || accessToken.Length < 1) {
                log.LogError("Invalid username");
                var fail = new RequestResponse("error", new Dictionary<string, dynamic>
                    {
                        { "message", "invalid token" }
                    }
                );
                return new OkObjectResult(fail);
            }

            var query = new TableQuery<AccountTable>().Where(
                TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));
            var queryResult = await userTable.ExecuteQuerySegmentedAsync(query, null);

            if (queryResult is null || queryResult.Results.Count == 0) {
                log.LogError("Email not registered");
                var fail = new RequestResponse("error", new Dictionary<string, dynamic>
                    {
                        { "message", "email not registered" }
                    }
                );
                return new OkObjectResult(fail);
            }

            var acc = queryResult.Results.Find(x =>
                x.Password == Security.ComputeHash(email + password, SHA512.Create()));

            if (acc is null) {
                log.LogError("Wrong password");
                var fail = new RequestResponse("error", new Dictionary<string, dynamic>
                    {
                        { "message", "wrong password" }
                    }
                );
                return new OkObjectResult(fail);
            }

            var sessionQuery = new TableQuery<SessionTable>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, accessToken),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("AccountId", QueryComparisons.Equal, acc.RowKey)
            ));
            var sessionQueryResult = await sessionTable.ExecuteQuerySegmentedAsync(sessionQuery, null);

            if (sessionQueryResult is null || sessionQueryResult.Results.Count == 0) {
                await sessionTableCollector.AddAsync(new Session {
                    AccessToken = accessToken,
                    AccountId = acc.RowKey,
                    IsActive = true,
                    ExpireDate = DateTime.Now.AddDays(1)
                }.ToTable());
            }

            return new OkObjectResult(new RequestResponse("success", new Dictionary<string, dynamic>
            {
                { "message", "User logged in with provider token" },
                {
                    "user", new
                    {
                        id = acc.RowKey,
                        name = acc.Name,
                        email = acc.Email,
                        company = acc.Company
                    }
                }
            }));
        }
    }
}
