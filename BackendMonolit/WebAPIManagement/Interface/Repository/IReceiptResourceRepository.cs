using WebAPIManagement.Models;

namespace WebAPIManagement.Interface.Repository
{
    public interface IReceiptResourceRepository
    {
        Task<IEnumerable<ReceiptResource>> GetByDocumentIdAsync(int receiptDocumentId);
        Task<IEnumerable<ReceiptResource>> GetAllReceiptResource();
        Task<ReceiptResource?> GetByIdAsync(int resourceid);
        Task<ReceiptResource> AddResourceAsync(ReceiptResource newResource);
        Task<ReceiptResource> UpdateResourceAsync(ReceiptResource updatedResource);
        Task<bool> DeleteResourceAsync(int resourceid);
    }
}
