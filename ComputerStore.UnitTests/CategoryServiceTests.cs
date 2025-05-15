using Xunit;
using Moq;
using AutoMapper;
using ComputerStore.Application.Services;
using ComputerStore.Application.Interfaces;
using ComputerStore.Domain.Entities;
using ComputerStore.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly IMapper _mapper;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _categoryRepoMock = new Mock<ICategoryRepository>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Category, CategoryDto>().ReverseMap();
        });
        _mapper = config.CreateMapper();
        _service = new CategoryService(_categoryRepoMock.Object, _mapper);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCategories()
    {
        var categories = new List<Category> { new Category { Id = 1, Name = "Test" } };
        _categoryRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

        var result = await _service.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("Test", result.First().Name);
    }

    [Fact]
    public async Task CreateAsync_AddsCategoryAndReturnsDto()
    {
        var dto = new CreateCategoryDto { Name = "NewCat" };
        _categoryRepoMock.Setup(r => r.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto);

        Assert.Equal("NewCat", result.Name);
    }
} 