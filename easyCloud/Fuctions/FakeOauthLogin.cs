using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using extraAhorro.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using extraAhorro.Models.Responses;
using System.Collections.Generic;
using System.Net.Mail;

namespace extraAhorro
{
    public static class FakeOauthLogin
    {
        [FunctionName("Register")]
        public static async Task<IActionResult> Register(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "register/{OauthToken}/{isSeller?}")] HttpRequest req, string OauthToken, string? isSeller,
            [Table("Users", Connection = "AzureWebJobsStorage")] IAsyncCollector<UserTable> userTableCollector,
            [Table("Users", Connection = "AzureWebJobsStorage")] CloudTable userTable,
            [Table("OauthTokens", Connection = "AzureWebJobsStorage")] CloudTable fakeOauthTokenTable,
            ILogger log)
        {
            if (req is null)
            {
                throw new ArgumentNullException(nameof(req));
            }

            if (string.IsNullOrEmpty(OauthToken))
            {
                throw new ArgumentException($"'{nameof(OauthToken)}' cannot be null or empty.", nameof(OauthToken));
            }

            if (userTableCollector is null)
            {
                throw new ArgumentNullException(nameof(userTableCollector));
            }

            if (userTable is null)
            {
                throw new ArgumentNullException(nameof(userTable));
            }

            if (fakeOauthTokenTable is null)
            {
                throw new ArgumentNullException(nameof(fakeOauthTokenTable));
            }

            if (log is null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            log.LogInformation("Attempting to register an user");

            


            if (!Guid.TryParse(OauthToken, out _))
            {
                log.LogInformation("Invalid token");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "invalid token" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            if (isSeller != null  && isSeller != "seller" && isSeller != "")
            {
                log.LogInformation("Invalid user type");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "invalid user type" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            var queryUser = new TableQuery<UserTable>().Where(TableQuery.GenerateFilterCondition("OauthToken", QueryComparisons.Equal, OauthToken));

            var queryUserResult = await userTable.ExecuteQuerySegmentedAsync(queryUser, null);


            if (queryUserResult.Results.Count != 0)
            {
                log.LogInformation("User already registered");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "user already registered" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            var queryFakeOauthToken = new TableQuery<AccountTokenTable>().Where(TableQuery.GenerateFilterCondition("OauthToken", QueryComparisons.Equal, OauthToken));

            var queryFakeOauthTokeResult = await fakeOauthTokenTable.ExecuteQuerySegmentedAsync(queryFakeOauthToken, null);

            if (queryFakeOauthTokeResult.Results.Count == 0)
            {
                log.LogInformation("Invalid token");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "invalid token" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            var fakeOauthToken = queryFakeOauthTokeResult.Results.Find(x => x.OauthToken == OauthToken);

            var user = new User
            {
                OauthToken = OauthToken,
                Name = fakeOauthToken.Email.Split('@')[0],
                Email = fakeOauthToken.Email,
                IsSeller = isSeller != null ? true : false && isSeller != "",
            };

            await userTableCollector.AddAsync(user.ToTable());

            log.LogInformation("User registered");

            return new OkObjectResult(new Post<User>(new List<User> { user }));
        }

        [FunctionName("Login")]
        public static async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "login/{OauthToken}")] HttpRequest req, string OauthToken,
            [Table("Users", Connection = "AzureWebJobsStorage")] CloudTable userTable,
            [Table("Sessions", Connection = "AzureWebJobsStorage")] IAsyncCollector<SessionTable> sessionTable,
            ILogger log)
        {
            log.LogInformation("Attempting to login an user");

            if (!Guid.TryParse(OauthToken, out _))
            {
                log.LogInformation("Invalid token");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "invalid token" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            TableQuery<UserTable> query = new TableQuery<UserTable>().Where(TableQuery.GenerateFilterCondition("OauthToken", QueryComparisons.Equal, OauthToken));
            var queryResult = await userTable.ExecuteQuerySegmentedAsync(query, null);

            if (queryResult.Results.Count == 0)
            {
                log.LogInformation("User not registered");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "user not registered" }
                }
                );
                return new OkObjectResult(fail);
            }

            log.LogInformation("User found, creating session");

            var session = new Session();
            session.OauthToken = OauthToken;
            

            await sessionTable.AddAsync(session.ToTable());


            return new OkObjectResult(new Get<User>(new List<User> ( queryResult.Results.ConvertAll(x => x.ToUser() ) )));
        }

        [FunctionName("Logout")]
        public static async Task<IActionResult> Logout(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "logout/{OauthToken}")] HttpRequest req, string OauthToken,
            [Table("Sessions", Connection = "AzureWebJobsStorage")] CloudTable sessionTable,
            ILogger log)
        {
            log.LogInformation("Attempting to logout an user");

            if (!Guid.TryParse(OauthToken, out _))
            {
                log.LogInformation("Invalid token");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "invalid token" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            TableQuery<SessionTable> query = new TableQuery<SessionTable>().Where(TableQuery.GenerateFilterCondition("OauthToken", QueryComparisons.Equal, OauthToken));
            var queryResult = await sessionTable.ExecuteQuerySegmentedAsync(query, null);

            var lastSession = queryResult.Results.FindLast(x => x.OauthToken == OauthToken);
            if (!lastSession.IsActive)
            {
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

            await sessionTable.ExecuteAsync(TableOperation.InsertOrReplace(lastSession));

            return new OkObjectResult(new Post<SessionTable>(new List<SessionTable> { lastSession }));
        }


        [FunctionName("GenerateFakeOauthToken")]
        public static async Task<IActionResult> GenerateFakeOauthToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "generateOauthToken/{email}")] HttpRequest req, string email,
            [Table("OauthTokens", Connection = "AzureWebJobsStorage")] IAsyncCollector<AccountTokenTable> fakeOauthTokenCollector,
            [Table("OauthTokens", Connection = "AzureWebJobsStorage")] CloudTable fakeOauthTokenTable,
            ILogger log)
        {
            log.LogInformation("Attempting to generate a fake Oauth Token");

            try
            {
                new MailAddress(email);
            }
            catch (FormatException)
            {
                log.LogInformation("Invalid email");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "invalid email" }
                }
                );
                return new BadRequestObjectResult(fail);
            }

            TableQuery<AccountTokenTable> query = new TableQuery<AccountTokenTable>().Where(TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));

            var queryResult = await fakeOauthTokenTable.ExecuteQuerySegmentedAsync(query, null);

            if (queryResult.Results.Count != 0)
            {
                log.LogInformation("Fake Oauth Token already generated for the email provided");
                Fail fail = new Fail(new Dictionary<string, string>()
                {
                    { "error", "fake Oauth Token already generated, try another email or recover your token" }
                }
                );
                return new BadRequestObjectResult(fail);
            }
            
            AccountToken newToken = new AccountToken();

            query = new TableQuery<AccountTokenTable>().Where(TableQuery.GenerateFilterCondition("OauthToken", QueryComparisons.Equal, newToken.OauthToken));

            queryResult = await fakeOauthTokenTable.ExecuteQuerySegmentedAsync(query, null);

            while (queryResult.Results.Count != 0)
            {
                newToken.OauthToken = Guid.NewGuid().ToString();
                query = new TableQuery<AccountTokenTable>().Where(TableQuery.GenerateFilterCondition("OauthToken", QueryComparisons.Equal, newToken.OauthToken));
                queryResult = await fakeOauthTokenTable.ExecuteQuerySegmentedAsync(query, null);
            }

            newToken.Email = email;

            await fakeOauthTokenCollector.AddAsync(newToken.ToTable());

            log.LogInformation("Fake Oauth Token generated");
            return new OkObjectResult(new Get<AccountToken>(new List<AccountToken> { newToken }));
        }
    }
}
