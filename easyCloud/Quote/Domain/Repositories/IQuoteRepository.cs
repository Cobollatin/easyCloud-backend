namespace easyCloud.Quote.Domain.Repositories;

public interface IQuoteRepository
{
    Task<IEnumerable<Models.Quote>> ListAsync();
    Task AddAsync(Models.Quote quote);
    Task<Models.Quote> FindByIdAsync(int quoteId);
    void Update(Models.Quote quote);
    void Remove(Models.Quote quote);
}