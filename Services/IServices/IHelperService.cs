using StoreManagementSystem.Models.Domains;

namespace StoreManagementSystem.Services.IServices
{
    public interface IHelperService
    {
        string PasswordSalt(string password);
        string Authenticate(User user, bool IsSuperAdmin);
    }
}
