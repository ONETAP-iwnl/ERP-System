using WebAPIManagement.Models;

namespace WebAPIManagement.Interface.Service
{
    public interface IUnitService
    {
        Task<IEnumerable<Unit>> GetAllUnitsAsync();
        Task<IEnumerable<Unit>> GetInactiveUnitsAsync();
        Task<Unit> CreateUnitAsync(Unit unit);
        Task<Unit> ArchiveUnitAsync(int unitId);
        Task<bool> DeleteUnitAsync(int unitId);
        Task<Unit?> GetUnitByIdAsync(int id);
        Task<Unit> UpdateUnitAsync(Unit unit);
    }
}
