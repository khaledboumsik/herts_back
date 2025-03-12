using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Invoicer.Repositories
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string NFacture { get; set; }

        [Required]
        public required string NContract { get; set; }


        [Column(TypeName = "date")] // Store only the date part in the database
        public DateTime DateFacture { get; set; }

        [Column(TypeName = "date")] // Store only the date part in the database
        public DateTime DateDeposite { get; set; }

        [ForeignKey("Provider")]
        public int ProviderId { get; set; }

        public decimal Amount { get; set; }
        public Provider? Provider { get; internal set; }
    }
}
