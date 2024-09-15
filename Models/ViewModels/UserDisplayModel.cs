namespace StoreManagementSystem.Models.ViewModels
{
    public class UserDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class NullUserDisplayModel : UserDisplayModel { }
    public class NullUserListDisplayModel : List<UserDisplayModel> { }
}
