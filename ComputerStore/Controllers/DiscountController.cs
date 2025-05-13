using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;

namespace ComputerStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateDiscount([FromBody] IEnumerable<StockDto> basket)
        {
            try
            {
                var result = await _discountService.CalculateDiscountAsync(basket);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
