using Invoicer.DTOs.DTOHelpers;

namespace Invoicer.DTOs
{
    public class ExportedInvoiceDto
    {
        public string? ProviderName { get; set; }
        public string NContract { get; set; } = string.Empty;
        public string NFacture { get; set; } = string.Empty;
        public DateTime DateFacture { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateDeposite { get; set; }
        public string? Status { get; set; }
        public string? StatusColor {  get; set; }
    }
}