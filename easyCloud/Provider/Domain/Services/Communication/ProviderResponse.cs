using easyCloud.Shared.Domain.Services.Communication;

namespace easyCloud.Provider.Domain.Services.Communication;

public class ProviderResponse : BaseResponse<Models.Provider>
{
    public ProviderResponse(string message) : base(message)
    {
    }

    public ProviderResponse(Models.Provider resource) : base(resource)
    {
    }   
}