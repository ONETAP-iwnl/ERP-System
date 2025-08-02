using WebAPIManagement.Interface.Repository;
using WebAPIManagement.Interface.Service;
using WebAPIManagement.Models;

namespace WebAPIManagement.Services
{
    /// <summary>
    /// Сервис для бизнес-логики управлением единицами измерения, далее Е.И.
    /// </summary>
    public class UnitService: IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IReceiptResourceRepository _receiptResourceRepository;

        public UnitService(IUnitRepository unitRepository, IReceiptResourceRepository receiptResourceRepository)
        {
            _unitRepository = unitRepository;
            _receiptResourceRepository = receiptResourceRepository;
        }

        /// <summary>
        /// получение всех активных Е.И.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            return await _unitRepository.GetAllUnitsAsync();
        }

        /// <summary>
        /// получение всех не активных Е.И.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Unit>> GetInactiveUnitsAsync()
        {
            return await _unitRepository.GetInactiveUnitsAsync();
        }

        /// <summary>
        /// создание Е.И.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Unit> CreateUnitAsync(Unit unit)
        {
            var existing = (await _unitRepository.GetAllUnitsAsync())
                .FirstOrDefault(x => x.Name == unit.Name && x.IsActive); //проверка на то, есть ли в базе такая Е.И. с таким именем
            if (existing != null)
            {
                throw new InvalidOperationException("Уже существует единица измерения с таким названиваем");
            }
            return await _unitRepository.CreateUnitAsync(unit);
        }

        /// <summary>
        /// перенос Е.И. в архив
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Unit> ArchiveUnitAsync(int unitId)
        {
            var res = await _unitRepository.GetUnitByIdAsync(unitId);
            if(res == null)
            {
                throw new InvalidOperationException("Единица измерения не найдена");
            }
            if(!res.IsActive)
            {
                throw new InvalidOperationException("Единица измерения уже в архиве");
            }
            return await _unitRepository.ArchiveUnitAsync(unitId);
        }

        /// <summary>
        /// удаление Е.И., если она не используется
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> DeleteUnitAsync(int unitId)
        {
            var used = (await _receiptResourceRepository.GetByDocumentIdAsync(unitId)).Any(x => x.UnitId == unitId); //проверка на то, используеться где-то Е.Д. в ресурсах поступления
            if(used)
            {
                throw new InvalidOperationException("Невозхможно удалить единицу измерения, т.к. она используется");
            }
            return await _unitRepository.DeleteUnitAsync(unitId);
        }

        /// <summary>
        /// получение Е.И. по id
        /// </summary>
        /// <returns></returns>
        public async Task<Unit?> GetUnitByIdAsync(int id)
        {
            return await _unitRepository.GetUnitByIdAsync(id);
        }

        /// <summary>
        /// обновление существующей единицы измерения
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Unit> UpdateUnitAsync(Unit unit)
        {
            var existing = await _unitRepository.GetUnitByIdAsync(unit.Id);
            if (existing == null)
            {
                throw new InvalidOperationException("Единица измерения не найдена.");
            }

            var duplicateName = (await _unitRepository.GetAllUnitsAsync())
                .FirstOrDefault(x => x.Name == unit.Name && x.Id != unit.Id);

            if (duplicateName != null)
            {
                throw new InvalidOperationException("Другая единица измерения с таким названием уже существует.");
            }

            return await _unitRepository.UpdateUnitAsync(unit);
        }
    }
}
