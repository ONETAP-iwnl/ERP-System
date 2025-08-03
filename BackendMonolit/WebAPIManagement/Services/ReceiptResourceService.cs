using WebAPIManagement.Interface.Repository;
using WebAPIManagement.Interface.Service;
using WebAPIManagement.Models;
using WebAPIManagement.Repository;

namespace WebAPIManagement.Services
{
    /// <summary>
    /// сервис бизнес-логики для управлением ресурсами поступлениями
    /// </summary>
    public class ReceiptResourceService: IReceiptResourceService
    {
        private readonly IReceiptResourceRepository _receiptResourceRepository;
        private readonly IResourcesRepository _resourceRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IReceiptDocumentRepository _receiptDocumentRepository;

        public ReceiptResourceService(IReceiptResourceRepository receiptResourceRepository, IReceiptDocumentRepository receiptDocumentRepository, IUnitRepository unitRepository, IResourcesRepository resourcesRepository)
        {
            _receiptResourceRepository = receiptResourceRepository;
            _receiptDocumentRepository = receiptDocumentRepository;
            _unitRepository = unitRepository;
            _resourceRepository = resourcesRepository; 
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
        /// получить все ресурсы по id документа
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
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ReceiptResource> AddResourceAsync(ReceiptResource newResource)
        {
            // Проверка на существование документа
            var document = await _receiptDocumentRepository.GetByIdAsync(newResource.ReceiptDocumentId);
            if (document == null)
            {
                throw new InvalidOperationException("Документ поступления не найден");
            }

            // Проверка ресурса
            var resource = await _resourceRepository.GetResourceByIdAsync(newResource.ResourceId);
            if (resource == null || !resource.IsActive)
            {
                throw new InvalidOperationException("Ресурс не существует или находится в архиве");
            }

            // Проверка единицы измерения
            var unit = await _unitRepository.GetUnitByIdAsync(newResource.UnitId);
            if (unit == null || !unit.IsActive)
            {
                throw new InvalidOperationException("Единица измерения не существует или находится в архиве");
            }

            return await _receiptResourceRepository.AddResourceAsync(newResource);
        }

        /// <summary>
        /// обновить ресурс в документе
        /// </summary>
        /// <param name="updatedResource"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ReceiptResource> UpdateResourceAsync(ReceiptResource updatedResource)
        {
            var existing = await _receiptResourceRepository.GetByIdAsync(updatedResource.Id);
            if (existing == null)
            {
                throw new InvalidOperationException("Ресурс поступления не найден");
            }

            var resource = await _resourceRepository.GetResourceByIdAsync(updatedResource.ResourceId);
            if (resource == null || !resource.IsActive)
            {
                throw new InvalidOperationException("Ресурс не существует или находится в архиве");
            }

            var unit = await _unitRepository.GetUnitByIdAsync(updatedResource.UnitId);
            if (unit == null || !unit.IsActive)
            {
                throw new InvalidOperationException("Единица измерения не существует или находится в архиве");
            }

            return await _receiptResourceRepository.UpdateResourceAsync(updatedResource);
        }

        /// <summary>
        /// удалить ресурс поступления
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteResourceAsync(int id)
        {
            var existing = await _receiptResourceRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new InvalidOperationException("Ресурс поступления не найден");
            }

            return await _receiptResourceRepository.DeleteResourceAsync(id);
        }
    }
}
