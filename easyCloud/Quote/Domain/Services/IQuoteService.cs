using easyCloud.Quote.Domain.Services.Communication;

namespace easyCloud.Quote.Domain.Services;

public interface IQuoteService
{
    Task<IEnumerable<Models.Quote>> ListAsync();
    Task<QuoteResponse> SaveAsync(Models.Quote quote);
    Task<QuoteResponse> UpdateAsync(int quoteId, Models.Quote quote);
    Task<QuoteResponse> DeleteAsync(int quoteId);
}