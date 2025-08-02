using WebAPIManagement.DTO;
using WebAPIManagement.Interface.Repository;
using WebAPIManagement.Interface.Service;
using WebAPIManagement.Models;

namespace WebAPIManagement.Services
{
    /// <summary>
    /// сервис бизнес-логики для управлением документами поступления
    /// </summary>
    public class ReceipDocumentService: IReceipDocumentService
    {
        private readonly IReceiptDocumentRepository _receiptDocumentRepository;
        private readonly IReceiptResourceRepository _receiptResourceRepository;

        public ReceipDocumentService(IReceiptDocumentRepository receiptDocumentRepository, IReceiptResourceRepository receiptResourceRepository)
        {
            _receiptDocumentRepository = receiptDocumentRepository;
            _receiptResourceRepository = receiptResourceRepository;
        }

        /// <summary>
        /// получение всех документов с их ресурсами
        /// </summary>
        public async Task<IEnumerable<ReceiptDocument>> GetAllDocumentsWithResourcesAsync()
        {
            var documents = await _receiptDocumentRepository.GetAllDocumentsAsync();
            foreach (var doc in documents)
            {
                var resources = await _receiptResourceRepository.GetByDocumentIdAsync(doc.Id);
                doc.ReceiptResources = resources.ToList();
            }
            return documents;
        }

        /// <summary>
        /// получение одного документа по Id с его ресурсами
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ReceiptDocument?> GetDocumentByIdWithResourcesAsync(int id)
        {
            var document = await _receiptDocumentRepository.GetByIdAsync(id);
            if (document == null)
            {
                return null;
            }
            
            var resources = await _receiptResourceRepository.GetByDocumentIdAsync(document.Id);
            document.ReceiptResources = resources.ToList();
            return document;
        }

        /// <summary>
        /// создание нового документа поступления
        /// </summary>
        /// <param name="newDocument"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ReceiptDocument> CreateDocumentAsync(ReceiptDocument newDocument)
        {
            var allDocs = await _receiptDocumentRepository.GetAllDocumentsAsync();
            if (allDocs.Any(d => d.Number == newDocument.Number))
            {
                throw new InvalidOperationException("Документ с таким номером уже существует");
            }

            return await _receiptDocumentRepository.CreateDocumentAsync(newDocument);
        }

        /// <summary>
        /// обновление документа
        /// </summary>
        /// <param name="updatedDocument"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ReceiptDocument> UpdateDocumentAsync(ReceiptDocument updatedDocument)
        {
            var existing = await _receiptDocumentRepository.GetByIdAsync(updatedDocument.Id);
            if (existing == null)
            {
                throw new InvalidOperationException("Документ не найден");
            }

            return await _receiptDocumentRepository.UpdateDocumentAsync(updatedDocument);
        }

        /// <summary>
        /// Удаление документа
        /// </summary>
        public async Task<bool> DeleteDocumentAsync(int documentId)
        {
            return await _receiptDocumentRepository.DeleteDocumentAsync(documentId);
        }

        /// <summary>
        /// пполучение документов с фильтрацией по дате, номерам, ресурсам и единицам измерения
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReceiptDocument>> FilterDocumentsAsync(ReceiptDocumentFilter filter)
        {
            var allDocs = await _receiptDocumentRepository.GetAllDocumentsAsync();
            var allResources = await _receiptResourceRepository.GetAllReceiptResource();

            // присоединение ресурсов
            foreach (var doc in allDocs)
            {
                doc.ReceiptResources = allResources.Where(r => r.ReceiptDocumentId == doc.Id).ToList();
            }

            // фильтрация по дате
            if (filter.StartDate.HasValue || filter.EndDate.HasValue)
            {
                var start = filter.StartDate.HasValue ? DateOnly.FromDateTime(filter.StartDate.Value) : (DateOnly?)null;
                var end = filter.EndDate.HasValue ? DateOnly.FromDateTime(filter.EndDate.Value) : (DateOnly?)null;

                allDocs = allDocs
                    .Where(d =>
                        (!start.HasValue || d.Date >= start.Value) &&
                        (!end.HasValue || d.Date <= end.Value))
                    .ToList();
            }

            // фильтрация по номерам документов
            if (filter.DocumentNumbers?.Any() == true)
            {
                allDocs = allDocs
                    .Where(d => filter.DocumentNumbers.Contains(d.Number))
                    .ToList();
            }

            // фильтрация по ресурсам
            if (filter.ResourceIds?.Any() == true)
            {
                allDocs = allDocs
                    .Where(d => d.ReceiptResources.Any(r => filter.ResourceIds.Contains(r.ResourceId)))
                    .ToList();
            }

            // фильтрация по единицам измерения
            if (filter.UnitIds?.Any() == true)
            {
                allDocs = allDocs
                    .Where(d => d.ReceiptResources.Any(r => filter.UnitIds.Contains(r.UnitId)))
                    .ToList();
            }

            return allDocs;
        }
    }
}

