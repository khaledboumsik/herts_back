using Invoicer.DTOs;
using Invoicer.DTOs.DTOHelpers;
using Invoicer.Exporters;
using Invoicer.Readers;
using Invoicer.Repositories;
using Invoicer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _service;
        private readonly InvoiceReader _invoiceReader;

        public InvoiceController(IInvoiceService service, InvoiceReader invoiceReader)
        {
            _service = service;
            _invoiceReader = invoiceReader;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return Ok(await _service.GetAllInvoices());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _service.GetInvoiceById(id);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAllInvoices()
        {
            await _service.DeleteAllInvoices();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreateInvoice(Invoice invoice)
        {
            await _service.AddInvoice(invoice);
            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, Invoice invoice)
        {
            if (id != invoice.Id) return BadRequest();
            await _service.UpdateInvoice(invoice);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            await _service.DeleteInvoice(id);
            return NoContent();
        }
        [HttpPost("download")]
        public async Task<IActionResult> DownloadExcel([FromBody] InvoicesRequestDto invoices)
        {
            try
            {
                var fileResult = await InvoiceExporter.ExportExcel(invoices);
                return fileResult;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required.");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                await _invoiceReader.ReadFileAsync(stream);
            }

            return Ok("File processed successfully.");
        }
    }

}
