using System;
using Microsoft.Azure.Cosmos.Table;

namespace easyCloud.Models
{
    public class UserTable : TableEntity
    {
        public string OauthToken { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string OauthToken { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class CreateUserDto
    {
        public string OauthToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public static class UserExtensions
    {
        public static UserTable ToTable(this User user)
        {
            return new UserTable
            {
                PartitionKey = "USER",
                RowKey = user.Id,
                OauthToken = user.OauthToken,
                Name = user.Name,
                Email = user.Email
            };
        }

        public static User ToUser(this UserTable userTable)
        {
            return new User
            {
                Id = userTable.RowKey,
                OauthToken = userTable.OauthToken,
                Name = userTable.Name,
                Email = userTable.Email
            };
        }
    }
}
