using Invoicer.Repositories;

namespace Invoicer.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _repository;

        public ProviderService(IProviderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Provider>> GetAllProviders()
        {
            return await _repository.GetAllProviders();
        }

        public async Task<Provider> GetProviderById(int id)
        {
            return await _repository.GetProviderById(id);
        }
        public async Task<int?> GetIdByName(string name)
        {
            return await _repository.GetIdByName(name);
        }

        public async Task AddProvider(Provider provider)
        {
            await _repository.AddProvider(provider);
        }
        public async Task AddProvidersAsync(List<Provider> providers)
        {
            await _repository.AddProvidersAsync(providers);
        }
        public async Task UpdateProvider(Provider provider)
        {
            await _repository.UpdateProvider(provider);
        }

        public async Task DeleteProvider(int id)
        {
            await _repository.DeleteProvider(id);
        }
        public async Task DeleteAllProviders()
        {
            await _repository.DeleteAllProviders();
        }
    }
}
