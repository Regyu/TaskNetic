using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface IFileAttachmentService : IRepository<FileAttachment>
    {
        Task<IEnumerable<FileAttachment>> GetAttachmentsByCardIdAsync(int cardId);
    }
}
