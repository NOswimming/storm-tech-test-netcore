namespace Todo.Models.TodoItems
{
    public class UserSummaryViewmodel
    {
        public string UserName { get; }
        public string Email { get; }
        public string FullName { get; set; }

        public UserSummaryViewmodel(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }
    }
}