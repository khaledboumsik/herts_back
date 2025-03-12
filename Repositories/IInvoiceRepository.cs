namespace Invoicer.Repositories
{


    public interface IInvoiceRepository
    {
        Task<IEnumerable<Invoice>> GetAll();
        Task<Invoice> GetById(int id);
        Task Add(Invoice invoice);
        Task Update(Invoice invoice);
        Task Delete(int id);
        Task AddInvoicesAsync(List<Invoice> invoices);
        Task DeleteAll();
    }

}
