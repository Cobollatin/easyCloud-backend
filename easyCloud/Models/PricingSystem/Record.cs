using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

namespace easyCloud.Models.PricingSystem {
    public class RecordTable : TableEntity {
        public string UserID { get; set; }
        public string ProviderID { get; set; }
        public string QuoteID { get; set; }

    }

    public class Record {
        public string Id { get; set; }
        public string UserID { get; set; }
        public string ProviderID { get; set; }
        public string QuoteID { get; set; }
    }

    public class CreateRecordDto {
        public string UserID { get; set; }
        public string ProviderID { get; set; }
        public string QuoteID { get; set; }
    }

    public static class RecordExtensions {
        public static Record ToRecord(RecordTable recordTable) {
            return new Record {
                Id = recordTable.PartitionKey,
                UserID = recordTable.UserID,
                ProviderID = recordTable.ProviderID,
                QuoteID = recordTable.QuoteID,
            };
        }

        public static RecordTable ToTable(Record record) {
            return new RecordTable {
                PartitionKey = record.Id,
                UserID = record.UserID,
                ProviderID = record.ProviderID,
                QuoteID = record.QuoteID,
            };
        }
    }
}
