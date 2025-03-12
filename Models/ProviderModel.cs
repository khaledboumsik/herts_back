using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Add this
namespace Invoicer.Repositories
{
    public class Provider
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public int DateLimit { get; set; }
        [JsonIgnore]
        public List<Invoice>? Invoices { get; set; }
    }
}
