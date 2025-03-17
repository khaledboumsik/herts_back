using Invoicer.Repositories;

namespace Invoicer.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;

        public InvoiceService(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoices()
        {
            return await _repository.GetAll();
        }
        public async Task DeleteAllInvoices()
        {
            await _repository.DeleteAll();
        }
        public async Task<Invoice> GetInvoiceById(int id)
        {
            return await _repository.GetById(id);
        }
        public async Task AddInvoice(Invoice invoice)
        {
            await _repository.Add(invoice);
        }

        public async Task UpdateInvoice(Invoice invoice)
        {
            await _repository.Update(invoice);
        }

        public async Task DeleteInvoice(int id)
        {
            await _repository.Delete(id);
        }
    }
}
