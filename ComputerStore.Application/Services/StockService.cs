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

        public async Task ImportStockAsync(IEnumerable<StockDto> stockDtos)
        {
            await Task.CompletedTask;
        }

        public async Task<StockDto?> GetStockByProductIdAsync(int productId)
        {
            return await Task.FromResult<StockDto?>(null);
        }
    }
}
