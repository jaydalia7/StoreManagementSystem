using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Services.IServices
{
    public interface IUserService
    {
        Task<(UserDisplayModel, StoreManagmentError)> CreateUser(UserAddModel userAddModel, CancellationToken cancellationToken = default);
        Task<(string, StoreManagmentError)> Login(UserLoginModel userLogin, CancellationToken cancellationToken = default);
        Task<(List<UserDisplayModel>, StoreManagmentError)> GetAllUsers(CancellationToken cancellationToken = default);
        Task<(List<UserDisplayModel>, StoreManagmentError)> GetActiveUsers(CancellationToken cancellationToken = default);
        Task<(string, StoreManagmentError)> RemoveUser(int userId, CancellationToken cancellationToken = default);
    }
}
