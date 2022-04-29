using System;
using Microsoft.Azure.Cosmos.Table;

namespace extraAhorro.Models
{
    public class AccountTokenTable : TableEntity
    {
        public string OauthToken { get; set; }
        public string Email { get; set; }
    }

    public class AccountToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string OauthToken { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = "";

    }

    public class CreateAccountTokenDto
    {
        public string OauthToken { get; set; }
        public string Email { get; set; }
    }

    public static class AccountTokenExtensions
    {
        public static AccountTokenTable ToTable(this AccountToken fakeOauthToken)
        {
            return new AccountTokenTable
            {
                PartitionKey = "TOKEN",
                RowKey = fakeOauthToken.Id,
                OauthToken = fakeOauthToken.OauthToken,
                Email = fakeOauthToken.Email,
            };
        }

        public static AccountToken ToFakeOauthToken(this AccountTokenTable fakeOauthTokenTable)
        {
            return new AccountToken
            {
                Id = fakeOauthTokenTable.RowKey,
                OauthToken = fakeOauthTokenTable.OauthToken,
                Email = fakeOauthTokenTable.Email
            };
        }
    }
}
