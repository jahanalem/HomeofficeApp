
namespace Homeoffice.Models.Entities
{
    public interface IBaseEntity<TKey> : IAuditable
    {
        TKey Id { get; set; }
    }
}