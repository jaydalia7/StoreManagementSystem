using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagementSystem.Models.Domains
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }

        public User(string name, string emailAddress, string password, bool isAdmin, bool isDeleted)
        {
            Name = name;
            EmailAddress = emailAddress;
            Password = password;
            IsAdmin = isAdmin;
            IsDeleted = isDeleted;
        }
    }
}
