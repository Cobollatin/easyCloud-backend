namespace easyCloud.Provider.Domain.Repositories;

public interface IProviderRepository
{
    Task<IEnumerable<Models.Provider>> ListAsync();
    Task AddAsync(Models.Provider provider);
    Task<Models.Provider> FindByIdAsync(int providerId);
    void Update(Models.Provider provider);
    void Remove(Models.Provider provider);
}