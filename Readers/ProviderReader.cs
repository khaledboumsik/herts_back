using Invoicer.Repositories;
using Invoicer.Services;
using OfficeOpenXml;

namespace Invoicer.Readers
{
    public class ProviderReader : IReader
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ProviderReader(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task ReadFileAsync(Stream fileStream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.End.Row;
                var providers = new List<Provider>();

                for (int row = 2; row <= rowCount; row++)
                {
                    var provider = worksheet.Cells[row, 2].Text?.Trim();
                    var date = worksheet.Cells[row, 3].Text?.Trim();

                    if (string.IsNullOrEmpty(provider) || string.IsNullOrEmpty(date))
                        continue;

                    Console.WriteLine($"Processing Row {row}: {provider}, {date}");

                    providers.Add(new Provider
                    {
                        Name = provider.ToLower(),
                        DateLimit = int.Parse(date.Split(" ")[0]),
                    });
                }

                if (providers.Count > 0)
                {
                    // Create a new DI scope to ensure the DbContext used in the provider service is not disposed prematurely.
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var providerService = scope.ServiceProvider.GetRequiredService<IProviderService>();
                        await providerService.AddProvidersAsync(providers); // Bulk insert
                    }
                }
            }
        }
    }
}
