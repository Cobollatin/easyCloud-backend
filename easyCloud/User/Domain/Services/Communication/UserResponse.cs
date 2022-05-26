using easyCloud.Shared.Domain.Services.Communication;
namespace easyCloud.User.Domain.Services.Communication;

public class UserResponse : BaseResponse<Models.User>
{
    public UserResponse(string message) : base(message)
    {
    }

    public UserResponse(Models.User resource) : base(resource)
    {
    }   
}