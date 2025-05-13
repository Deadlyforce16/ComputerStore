using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using ComputerStore;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ComputerStore.Application.DTOs;


public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CategoriesControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/categories");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated()
    {
        var dto = new CategoryDto { Name = "IntegrationTestCat" };
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/categories", content);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
} 