using System.Reflection;
using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(easyCloud.Swashbuckle.SwashbuckleStartup))]
namespace easyCloud.Swashbuckle
{
    public class SwashbuckleStartup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
        }
    }
}
