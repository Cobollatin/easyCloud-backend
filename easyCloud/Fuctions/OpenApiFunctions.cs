using System;
using System.Net.Http;
using System.Threading.Tasks;

using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace easyCloud.OpenApi {
    /// <summary>
    /// 
    /// </summary>
    public static class OpenApiFunctions {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="swashbuckleClient"></param>
        /// <returns></returns>
        [SwaggerIgnore]
        [FunctionName("OpenApiJson")]
        public static Task<HttpResponseMessage> OpenApiJson([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "openapi/json")] HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashbuckleClient) {
            return Task.FromResult(swashbuckleClient.CreateSwaggerJsonDocumentResponse(req));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="swashbuckleClient"></param>
        /// <returns></returns>
        [SwaggerIgnore]
        [FunctionName("OpenApiUI")]
        public static Task<HttpResponseMessage> OpenApiUI([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "openapi/ui")] HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashbuckleClient) {
            // CreateOpenApiUIResponse generates the HTML page from the JSON results
            return Task.FromResult(swashbuckleClient.CreateSwaggerUIResponse(req, "openapi/json"));
        }
    }
}