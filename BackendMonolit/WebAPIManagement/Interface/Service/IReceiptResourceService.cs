using WebAPIManagement.Models;

namespace WebAPIManagement.Interface.Service
{
    public interface IReceiptResourceService
    {
        Task<ReceiptResource> AddResourceAsync(ReceiptResource newResource);
        Task<bool> DeleteResourceAsync(int resourceid);
        Task<IEnumerable<ReceiptResource>> GetAllReceiptResource();
        Task<IEnumerable<ReceiptResource>> GetByDocumentIdAsync(int receiptDocumentId);
        Task<ReceiptResource?> GetByIdAsync(int resourceid);
        Task<ReceiptResource> UpdateResourceAsync(ReceiptResource updatedResource);
    }
}
