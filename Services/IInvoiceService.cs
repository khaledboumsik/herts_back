using Invoicer.Repositories;
namespace Invoicer.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllInvoices();
        Task<Invoice> GetInvoiceById(int id);
        Task AddInvoice(Invoice invoice);
        Task UpdateInvoice(Invoice invoice);
        Task DeleteInvoice(int id);
        Task DeleteAllInvoices();
    }
}
