using System;
using System.Security;

using easyCloud.Models;

using Microsoft.Azure.Cosmos.Table;

namespace easyCloud.Models {
    public class AccountTable : TableEntity {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
    }

    public class Account {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
    }

    public class CreateAccountDto {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
    }

    public static class AccountExtensions {

        public static AccountTable ToTable(this Account account) {
            return new AccountTable {
                PartitionKey = "ACCOUNT",
                RowKey = account.Id,
                Name = account.Name,
                Email = account.Email,
                Password = account.Password,
                Company = account.Company
            };
        }

        public static Account ToFakeOauthToken(this AccountTable accountTable) {
            return new Account {
                Id = accountTable.RowKey,
                Name = accountTable.Name,
                Email = accountTable.Email,
                Password = accountTable.Password,
                Company = accountTable.Company
            };
        }
    }
}
