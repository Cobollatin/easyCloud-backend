using easyCloud.Supplier.Domain.Services.Communication;

namespace easyCloud.Supplier.Domain.Services;

public interface ISupplierService
{
    Task<IEnumerable<Models.Supplier>> ListAsync();
    Task<SupplierResponse> SaveAsync(Models.Supplier supplier);
    Task<SupplierResponse> UpdateAsync(int supplierId, Models.Supplier supplier);
    Task<SupplierResponse> DeleteAsync(int supplierId);
}