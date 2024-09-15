using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using StoreManagementSystem.Data;
using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Models.ViewModels;
using StoreManagementSystem.Repository.IRepository;

namespace StoreManagementSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        public UserRepository(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }
        public async Task<User> GetUserEmailAddress(string emailAddress, CancellationToken cancellationToken = default)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(m => m.EmailAddress.Equals(emailAddress), cancellationToken);
            return user;
        }
        public async Task<int> AddUserData(User user, CancellationToken cancellationToken = default)
        {
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return user.Id;
        }

        public async Task<User> GetUserById(int Id, CancellationToken cancellationToken = default)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(m => m.Id == Id, cancellationToken);
            return user;
        }

        public async Task<bool> CheckSuperAdmin(UserLoginModel userLogin, CancellationToken cancellationToken = default)
        {
            if (userLogin.EmailAddress == _configuration["SuperAdmin:EmailAddress"] && userLogin.Password == _configuration["SuperAdmin:Password"])
                return true;
            return false;
        }

        public async Task<List<User>> GetAllUser(string userRole, CancellationToken cancellationToken = default)
        {
            List<User> user = null;
            if (userRole.Equals("SuperAdmin"))
                user = await _dataContext.Users.Where(m => m.EmailAddress != "superAdmin@super.com").ToListAsync(cancellationToken);
            else
                user = await _dataContext.Users.Where(m => m.IsAdmin == false).ToListAsync(cancellationToken);

            return user;
        }

        public async Task<List<User>> GetActiveUsers(string userRole, CancellationToken cancellationToken = default)
        {
            List<User> user = null;
            if (userRole.Equals("SuperAdmin"))
                user = await _dataContext.Users.Where(m => m.EmailAddress != "superAdmin@super.com" && m.IsDeleted == false).ToListAsync(cancellationToken);
            else
                user = await _dataContext.Users.Where(m => m.IsAdmin == false && m.IsDeleted == false).ToListAsync(cancellationToken);

            return user;
        }

        public async Task<bool> RemoveUser(User user, CancellationToken cancellationToken = default)
        {
            var count = 0;
            var userDeleted = await _dataContext.Users.FirstOrDefaultAsync(m => m.Id == user.Id && m.IsDeleted == false);
            if (userDeleted != null)
            {
                userDeleted.IsDeleted = true;
                _dataContext.Entry(userDeleted).State = EntityState.Modified;
                count = await _dataContext.SaveChangesAsync(cancellationToken);
            }
            return count > 0 ? true : false;


        }
    }
}
