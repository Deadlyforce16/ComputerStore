using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.Application.DTOs;

namespace ComputerStore.Application.Interfaces
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountCalculationDto>> CalculateDiscountAsync(IEnumerable<StockDto> basket);
    }
}
