using Invoicer.Repositories;

namespace Invoicer.Services
{
    public interface IProviderService
    {
        Task<IEnumerable<Provider>> GetAllProviders();
        Task<Provider> GetProviderById(int id);
        Task AddProvider(Provider provider);
        Task UpdateProvider(Provider provider);
        Task DeleteProvider(int id);

        Task<int?> GetIdByName(string name);
        Task AddProvidersAsync(List<Provider> providers);
        Task DeleteAllProviders();
    }
}
