using DocumentFormat.OpenXml.Presentation;
using System.Security.Principal;

namespace BOMComparator.Models
{
    class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name, string[] roles)
        {
            Name = name;
            Roles = roles;
        }

        /// <summary>
        /// Name of the user - company email adress.
        /// </summary>
        public string Name {get;private set;}
        public string[] Roles;

        #region IIdentity Members
        public string AuthenticationType { get {return "Custom authentication"; } }
        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
        #endregion
    }
}
