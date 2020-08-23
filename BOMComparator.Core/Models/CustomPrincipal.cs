using System.Linq;
using System.Security.Principal;

namespace BOMComparator.Models
{
    internal class CustomPrincipal : IPrincipal
    {
        private CustomIdentity _currentIdentity;

        public CustomIdentity CurrentIdentity { 
            get { return _currentIdentity ?? new AnonymousIdentity(); }
            set { _currentIdentity = value; } }

        public IIdentity Identity { get { return CurrentIdentity; } }

        public bool IsInRole(string role)
        {
            return _currentIdentity.Roles.Contains(role);
        }
    }
}
