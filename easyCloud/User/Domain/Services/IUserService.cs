using easyCloud.User.Domain.Services.Communication;

namespace easyCloud.User.Domain.Services;

public interface IUserService
{
    Task<IEnumerable<Models.User>> ListAsync();
    Task<UserResponse> SaveAsync(Models.User user);
    Task<UserResponse> UpdateAsync(int userId, Models.User user);
    Task<UserResponse> DeleteAsync(int userId);
}