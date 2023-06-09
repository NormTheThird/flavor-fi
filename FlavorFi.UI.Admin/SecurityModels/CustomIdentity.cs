using System.Security.Principal;
using FlavorFi.Common.Models.DatabaseModels;

namespace FlavorFi.UI.Admin.SecurityModels
{
    public class CustomIdentity : IIdentity
    {
        public IIdentity Identity { get; set; }
        public SecurityModel SecurityModel { get; set; }
        public string Name => ((this.SecurityModel?.FirstName ?? string.Empty).Trim() + " " + (this.SecurityModel?.LastName ?? string.Empty).Trim()).Trim();
        public string AuthenticationType => Identity.AuthenticationType;
        public bool IsAuthenticated => !string.IsNullOrEmpty(this.SecurityModel?.Email);

        public CustomIdentity(SecurityModel securityModel)
        {
            this.Identity = new GenericIdentity(securityModel.Email);
            this.SecurityModel = securityModel;
        }
    }
}