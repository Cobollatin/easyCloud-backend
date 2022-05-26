using easyCloud.Shared.Domain.Services.Communication;

namespace easyCloud.Quote.Domain.Services.Communication;

public class QuoteResponse: BaseResponse<Models.Quote>
{
    public QuoteResponse(string message) : base(message)
    {
    }

    public QuoteResponse(Models.Quote resource) : base(resource)
    {
    }   
}