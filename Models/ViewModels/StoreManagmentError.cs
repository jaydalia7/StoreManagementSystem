namespace StoreManagementSystem.Models.ViewModels
{
    public class StoreManagmentError
    {
        public string Message { get; set; }
        public StoreManagmentError(string message)
        {
            Message = message;
        }
    }
}
