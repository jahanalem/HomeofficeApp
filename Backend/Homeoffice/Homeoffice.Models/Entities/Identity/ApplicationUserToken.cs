using Microsoft.AspNetCore.Identity;

namespace Homeoffice.Models.Entities.Identity
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual User User { get; set; }
    }
}
