using System.Reflection;

using AzureFunctions.Extensions.Swashbuckle;

using easyCloud.Models.Documentation;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(SwashbuckleStartup))]
namespace easyCloud.Models.Documentation {
    public class SwashbuckleStartup : FunctionsStartup {
        public override void Configure(IFunctionsHostBuilder builder) {
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
        }
    }
}
