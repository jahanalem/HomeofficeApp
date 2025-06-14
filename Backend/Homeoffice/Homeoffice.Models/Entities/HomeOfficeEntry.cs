using Homeoffice.Models.Entities.Identity;

namespace Homeoffice.Models.Entities
{
    public class HomeOfficeEntry : BaseEntity
    {
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }

        public string? Description { get; set; }

        public bool IsEmailSent { get; set; } = false;

        public string UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
