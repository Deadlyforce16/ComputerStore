using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.Application.DTOs;
using ComputerStore.Application.Interfaces;
using AutoMapper;

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
            return await Task.FromResult<IEnumerable<DiscountCalculationDto>>(new List<DiscountCalculationDto>());
        }
    }
}
