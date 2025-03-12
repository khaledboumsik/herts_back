using Invoicer.Readers;
using Invoicer.Repositories;
using Invoicer.Services;
using Microsoft.AspNetCore.Mvc;
using Invoicer.Controllers;
namespace Invoicer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _service;
        private readonly ProviderReader _providerReader;
        public ProviderController(IProviderService service, ProviderReader providerReader)
        {
            _service = service;
            _providerReader = providerReader;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Provider>>> GetProvider()
        {
            return Ok(await _service.GetAllProviders());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Provider>> GetProvider(int id)
        {
            var invoice = await _service.GetProviderById(id);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProvider(Provider provider)
        {
            await _service.AddProvider(provider);
            return CreatedAtAction(nameof(GetProvider), new { id = provider.Id }, provider);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProvider(int id, Provider provider)
        {
            if (id != provider.Id) return BadRequest();
            await _service.UpdateProvider(provider);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllProviders()
        {
            await _service.DeleteAllProviders();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            await _service.DeleteProvider(id);
            return NoContent();
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
                _providerReader.ReadFileAsync(stream);
            }

            return Ok("File processed successfully.");
        }
    }
}
