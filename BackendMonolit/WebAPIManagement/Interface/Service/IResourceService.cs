using WebAPIManagement.Models;

namespace WebAPIManagement.Interface.Service
{
    public interface IResourceService
    {
        Task<Resource> CreateResourceAsync(Resource newResource);
        Task<IEnumerable<Resource>> GetAllResourcesAsync();
        Task<IEnumerable<Resource>> GetInactiveResourcesAsync();
        Task<Resource?> GetResourceByIdAsync(int id);
        Task<Resource> UpdateResourceAsync(Resource resource);
        Task<Resource> ArchiveResourceAsync(int resourceId);
        Task<bool> DeleteResourceAsync(int resourceId);
    }
}
