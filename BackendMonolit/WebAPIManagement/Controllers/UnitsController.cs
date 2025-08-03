using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIManagement.Interface.Service;
using WebAPIManagement.Models;
using WebAPIManagement.DTO;

namespace WebAPIManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        /// <summary>
        /// получить все активные единицы измерения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitDto>>> GetUnits()
        {
            try
            {
                var units = await _unitService.GetAllUnitsAsync();
                var unitDtos = units.Select(u => new UnitDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    IsActive = u.IsActive
                });
                return Ok(unitDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// получить все неактивные единицы измерения
        /// </summary>
        /// <returns></returns>
        [HttpGet("inactive")]
        public async Task<ActionResult<IEnumerable<UnitDto>>> GetInactiveUnits()
        {
            try
            {
                var units = await _unitService.GetInactiveUnitsAsync();
                var unitDtos = units.Select(u => new UnitDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    IsActive = u.IsActive
                });
                return Ok(unitDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// получить единицу измерения по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UnitDto>> GetUnit(int id)
        {
            try
            {
                var unit = await _unitService.GetUnitByIdAsync(id);
                if (unit == null)
                {
                    return NotFound($"Единица измерения с ID {id} не найдена");
                }
                var unitDto = new UnitDto
                {
                    Id = unit.Id,
                    Name = unit.Name,
                    IsActive = unit.IsActive
                };
                return Ok(unitDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// создать новую единицу измерения
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<UnitDto>> CreateUnit([FromBody] CreateUnitDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var unit = new Unit
                {
                    Name = createDto.Name,
                    IsActive = createDto.IsActive
                };

                var createdUnit = await _unitService.CreateUnitAsync(unit);
                var unitDto = new UnitDto
                {
                    Id = createdUnit.Id,
                    Name = createdUnit.Name,
                    IsActive = createdUnit.IsActive
                };
                return CreatedAtAction(nameof(GetUnit), new { id = unitDto.Id }, unitDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// обновить единицу измерения
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UnitDto>> UpdateUnit(int id, [FromBody] UpdateUnitDto updateDto)
        {
            try
            {
                if (id != updateDto.Id)
                {
                    return BadRequest("ID в URL не соответствует ID в теле запроса");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var unit = new Unit
                {
                    Id = updateDto.Id,
                    Name = updateDto.Name,
                    IsActive = updateDto.IsActive
                };

                var updatedUnit = await _unitService.UpdateUnitAsync(unit);
                var unitDto = new UnitDto
                {
                    Id = updatedUnit.Id,
                    Name = updatedUnit.Name,
                    IsActive = updatedUnit.IsActive
                };
                return Ok(unitDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// перевести единицу измерения в архив
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/archive")]
        public async Task<ActionResult<UnitDto>> ArchiveUnit(int id)
        {
            try
            {
                var archivedUnit = await _unitService.ArchiveUnitAsync(id);
                var unitDto = new UnitDto
                {
                    Id = archivedUnit.Id,
                    Name = archivedUnit.Name,
                    IsActive = archivedUnit.IsActive
                };
                return Ok(unitDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// удалить единицу измерения (только если она не используется)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUnit(int id)
        {
            try
            {
                var result = await _unitService.DeleteUnitAsync(id);
                if (!result)
                {
                    return BadRequest("Невозможно удалить единицу измерения, так как она используется в документах");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
} 