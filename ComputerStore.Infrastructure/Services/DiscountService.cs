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
    public class DiscountService : IDiscountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DiscountService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DiscountCalculationDto>> CalculateDiscountAsync(IEnumerable<StockDto> basket)
        {
            var result = new List<DiscountCalculationDto>();
            var productIds = basket.Select(b => b.ProductId).ToList();
            var products = await _context.Products.Include(p => p.Categories).Include(p => p.Stock).Where(p => productIds.Contains(p.Id)).ToListAsync();

            foreach (var item in basket)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null || product.Stock == null || product.Stock.Quantity < item.Quantity)
                {
                    throw new Exception($"Not enough stock for product ID {item.ProductId}");
                }
            }

            var categoryGroups = basket
                .SelectMany(b =>
                    products.First(p => p.Id == b.ProductId).Categories.Select(c => new { b, c }))
                .GroupBy(x => x.c.Name);

            foreach (var group in categoryGroups)
            {
                var items = group.Select(x => x.b).ToList();
                if (items.Count > 1)
                {
                    var first = items.First();
                    var product = products.First(p => p.Id == first.ProductId);
                    result.Add(new DiscountCalculationDto
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        CategoryName = group.Key,
                        Quantity = first.Quantity,
                        UnitPrice = product.Price,
                        DiscountApplied = product.Price * 0.05m,
                        DiscountedPrice = product.Price * 0.95m
                    });
                    foreach (var item in items.Skip(1))
                    {
                        var prod = products.First(p => p.Id == item.ProductId);
                        result.Add(new DiscountCalculationDto
                        {
                            ProductId = prod.Id,
                            ProductName = prod.Name,
                            CategoryName = group.Key,
                            Quantity = item.Quantity,
                            UnitPrice = prod.Price,
                            DiscountApplied = 0,
                            DiscountedPrice = prod.Price
                        });
                    }
                }
                else
                {
                    var item = items.First();
                    var product = products.First(p => p.Id == item.ProductId);
                    result.Add(new DiscountCalculationDto
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        CategoryName = group.Key,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        DiscountApplied = 0,
                        DiscountedPrice = product.Price
                    });
                }
            }
            return result;
        }
    }
}
