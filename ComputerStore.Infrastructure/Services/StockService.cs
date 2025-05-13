using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;
using ComputerStore.Domain.Entities;
using ComputerStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ComputerStore.Infrastructure.Services
{
    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StockService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task ImportStockAsync(IEnumerable<StockDto> stockDtos)
        {
            foreach (var stockDto in stockDtos)
            {
                var product = await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == stockDto.ProductId);
                if (product == null)
                {
                    product = new Product { Name = $"Product_{stockDto.ProductId}", Price = 0 };
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                }
                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == product.Id);
                if (stock == null)
                {
                    stock = new Stock { ProductId = product.Id, Quantity = stockDto.Quantity };
                    _context.Stocks.Add(stock);
                }
                else
                {
                    stock.Quantity = stockDto.Quantity;
                    _context.Stocks.Update(stock);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<StockDto?> GetStockByProductIdAsync(int productId)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == productId);
            return stock == null ? null : _mapper.Map<StockDto>(stock);
        }
    }
}
