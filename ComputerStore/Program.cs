using ComputerStore.Application.Interfaces;
using ComputerStore.Infrastructure.Data;
using ComputerStore.Infrastructure.Repositories;
using ComputerStore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ComputerStore.Application.Services;
using ComputerStore.Application.Mappings;
using Microsoft.Extensions.Configuration;

namespace ComputerStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ComputerStore API",
                    Version = "v1",
                    Description = "API for ComputerStore project"
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=ComputerStoreDb;Trusted_Connection=True;";
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IStockService, ComputerStore.Application.Services.StockService>();
            builder.Services.AddScoped<IDiscountService, ComputerStore.Infrastructure.Services.DiscountService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<ProductService>(provider =>

            {
                var productRepo = provider.GetRequiredService<IProductRepository>();
                var categoryRepo = provider.GetRequiredService<ICategoryRepository>();
                var mapper = provider.GetRequiredService<AutoMapper.IMapper>();
                return new ProductService(productRepo, categoryRepo, mapper);
            });
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
