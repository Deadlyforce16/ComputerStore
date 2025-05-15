using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;
using AutoMapper;
using ComputerStore.Domain.Entities;

namespace ComputerStore.Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public DiscountService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DiscountCalculationDto>> CalculateDiscountAsync(IEnumerable<StockDto> basket)
        {
            var basketList = basket.ToList();
            var result = new List<DiscountCalculationDto>();
            var products = new List<Product>();
            foreach (var item in basketList)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with ID {item.ProductId} does not exist.");
                if (product.Stock == null || product.Stock.Quantity < item.Quantity)
                    throw new Exception($"Not enough stock for product {product.Name}.");
                products.Add(product);
            }

            var categoryProductCounts = new Dictionary<int, int>();
            foreach (var item in basketList)
            {
                var product = products.First(p => p.Id == item.ProductId);
                foreach (var cat in product.Categories)
                {
                    if (!categoryProductCounts.ContainsKey(cat.Id))
                        categoryProductCounts[cat.Id] = 0;
                    categoryProductCounts[cat.Id] += item.Quantity;
                }
            }

            foreach (var item in basketList)
            {
                var product = products.First(p => p.Id == item.ProductId);
                string categoryName = product.Categories.FirstOrDefault()?.Name ?? "";
                decimal discount = 0;
                decimal discountedPrice = product.Price * item.Quantity;

                bool qualifiesForDiscount = product.Categories.Any(cat => categoryProductCounts[cat.Id] > 1);

                if (qualifiesForDiscount && item.Quantity > 0)
                {
                    discount = product.Price * 0.05m;
                    discountedPrice = (product.Price - discount) + product.Price * (item.Quantity - 1);
                }

                result.Add(new DiscountCalculationDto
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    CategoryName = categoryName,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    DiscountApplied = discount,
                    DiscountedPrice = discountedPrice
                });
            }
            return result;
        }
    }
}
