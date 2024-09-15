using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserEmailAddress(string emailAddress, CancellationToken cancellationToken = default);
        Task<int> AddUserData(User user, CancellationToken cancellationToken = default);
        Task<User> GetUserById(int Id, CancellationToken cancellationToken = default);
        Task<bool> CheckSuperAdmin(UserLoginModel userLogin, CancellationToken cancellationToken = default);
        Task<List<User>> GetAllUser(string userRole, CancellationToken cancellationToken = default);
        Task<List<User>> GetActiveUsers(string userRole, CancellationToken cancellationToken = default);
        Task<bool> RemoveUser(User user, CancellationToken cancellationToken = default);
    }
}
