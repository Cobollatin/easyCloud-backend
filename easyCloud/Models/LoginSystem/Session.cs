using System;

using Microsoft.Azure.Cosmos.Table;

namespace easyCloud.Models {
    public class SessionTable : TableEntity {
        public string AccountId { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public class Session {
        public string AccountId { get; set; }
        public string AccessToken { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime ExpireDate { get; set; } = DateTime.Now.AddDays(1);

        private bool Update() {
            if (ExpireDate < DateTime.Now) {
                IsActive = false;
            }

            return IsActive;
        }
    }

    public class CreateSessionDto {
        public string OauthToken { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public static class SessionExtensions {

        public static SessionTable ToTable(this Session session) {
            return new SessionTable {
                PartitionKey = "SESSION",
                RowKey = session.AccessToken,
                AccountId = session.AccountId,
                IsActive = session.IsActive,
                ExpireDate = session.ExpireDate
            };
        }

        public static Session ToSession(this SessionTable session) {
            return new Session {
                AccessToken = session.RowKey,
                AccountId = session.AccountId,
                IsActive = session.IsActive,
                ExpireDate = session.ExpireDate
            };
        }
    }
}
