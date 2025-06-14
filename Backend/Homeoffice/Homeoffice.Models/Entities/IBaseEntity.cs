
namespace Homeoffice.Models.Entities
{
    public interface IBaseEntity<TKey>
    {
        TKey Id { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}