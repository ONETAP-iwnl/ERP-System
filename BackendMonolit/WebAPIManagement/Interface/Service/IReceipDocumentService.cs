using WebAPIManagement.DTO;
using WebAPIManagement.Models;

namespace WebAPIManagement.Interface.Service
{
    public interface IReceipDocumentService
    {
        Task<IEnumerable<ReceiptDocument>> GetAllDocumentsWithResourcesAsync();
        Task<ReceiptDocument?> GetDocumentByIdWithResourcesAsync(int id);
        Task<ReceiptDocument> CreateDocumentAsync(ReceiptDocument newDocument);
        Task<ReceiptDocument> UpdateDocumentAsync(ReceiptDocument updatedDocument);
        Task<bool> DeleteDocumentAsync(int documentId);
        Task<IEnumerable<ReceiptDocument>> FilterDocumentsAsync(ReceiptDocumentFilter filter);
    }
}
