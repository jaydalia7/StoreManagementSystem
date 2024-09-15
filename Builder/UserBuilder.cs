using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Builder
{
    public class UserBuilder
    {
        public static User Convert(UserAddModel userAdd, string encryptedPassword)
        {
            var user = new User(userAdd.Name, userAdd.EmailAddress, encryptedPassword, userAdd.IsAdmin, false);
            return user;
        }
    }
}
