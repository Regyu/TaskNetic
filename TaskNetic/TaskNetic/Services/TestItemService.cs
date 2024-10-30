using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskNetic.Data;
using TaskNetic.Models;

namespace TaskNetic.Services
{
    public class TestItemService
    {
        private readonly ApplicationDbContext _context;

        public TestItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TestItem>> GetTestItemsAsync()
        {
            return await _context.TestItems.ToListAsync();
        }
    }
}
