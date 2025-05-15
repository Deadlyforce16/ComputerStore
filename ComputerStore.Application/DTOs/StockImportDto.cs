using System.Collections.Generic;

namespace ComputerStore.Application.DTOs
{
    public class StockImportDto
    {
        public string Name { get; set; } = null!;
        public List<string> Categories { get; set; } = new();
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
} 