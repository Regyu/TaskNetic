using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IFileAttachmentService : IRepository<FileAttachment>
    {
        Task<IEnumerable<FileAttachment>> GetAttachmentsByCardAsync(Card card);
        Task AddAttachmentToCardAsync(Card card, FileAttachment fileAttachment);
        Task DeleteAttachmentAsync(FileAttachment fileAttachment);
    }
}
