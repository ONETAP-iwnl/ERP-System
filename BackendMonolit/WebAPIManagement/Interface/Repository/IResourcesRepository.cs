using WebAPIManagement.Models;

namespace WebAPIManagement.Interface.Repository
{
    public interface IResourcesRepository
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync();
        Task<IEnumerable<Resource>> GetInactiveResourcesAsync();
        Task<Resource?> GetResourceByIdAsync(int resourceid);
        Task<Resource> CreateResourceAsync(Resource newResource);
        Task<Resource> UpdateResourceAsync(Resource updatedResource);
        Task<Resource> ArchiveResourceAsync(int resourceid);
        Task<bool> DeleteResourceAsync(int resourceId);
    }
}
