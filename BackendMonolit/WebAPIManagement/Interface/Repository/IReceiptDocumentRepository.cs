using WebAPIManagement.Models;

namespace WebAPIManagement.Interface.Repository
{
    public interface IReceiptDocumentRepository
    {
        Task<IEnumerable<ReceiptDocument>> GetAllDocumentsAsync();
        Task<ReceiptDocument?> GetByIdAsync(int documentid);
        Task<ReceiptDocument> CreateDocumentAsync(ReceiptDocument newDocument);
        Task<ReceiptDocument> UpdateDocumentAsync(ReceiptDocument updatedDocument);
        Task<bool> DeleteDocumentAsync(int documentId);
    }
}
