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
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="accountTableCollector"></param>
        /// <param name="accountTable"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Register")]
        public static async Task<IActionResult> Register(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/register")] HttpRequest req,
            [Table("Users", Connection = "AzureWebJobsStorage")] IAsyncCollector<AccountTable> accountTableCollector,
            [Table("Users", Connection = "AzureWebJobsStorage")] CloudTable accountTable,
            ILogger log) {
            log.LogInformation("Attempting to register an user");

            string username = req.Query["username"];
            string password = req.Query["password"];
            string email = req.Query["email"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            username = username ?? data?.username;
            password = password ?? data?.password;
            email = email ?? data?.email;

            if (username is null || username.Length is > 18 or < 4) {
                log.LogError("Invalid username");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "username must have from 4 to 12 characters" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            if (password is null || password.Length is > 32 or < 8) {
                log.LogError("Invalid password");
                Fail fail = new Fail(new Dictionary<string, string>() {
                    { "error", "password must have from 8 to 32 characters" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            try {
                new MailAddress(email);
            } catch (FormatException) {
                log.LogError("Invalid email");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "invalid email" }
                }
                );
                return new BadRequestObjectResult(fail);
            }


            var query = new TableQuery<AccountTable>().Where(TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));
            var queryResult = await accountTable.ExecuteQuerySegmentedAsync(query, null);


            if (queryResult is null || queryResult.Results.Count != 0) {
                log.LogError("Email already taken");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "email already taken" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            query = new TableQuery<AccountTable>().Where(TableQuery.GenerateFilterCondition("Username", QueryComparisons.Equal, username));
            queryResult = await accountTable.ExecuteQuerySegmentedAsync(query, null);

            if (queryResult is null || queryResult.Results.Count != 0) {
                log.LogError("Username already taken");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "username already taken" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            await accountTableCollector.AddAsync(new Account() {
                Username = username,
                Password = Security.ComputeHash(password, new SHA256CryptoServiceProvider()),
                Email = email,
            }.ToTable());

            log.LogTrace("User registered");

            return new OkObjectResult(new Post<Account>(new List<Account>
            {
                new Account()
                {
                    Username= username,
                    Email= email,
                    Password= "*"
                }
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userTable"></param>
        /// <param name="sessionTable"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Login")]
        public static async Task<IActionResult> Login(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/login")] HttpRequest req,
        [Table("Users", Connection = "AzureWebJobsStorage")] CloudTable userTable,
        [Table("Sessions", Connection = "AzureWebJobsStorage")] IAsyncCollector<SessionTable> sessionTable,
        ILogger log) {

            string username = req.Query["username"];
            string password = req.Query["password"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            username = username ?? data?.username;
            password = password ?? data?.password;

            if (username is null || username.Length > 18 || username.Length < 4) {
                log.LogError("Invalid username");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "username must have from 4 to 12 characters" }
                }
                );
                return new BadRequestObjectResult(fail);
            }
            if (password is null || password.Length > 32 || password.Length < 8) {
                log.LogError("Invalid username");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    {
                        "error", "password must have from 8 to 32 characters"
                    }
                }
                );
                return new BadRequestObjectResult(fail);
            }


            var query = new TableQuery<AccountTable>().Where(TableQuery.GenerateFilterCondition("Username", QueryComparisons.Equal, username));
            var queryResult = await userTable.ExecuteQuerySegmentedAsync(query, null);

            if (queryResult is null || queryResult.Results.Count == 0) {
                log.LogError("User not registered");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "user not registered" },
                    { "links", "/register" }
                }
                );
                return new OkObjectResult(fail);
            }

            var acc = queryResult.Results.Find(x => x.Password == Security.ComputeHash(password, new SHA256CryptoServiceProvider()));

            if (acc is not null) {
                log.LogError("Wrong password");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "wrong password" },
                }
                );
                return new OkObjectResult(fail);
            }

            var seesionQuery = new TableQuery<SessionTable>().Where(TableQuery.GenerateFilterCondition("AccountToken", QueryComparisons.Equal, acc.Token));
            var sessionQueryResult = await userTable.ExecuteQuerySegmentedAsync(seesionQuery, null);

            if (queryResult is null || sessionQueryResult.Results.Count != 0) {
                log.LogInformation("User already logged in");
                var currentSeesion = sessionQueryResult.Results.FindLast(x => x.IsActive == true).ToSession();
                return new OkObjectResult(new Post<Session>(new List<Session> { currentSeesion }));
            }

            log.LogInformation("User found, creating session");

            var session = new Session();
            session.AccountToken = acc.Token;
            await sessionTable.AddAsync(session.ToTable());

            return new OkObjectResult(new Post<Session>(new List<Session> { session }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="sessionTable"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Logout")]
        public static async Task<IActionResult> Logout(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/logout")] HttpRequest req,
            [Table("Sessions", Connection = "AzureWebJobsStorage")] CloudTable sessionTable,
            ILogger log) {
            log.LogInformation("Attempting to logout an user");

            string accountToken = req.Query["AccountToken"];
            string sessionToken = req.Query["SessionToken"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            accountToken = accountToken ?? data?.accountToken;
            sessionToken = sessionToken ?? data?.password;

            if (!Guid.TryParse(accountToken, out _) || !Guid.TryParse(sessionToken, out _)) {
                log.LogInformation("Invalid tokens");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "invalid data" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            var query = new TableQuery<SessionTable>().Where(TableQuery.GenerateFilterCondition("AccountToken", QueryComparisons.Equal, accountToken));
            var queryResult = await sessionTable.ExecuteQuerySegmentedAsync(query, null);

            var lastSession = queryResult.Results.Find(x => x.AccountToken == accountToken && x.IsActive == true);
            if (queryResult is null || queryResult.Results.Count == 0 || lastSession is null) {
                log.LogInformation("User not logged in");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "user not logged in" }
                }
                );
                return new OkObjectResult(fail);
            }

            log.LogInformation("User logged out");

            lastSession.IsActive = false;
            lastSession.End = DateTime.UtcNow;
            await sessionTable.ExecuteAsync(TableOperation.InsertOrReplace(lastSession));

            return new OkObjectResult(new Post<SessionTable>(new List<SessionTable> { lastSession }));
        }

        [FunctionName("Validate")]
        public static async Task<IActionResult> Validate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/validate")]
            HttpRequest req,
            [Table("Sessions", Connection = "AzureWebJobsStorage")]
            CloudTable sessionTable,
            ILogger log) {
            log.LogInformation("Attempting to validate a session");

            string accountToken = req.Query["AccountToken"];
            string sessionToken = req.Query["SessionToken"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            accountToken = accountToken ?? data?.accountToken;
            sessionToken = sessionToken ?? data?.password;

            if (!Guid.TryParse(accountToken, out _) || !Guid.TryParse(sessionToken, out _)) {
                log.LogInformation("Invalid tokens");
                Fail fail = new Fail(new Dictionary<string, string>()
                    {
                        { "error", "invalid data" }
                    }
                );
                return new BadRequestObjectResult(fail);
            }

            var query = new TableQuery<SessionTable>().Where(TableQuery.GenerateFilterCondition("AccountToken", QueryComparisons.Equal, accountToken));
            var queryResult = await sessionTable.ExecuteQuerySegmentedAsync(query, null);

            var lastSession = queryResult.Results.Find(x => x.AccountToken == accountToken && x.IsActive == true);
            if (queryResult is null || queryResult.Results.Count == 0 || lastSession is null) {
                log.LogInformation("User not logged in");
                Fail fail = new Fail(new Dictionary<string, string>()
                    {
                        { "error", "user not logged in" }
                    }
                );
                return new OkObjectResult(fail);
            }

            log.LogInformation("Valid session out");

            return new OkObjectResult(new Get<string>(new List<string> { "valid" }));
        }
    }
}
