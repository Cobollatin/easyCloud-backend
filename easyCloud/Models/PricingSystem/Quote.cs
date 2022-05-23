using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.OData.Edm;

namespace easyCloud.Models.PricingSystem {
    public class QuoteTable : TableEntity {
        public string Title { get; set; }
        public string Description { get; set; }
        public Date Date { get; set; }
        public string CloudService { get; set; }
        public string Price { get; set; }
        public bool IsScale { get; set; }
    }

    public class Quote {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Date Date { get; set; }
        public string CloudService { get; set; }
        public string Price { get; set; }
        public bool IsScale { get; set; }
    }

    public class CreateQuoteDto {
        public string Title { get; set; }
        public string Description { get; set; }
        public Date Date { get; set; }
        public string CloudService { get; set; }
        public string Price { get; set; }
        public bool IsScale { get; set; }
    }

    public static class QuoteExtensions {
        public static QuoteTable ToQuoteTable(Quote quote) {
            return new QuoteTable() {
                PartitionKey = quote.Id,
                Title = quote.Title,
                Description = quote.Description,
                Date = quote.Date,
                CloudService = quote.CloudService,
                Price = quote.Price,
                IsScale = quote.IsScale
            };
        }

        public static Quote ToQuote(QuoteTable quoteTable) {
            return new Quote() {
                Id = quoteTable.PartitionKey,
                Title = quoteTable.Title,
                Description = quoteTable.Description,
                Date = quoteTable.Date,
                CloudService = quoteTable.CloudService,
                Price = quoteTable.Price,
                IsScale = quoteTable.IsScale
            };
        }

    }
}

