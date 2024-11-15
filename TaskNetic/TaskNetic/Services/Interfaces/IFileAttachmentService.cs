using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IFileAttachmentService : IRepository<FileAttachment>
    {
        Task<IEnumerable<FileAttachment>> GetAttachmentsByCardIdAsync(int cardId);
    }
}
