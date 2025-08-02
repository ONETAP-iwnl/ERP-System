using Microsoft.EntityFrameworkCore;
using WebAPIManagement.Interface.Repository;
using WebAPIManagement.Models;

namespace WebAPIManagement.Repository
{
    public class ReceiptResourceRepository : IReceiptResourceRepository
    {
        private readonly ErpSystemContext _context;
        public ReceiptResourceRepository(ErpSystemContext context)
        {
            _context = context;
        }
        public async Task<ReceiptResource> AddResourceAsync(ReceiptResource newResource)
        {
            _context.ReceiptResources.Add(newResource);
            await _context.SaveChangesAsync();
            return newResource;
        }

        public async Task<bool> DeleteResourceAsync(int resourceid)
        {
            var resource = await _context.ReceiptResources.FindAsync(resourceid);
            if (resource == null) return false;
            _context.ReceiptResources.Remove(resource);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ReceiptResource>> GetAllReceiptResource()
        {
            return await _context.ReceiptResources.ToListAsync();        
        }

        public async Task<IEnumerable<ReceiptResource>> GetByDocumentIdAsync(int receiptDocumentId)
        {
            return await _context.ReceiptResources.Where(x => x.ReceiptDocumentId == receiptDocumentId).ToListAsync();
        }

        public async Task<ReceiptResource?> GetByIdAsync(int resourceid)
        {
            return await _context.ReceiptResources.FindAsync(resourceid);
        }

        public async Task<ReceiptResource> UpdateResourceAsync(ReceiptResource updatedResource)
        {
            _context.ReceiptResources.Update(updatedResource);
            await _context.SaveChangesAsync();
            return updatedResource;
        }
    }
}
