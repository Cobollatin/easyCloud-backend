using System;
using Microsoft.Azure.Cosmos.Table;

namespace easyCloud.Models
{
    public class SessionTable : TableEntity
    {
        public string AccountToken { get; set; }
        public string SessionToken { get; set; }
        public bool IsActive { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class Session
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AccountToken { get; set; }
        public string SessionToken { get; set; } = Guid.NewGuid().ToString();
        public bool IsActive { get; set; } = true;
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now.AddDays(1);

        bool Update()
        {
            if (End < DateTime.Now)
            {
                IsActive = false;
            }
            return IsActive;
        }
    }

    public class CreateSessionDto
    {
        public string OauthToken { get; set; }
        public string SessionToken { get; set; }
        public bool IsActive { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public static class SessionExtensions
    {
        public static SessionTable ToTable(this Session session)
        {
            return new SessionTable
            {
                PartitionKey = "SESSION",
                RowKey = session.Id,
                AccountToken = session.AccountToken,
                SessionToken = session.SessionToken,
                IsActive = session.IsActive,
                Start = session.Start,
                End = session.End
            };
        }

        public static Session ToSession(this SessionTable session)
        {
            return new Session
            {
                Id = session.RowKey,
                AccountToken = session.AccountToken,
                IsActive = session.IsActive,
                Start = session.Start,
                End = session.End
            };
        }
    }
}
