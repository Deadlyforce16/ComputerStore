using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;
using ComputerStore.Domain.Entities;
using AutoMapper;

namespace ComputerStore.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public StockService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task ImportStockAsync(System.Collections.Generic.IEnumerable<ComputerStore.Application.DTOs.StockImportDto> stockDtos)
        {
            foreach (var item in stockDtos)
            {
                var categories = new System.Collections.Generic.List<ComputerStore.Domain.Entities.Category>();
                foreach (var catName in item.Categories)
                {
                    var category = await _categoryRepository.GetByNameAsync(catName.Trim());
                    if (category == null)
                    {
                        category = new ComputerStore.Domain.Entities.Category { Name = catName.Trim() };
                        await _categoryRepository.AddAsync(category);
                    }
                    categories.Add(category);
                }

                var product = await _productRepository.GetByNameAsync(item.Name);
                if (product == null)
                {
                    product = new ComputerStore.Domain.Entities.Product
                    {
                        Name = item.Name,
                        Price = item.Price,
                        Categories = categories
                    };
                    await _productRepository.AddAsync(product);
                }
                else
                {
                    product.Price = item.Price;
                    product.Categories = categories;
                    await _productRepository.UpdateAsync(product);
                }

                if (product.Stock == null)
                {
                    product.Stock = new ComputerStore.Domain.Entities.Stock { Product = product, ProductId = product.Id, Quantity = item.Quantity };
                }
                else
                {
                    product.Stock.Quantity = item.Quantity;
                }
                await _productRepository.UpdateAsync(product); // This saves the Stock entity as well
            }
        }

        public async Task<ComputerStore.Application.DTOs.StockDto?> GetStockByProductIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.Stock == null)
                return null;
            return new ComputerStore.Application.DTOs.StockDto { ProductId = product.Id, Quantity = product.Stock.Quantity };
        }

        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception($"Product with ID {productId} does not exist.");
            if (product.Stock == null)
            {
                product.Stock = new ComputerStore.Domain.Entities.Stock { Product = product, Quantity = quantity };
            }
            else
            {
                product.Stock.Quantity = quantity;
            }
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteStockAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.Stock == null)
                return;
            product.Stock.Quantity = 0;
            await _productRepository.UpdateAsync(product);
        }
    }
}
