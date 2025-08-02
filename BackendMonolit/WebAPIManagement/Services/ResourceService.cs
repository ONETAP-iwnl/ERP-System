using WebAPIManagement.Interface.Repository;
using WebAPIManagement.Interface.Service;
using WebAPIManagement.Models;
using WebAPIManagement.Repository;

namespace WebAPIManagement.Services
{
    /// <summary>
    /// Сервис для бизнес-логики управлением ресурсами
    /// </summary>
    public class ResourceService: IResourceService
    {
        private readonly IResourcesRepository _resourcesRepository;
        private readonly IReceiptResourceRepository _receiptResourceRepository;

        public ResourceService(IResourcesRepository resourcesRepository, IReceiptResourceRepository receiptResourceRepository)
        {
            _resourcesRepository = resourcesRepository;
            _receiptResourceRepository = receiptResourceRepository;
        }

        /// <summary>
        /// создание нового ресурса
        /// </summary>
        /// <param name="newResource"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Resource> CreateResourceAsync(Resource newResource)
        {
            var existing = (await _resourcesRepository.GetAllResourcesAsync())
                .FirstOrDefault(x => x.Name == newResource.Name && x.IsActive); //проверка на то, есть ли в базе такой ресурс с таким именем
            if (existing != null)
            {
                throw new InvalidOperationException("Ресурс с таким названиваем уже существует");
            }
            return await _resourcesRepository.CreateResourceAsync(newResource);
        }

        /// <summary>
        /// получение всех активных ресурсов
        /// </summary>
        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            return await _resourcesRepository.GetAllResourcesAsync();
        }

        /// <summary>
        /// получение всех архивированных ресурсов
        /// </summary>
        public async Task<IEnumerable<Resource>> GetInactiveResourcesAsync()
        {
            return await _resourcesRepository.GetInactiveResourcesAsync();
        }

        /// <summary>
        /// Получение ресурса по Id
        /// </summary>
        public async Task<Resource?> GetResourceByIdAsync(int id)
        {
            return await _resourcesRepository.GetResourceByIdAsync(id);
        }

        /// <summary>
        /// Обновление ресурса
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Resource> UpdateResourceAsync(Resource resource)
        {
            var existing = await _resourcesRepository.GetResourceByIdAsync(resource.Id);
            if (existing == null)
            {
                throw new InvalidOperationException("Ресурс не найден");
            }
            var duplicate = (await _resourcesRepository.GetAllResourcesAsync())
                .FirstOrDefault(x => x.Name == resource.Name && x.Id != resource.Id);

            if (duplicate != null)
            {
                throw new InvalidOperationException("Другой ресурс с таким названием уже существует");
            }
             
            return await _resourcesRepository.UpdateResourceAsync(resource);
        }

        /// <summary>
        /// Перенос ресурса в архив
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Resource> ArchiveResourceAsync(int resourceId)
        {
            var resource = await _resourcesRepository.GetResourceByIdAsync(resourceId);
            if (resource == null)
            {
                throw new InvalidOperationException("Ресурс не найден");
            }

            if (!resource.IsActive)
            {
                throw new InvalidOperationException("Ресурс уже в архиве");
            }

            return await _resourcesRepository.ArchiveResourceAsync(resourceId);
        }

        /// <summary>
        /// удаление ресурса, если он не используется
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> DeleteResourceAsync(int resourceId)
        {
            var used = (await _receiptResourceRepository.GetByDocumentIdAsync(resourceId))
                .Any(x => x.ResourceId == resourceId);

            if (used)
            {
                throw new InvalidOperationException("Невозможно удалить ресурс, т.к. он используется");
            }    

            return await _resourcesRepository.DeleteResourceAsync(resourceId);
        }
    }
}
