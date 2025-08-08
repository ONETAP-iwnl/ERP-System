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
            return await _context.ReceiptDocuments
                .Include(d => d.ReceiptResources)
                    .ThenInclude(rr => rr.Resource)
                .Include(d => d.ReceiptResources)
                    .ThenInclude(rr => rr.Unit)
                .ToListAsync();
        }

        public async Task<ReceiptDocument?> GetByIdAsync(int documentid)
        {
            return await _context.ReceiptDocuments
                .Include(d => d.ReceiptResources)
                    .ThenInclude(rr => rr.Resource)
                .Include(d => d.ReceiptResources)
                    .ThenInclude(rr => rr.Unit)
                .FirstOrDefaultAsync(d => d.Id == documentid);
        }

        public async Task<ReceiptDocument> UpdateDocumentAsync(ReceiptDocument updatedDocument)
        {
            var existingDocument = await _context.ReceiptDocuments.FindAsync(updatedDocument.Id);
            if (existingDocument == null)
            {
                throw new InvalidOperationException($"Документ с ID {updatedDocument.Id} не найден");
            }
            existingDocument.Number = updatedDocument.Number;
            existingDocument.Date = updatedDocument.Date;

            await _context.SaveChangesAsync();
            return existingDocument;
        }
    }
}
