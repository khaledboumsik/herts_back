using ClosedXML.Excel;
using Invoicer.DTOs.DTOHelpers;
using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Exporters
{
    public static class InvoiceExporter
    {
        public static async Task<FileContentResult> ExportExcel(InvoicesRequestDto invoices)
        {
            if (invoices == null || invoices.Invoices.Count == 0)
            {
                throw new ArgumentException("No invoices to export.");
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Invoices");

                // Headers
                worksheet.Cell(1, 1).Value = "Provider Name";
                worksheet.Cell(1, 2).Value = "Contract Number";
                worksheet.Cell(1, 3).Value = "Invoice Number";
                worksheet.Cell(1, 4).Value = "Invoice Date";
                worksheet.Cell(1, 5).Value = "Amount";
                worksheet.Cell(1, 6).Value = "Deposit Date";
                worksheet.Cell(1, 7).Value = "Status";

                // Populate data
                int row = 2;
                foreach (var invoice in invoices.Invoices)
                {
                    worksheet.Cell(row, 1).Value = invoice.ProviderName;
                    worksheet.Cell(row, 2).Value = invoice.NContract;
                    worksheet.Cell(row, 3).Value = invoice.NFacture;
                    worksheet.Cell(row, 4).Value = invoice.DateFacture.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 5).Value = invoice.Amount;
                    worksheet.Cell(row, 6).Value = invoice.DateDeposite.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 7).Value = invoice.Status;
                    worksheet.Cell(row, 7).Style.Fill.BackgroundColor = XLColor.FromName(invoice.StatusColor);
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                // Save to MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin); // Ensure the stream is at the beginning

                    // Return the file as a byte array
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "Invoices.xlsx"
                    };
                }
            }
        }
    }
}
