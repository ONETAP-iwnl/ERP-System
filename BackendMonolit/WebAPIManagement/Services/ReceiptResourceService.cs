using WebAPIManagement.Interface.Repository;
using WebAPIManagement.Interface.Service;
using WebAPIManagement.Models;

namespace WebAPIManagement.Services
{
    /// <summary>
    /// сервис бизнес-логики для управлением ресурсами поступлениями
    /// </summary>
    public class ReceiptResourceService: IReceiptResourceService
    {
        private IReceiptResourceRepository _receiptResourceRepository;

        public ReceiptResourceService(IReceiptResourceRepository receiptResourceRepository)
        {
            _receiptResourceRepository = receiptResourceRepository;
        }

        /// <summary>
        /// получить все ресурсы из документов поступления
        /// </summary>
        public async Task<IEnumerable<ReceiptResource>> GetAllReceiptResource()
        {
            return await _receiptResourceRepository.GetAllReceiptResource();
        }

        /// <summary>
        /// получить ресурс по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ReceiptResource?> GetByIdAsync(int id)
        {
            return await _receiptResourceRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Получить все ресурсы по ID документа
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReceiptResource>> GetByDocumentIdAsync(int documentId)
        {
            return await _receiptResourceRepository.GetByDocumentIdAsync(documentId);
        }

        /// <summary>
        /// добавить ресурс в документ
        /// </summary>
        /// <param name="newResource"></param>
        /// <returns></returns>
        public async Task<ReceiptResource> AddResourceAsync(ReceiptResource newResource)
        {
            return await _receiptResourceRepository.AddResourceAsync(newResource);
        }

        /// <summary>
        /// обновить ресурс в документе
        /// </summary>
        /// <param name="updatedResource"></param>
        /// <returns></returns>
        public async Task<ReceiptResource> UpdateResourceAsync(ReceiptResource updatedResource)
        {
            return await _receiptResourceRepository.UpdateResourceAsync(updatedResource);
        }

        /// <summary>
        /// Удалить ресурс поступления
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteResourceAsync(int id)
        {
            return await _receiptResourceRepository.DeleteResourceAsync(id);
        }
    }
}
