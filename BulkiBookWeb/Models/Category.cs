namespace BulkiBookWeb.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
