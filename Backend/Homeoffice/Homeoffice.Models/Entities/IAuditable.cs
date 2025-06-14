namespace Homeoffice.Models.Entities
{
    public interface IAuditable
    {
        DateTimeOffset? CreatedDate { get; set; }
        DateTimeOffset? ModifiedDate { get; set; }
    }
}
