using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.Application.DTOs;

namespace ComputerStore.Application.Interfaces
{
    public interface IStockService
    {
        Task ImportStockAsync(IEnumerable<StockImportDto> stockDtos);
        Task<StockDto?> GetStockByProductIdAsync(int productId);
        Task UpdateStockAsync(int productId, int quantity);
        Task DeleteStockAsync(int productId);
    }
}
