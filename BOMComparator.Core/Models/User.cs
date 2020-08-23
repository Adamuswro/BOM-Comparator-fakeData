namespace BOMComparator.Models
{
    public class User
    {
        public User(string email, string[] roles)
        {
            Email = email;
            Roles = roles;
        }

        public string Email { get; set; }
        public string[] Roles { get; set; }

    }
}
