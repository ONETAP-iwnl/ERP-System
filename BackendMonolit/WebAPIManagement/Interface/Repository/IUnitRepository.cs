using WebAPIManagement.Models;

namespace WebAPIManagement.Interface.Repository
{
    public interface IUnitRepository
    {
        Task<IEnumerable<Unit>> GetAllUnitsAsync();
        Task<IEnumerable<Unit>> GetInactiveUnitsAsync();
        Task<Unit?> GetUnitByIdAsync(int id);
        Task<Unit> CreateUnitAsync(Unit newUnit);
        Task<Unit> UpdateUnitAsync(Unit updatedUnit);
        Task<Unit> ArchiveUnitAsync(int unitId);
        Task<bool> DeleteUnitAsync(int unitId);
    }
}
