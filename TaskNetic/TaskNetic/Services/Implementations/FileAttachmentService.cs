using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class FileAttachmentService : Repository<FileAttachment>, IFileAttachmentService
    {
        public FileAttachmentService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<FileAttachment>> GetAttachmentsByCardIdAsync(int cardId)
        {
            throw new NotImplementedException();
        }
    }
}
