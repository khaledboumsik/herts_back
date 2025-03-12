using Microsoft.EntityFrameworkCore;
namespace Invoicer.Repositories
{

    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DatabaseContext _context;

        public InvoiceRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Invoice>> GetAll()
        {
            return await _context.Invoices.Include(i => i.Provider).AsQueryable().ToListAsync();
        }
        public async Task DeleteAll()
        {
             _context.Invoices.RemoveRange(_context.Invoices);
             _context.SaveChanges();
        }
        public async Task<Invoice> GetById(int id)
        {
            return await _context.Invoices.Include(i => i.Provider).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task Add(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
        }
        public async Task AddInvoicesAsync(List<Invoice> invoices)
        {
            foreach (var invoice in invoices)
            {
                try
                {
                    var existingInvoice = await _context.Invoices
                        .FirstOrDefaultAsync(i => i.NContract == invoice.NContract);

                    if (existingInvoice == null)
                    {
                        await _context.Invoices.AddAsync(invoice);
                    }
                    else
                    {
                        Console.WriteLine($"Provider with name {invoice.NContract} already exists.");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing provider {invoice.NContract}: {ex.Message}");
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task Update(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }
    }

}
