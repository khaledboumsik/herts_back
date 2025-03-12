using Invoicer.Repositories;
using OfficeOpenXml;
using Invoicer.Services;
using System.IO;

namespace Invoicer.Readers
{
    public class InvoiceReader : IReader
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public InvoiceReader(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task ReadFileAsync(Stream fileStream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                // Create a new DI scope to ensure the DbContext used in the invoice service is not disposed prematurely.
                using (var scope = _scopeFactory.CreateScope())
                {
                    var invoiceService = scope.ServiceProvider.GetRequiredService<IInvoiceService>();
                    var providerService = scope.ServiceProvider.GetRequiredService<IProviderService>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var provider = worksheet.Cells[row, 2].Text;
                        var contractNumber = worksheet.Cells[row, 3].Text;
                        var factureNumber = worksheet.Cells[row, 4].Text;
                        var factureDateText = worksheet.Cells[row, 5].Text;
                        var depositeDateText = worksheet.Cells[row, 6].Text;
                        var amountText = worksheet.Cells[row, 7].Text;

                        var providerId = await providerService.GetIdByName(provider.ToLower());
                        if (DateTime.TryParseExact(factureDateText, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime factureDate) &&
                            decimal.TryParse(amountText, out decimal amount) &&
                            DateTime.TryParseExact(depositeDateText, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime depositeDate))
                        {
                            Console.WriteLine(providerId);
                            if (providerId != null)
                            {
                                var invoice = new Invoice
                                {
                                    ProviderId = (int)providerId,
                                    NContract = contractNumber,
                                    NFacture = factureNumber,
                                    DateFacture = factureDate,
                                    Amount = amount,
                                    DateDeposite = depositeDate
                                };

                                Console.WriteLine($"New Invoice Created: ProviderId={invoice.ProviderId}, Contract={invoice.NContract}, " +
                                                  $"Invoice Number={invoice.NFacture}, Invoice Date={invoice.DateFacture:yyyy-MM-dd}, " +
                                                  $"Amount={invoice.Amount:C}, Deposit Date={invoice.DateDeposite:yyyy-MM-dd}");

                                await invoiceService.AddInvoice(invoice);
                            }
                            else
                            {
                                Console.WriteLine($"{provider} doesn't exist");
                            }
                        }
                    }
                }
            }
        }
    }
}
