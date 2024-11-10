using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Implementations;

namespace TaskNetic.Services.Interfaces
{
    public class ColorService : Repository<Color>, IColorService
    {
        public ColorService(ApplicationDbContext context) : base(context) { }
    }
}
