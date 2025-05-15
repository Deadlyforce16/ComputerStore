using System.Collections.Generic;

namespace ComputerStore.Application.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public List<int> CategoryIds { get; set; } = new();
    }
} 