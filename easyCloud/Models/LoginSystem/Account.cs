using System;
using System.Security;

using easyCloud.Models;

using Microsoft.Azure.Cosmos.Table;

namespace easyCloud.Models {
    public class AccountTable : TableEntity {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class Account {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }


    public class CreateAccountDto {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public static class AccountExtensions {
        public static AccountTable ToTable(this Account accountToken) {
            return new AccountTable {
                PartitionKey = "TOKEN",
                RowKey = accountToken.Id,
                Token = accountToken.Token,
                Username = accountToken.Username,
                Email = accountToken.Email,
                Password = accountToken.Password
            };
        }

        public static Account ToFakeOauthToken(this AccountTable accountTokenTable) {
            return new Account {
                Id = accountTokenTable.RowKey,
                Token = accountTokenTable.Token,
                Username = accountTokenTable.Username,
                Email = accountTokenTable.Email,
                Password = accountTokenTable.Password
            };
        }
    }

}
