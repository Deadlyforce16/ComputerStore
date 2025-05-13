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
        public async Task<IActionResult> ImportStock([FromBody] IEnumerable<StockDto> stockDtos)
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
    }
}
