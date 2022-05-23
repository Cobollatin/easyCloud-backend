using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

namespace easyCloud.Models.PricingSystem {
    public class ProviderTable : TableEntity {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EndPoing { get; set; }
        public string Scheme { get; set; }
    }
    public class Provider {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EndPoing { get; set; }
        public string Scheme { get; set; }
    }

    public class CreateProviderDto {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EndPoing { get; set; }
        public string Scheme { get; set; }
    }

    public static class ProviderExtensions {
        public static Provider ToProvider(this ProviderTable table) {
            return new Provider {
                Id = table.Id,
                Name = table.Name,
                Description = table.Description,
                EndPoing = table.EndPoing,
                Scheme = table.Scheme
            };
        }

        public static ProviderTable ToProviderTable(this Provider provider) {
            return new ProviderTable {
                Id = provider.Id,
                Name = provider.Name,
                Description = provider.Description,
                EndPoing = provider.EndPoing,
                Scheme = provider.Scheme
            };
        }
    }
}
