namespace easyCloud.Supplier.Domain.Repositories;

public interface ISupplierRepository
{
    Task<IEnumerable<Models.Supplier>> ListAsync();
    Task AddAsync(Models.Supplier supplier);
    Task<Models.Supplier> FindByIdAsync(int supplierId);
    void Update(Models.Supplier supplier);
    void Remove(Models.Supplier supplier);
}