using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;

namespace ComputerStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportStock([FromBody] IEnumerable<StockImportDto> stockDtos)
        {
            await _stockService.ImportStockAsync(stockDtos);
            return Ok();
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetStockByProductId(int productId)
        {
            var stock = await _stockService.GetStockByProductIdAsync(productId);
            if (stock == null) return NotFound();
            return Ok(stock);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateStock(int productId, [FromBody] int quantity)
        {
            await _stockService.UpdateStockAsync(productId, quantity);
            return NoContent();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteStock(int productId)
        {
            await _stockService.DeleteStockAsync(productId);
            return NoContent();
        }
    }
}
