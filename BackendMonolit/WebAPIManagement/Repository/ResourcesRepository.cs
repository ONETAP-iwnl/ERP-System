using Microsoft.EntityFrameworkCore;
using WebAPIManagement.Interface.Repository;
using WebAPIManagement.Models;

namespace WebAPIManagement.Repository
{
    public class ResourcesRepository : IResourcesRepository
    {
        private readonly ErpSystemContext _context;
        public ResourcesRepository(ErpSystemContext context)
        {
            _context = context;
        }
        public async Task<Resource> ArchiveResourceAsync(int resourceid)
        {
            var resource = await _context.Resources.FindAsync(resourceid);
            if (resource == null) throw new Exception("Ресурсы не найдены");
            resource.IsActive = false;
            await _context.SaveChangesAsync();
            return resource;
        }

        public async Task<Resource> CreateResourceAsync(Resource newResource)
        {
            _context.Resources.Add(newResource);
            await _context.SaveChangesAsync();
            return newResource;
        }

        public async Task<bool> DeleteResourceAsync(int resourceId)
        {
            var resource = await _context.Resources.FindAsync(resourceId);
            if (resource == null) return false;
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            return await _context.Resources.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Resource>> GetInactiveResourcesAsync()
        {
            return await _context.Resources.Where(x => !x.IsActive).ToListAsync();
        }

        public async Task<Resource?> GetResourceByIdAsync(int resourceid)
        {
            return await _context.Resources.FindAsync(resourceid);
        }

        public async Task<Resource> UpdateResourceAsync(Resource updatedResource)
        {
            _context.Resources.Update(updatedResource);
            await _context.SaveChangesAsync();
            return updatedResource;
        }
    }
}
