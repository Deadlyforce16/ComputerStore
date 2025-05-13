using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.Application.Interfaces;
using ComputerStore.Domain.Entities;
using ComputerStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetByIdAsync(int id)
            => await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _context.Categories.Include(c => c.Products).ToListAsync();

        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Category?> GetByNameAsync(string name)
            => await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Name == name);
    }
}
