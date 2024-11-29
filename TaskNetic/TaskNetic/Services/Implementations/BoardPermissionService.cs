using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class BoardPermissionService : Repository<BoardPermission>, IBoardPermissionService
    {
        public BoardPermissionService(ApplicationDbContext context) : base(context) { }

    }
}
