using Microsoft.EntityFrameworkCore;
using WebAPIManagement.Interface.Repository;
using WebAPIManagement.Models;

namespace WebAPIManagement.Repository
{
    public class ReceiptDocumentRepository : IReceiptDocumentRepository
    {
        private readonly ErpSystemContext _context;

        public ReceiptDocumentRepository(ErpSystemContext context)
        {
            _context = context;
        }

        public async Task<ReceiptDocument> CreateDocumentAsync(ReceiptDocument newDocument)
        {
            _context.ReceiptDocuments.Add(newDocument);
            await _context.SaveChangesAsync();
            return newDocument;
        }

        public async Task<bool> DeleteDocumentAsync(int documentId)
        {
            var document = await _context.ReceiptDocuments.FindAsync(documentId);
            if (document == null) return false;
            _context.ReceiptDocuments.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ReceiptDocument>> GetAllDocumentsAsync()
        {
            return await _context.ReceiptDocuments.ToListAsync();
        }

        public async Task<ReceiptDocument?> GetByIdAsync(int documentid)
        {
            return await _context.ReceiptDocuments.FindAsync(documentid);
        }

        public async Task<ReceiptDocument> UpdateDocumentAsync(ReceiptDocument updatedDocument)
        {
            _context.ReceiptDocuments.Update(updatedDocument);
            await _context.SaveChangesAsync();
            return updatedDocument;
        }
    }
}
