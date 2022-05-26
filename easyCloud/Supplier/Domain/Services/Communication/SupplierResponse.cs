using easyCloud.Shared.Domain.Services.Communication;

namespace easyCloud.Supplier.Domain.Services.Communication;

public class SupplierResponse : BaseResponse<Models.Supplier>
{
    public SupplierResponse(string message) : base(message)
    {
    }

    public SupplierResponse(Models.Supplier resource) : base(resource)
    {
    }   
}