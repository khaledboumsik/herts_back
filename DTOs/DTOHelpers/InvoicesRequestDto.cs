namespace Invoicer.DTOs.DTOHelpers
{
    public class InvoicesRequestDto
    {
        public List<ExportedInvoiceDto> Invoices { get; set; } = new();
    }
}
