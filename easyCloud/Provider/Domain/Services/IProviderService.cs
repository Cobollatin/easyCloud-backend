using easyCloud.Provider.Domain.Services.Communication;

namespace easyCloud.Provider.Domain.Services;

public interface IProviderService
{
    Task<IEnumerable<Models.Provider>> ListAsync();
    Task<ProviderResponse> SaveAsync(Models.Provider provider);
    Task<ProviderResponse> UpdateAsync(int providerId, Models.Provider provider);
    Task<ProviderResponse> DeleteAsync(int providerId);
}