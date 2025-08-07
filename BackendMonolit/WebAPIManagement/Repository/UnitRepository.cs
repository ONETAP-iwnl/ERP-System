using Microsoft.EntityFrameworkCore;
using WebAPIManagement.Interface.Repository;
using WebAPIManagement.Models;

namespace WebAPIManagement.Repository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ErpSystemContext _context;
        public UnitRepository(ErpSystemContext context) => _context = context;
        public async Task<Unit> ArchiveUnitAsync(int unitId)
        {
            var unit = await _context.Units.FindAsync(unitId);
            if (unit == null) throw new Exception("Единица измерения не найдена");
            unit.IsActive = false;
            await _context.SaveChangesAsync();
            return unit;
        }

        public async Task<Unit> CreateUnitAsync(Unit newUnit)
        {
            _context.Units.Add(newUnit);
            await _context.SaveChangesAsync();
            return newUnit;
        }

        public async Task<bool> DeleteUnitAsync(int unitId)
        {
            var unit = await _context.Units.FindAsync(unitId);
            if (unit == null) return false;
            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            return await _context.Units.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Unit>> GetInactiveUnitsAsync()
        {
            return await _context.Units.Where(x => !x.IsActive).ToListAsync();
        }

        public async Task<Unit?> GetUnitByIdAsync(int id)
        {
            return await _context.Units.FindAsync(id);
        }

        public async Task<Unit> UpdateUnitAsync(Unit updatedUnit)
        {
            var existing = await _context.Units.FindAsync(updatedUnit.Id);
            if (existing == null)
                throw new InvalidOperationException("Единица измерения не найдена.");
            existing.Name = updatedUnit.Name;
            existing.IsActive = updatedUnit.IsActive;

            await _context.SaveChangesAsync();

            return existing;
        }
    }
}
