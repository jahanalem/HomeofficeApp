using Microsoft.AspNetCore.Identity;

namespace Homeoffice.Models.Entities.Identity
{
    public class User : IdentityUser, IBaseEntity<string>, IAuditable
    {
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<HomeOfficeEntry> HomeOfficeEntries { get; set; } = new List<HomeOfficeEntry>();
    }
}
