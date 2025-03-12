using Microsoft.EntityFrameworkCore;

namespace Invoicer.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly DatabaseContext _context;

        public ProviderRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Provider>> GetAllProviders()
        {
            return await _context.Providers.ToListAsync();
        }

        public async Task<int?> GetIdByName(string providerName)
        {
            return await _context.Providers
                .Where(p => p.Name == providerName)
                .Select(p => (int?)p.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Provider> GetProviderById(int id)
        {
            return await _context.Providers.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddProvider(Provider provider)
        {
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();
        }

        public async Task AddProvidersAsync(List<Provider> providers)
        {
            foreach (var provider in providers)
            {
                try
                {
                    var existingProvider = await _context.Providers
                        .FirstOrDefaultAsync(p => p.Name == provider.Name);

                    if (existingProvider == null)
                    {
                        await _context.Providers.AddAsync(provider);
                    }
                    else
                    {
                        Console.WriteLine($"Provider with name {provider.Name} already exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing provider {provider.Name}: {ex.Message}");
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProvider(Provider provider)
        {
            _context.Providers.Update(provider);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAllProviders()
        {
            _context.Providers.RemoveRange(_context.Providers);
            _context.SaveChanges();
        }
        public async Task DeleteProvider(int id)
        {
            var provider = await _context.Providers.FindAsync(id);
            if (provider != null)
            {
                _context.Providers.Remove(provider);
                await _context.SaveChangesAsync();
            }
        }
    }
}
