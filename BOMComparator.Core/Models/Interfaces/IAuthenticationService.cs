namespace BOMComparator.Models
{
    internal interface IAuthenticationService
    {
        User AuthenticateUser(string username, string password);
    }
}
