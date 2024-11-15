using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class ColorService : Repository<Color>, IColorService
    {
        public ColorService(ApplicationDbContext context) : base(context) { }
    }
}
