using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIManagement.Interface.Service;
using WebAPIManagement.Models;
using WebAPIManagement.DTO;

namespace WebAPIManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourcesController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        /// <summary>
        /// Получить все активные ресурсы
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetResources()
        {
            try
            {
                var resources = await _resourceService.GetAllResourcesAsync();
                var resourceDtos = resources.Select(r => new ResourceDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsActive = r.IsActive
                });
                return Ok(resourceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить все неактивные (архивные) ресурсы
        /// </summary>
        [HttpGet("inactive")]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetInactiveResources()
        {
            try
            {
                var resources = await _resourceService.GetInactiveResourcesAsync();
                var resourceDtos = resources.Select(r => new ResourceDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsActive = r.IsActive
                });
                return Ok(resourceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить ресурс по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceDto>> GetResource(int id)
        {
            try
            {
                var resource = await _resourceService.GetResourceByIdAsync(id);
                if (resource == null)
                {
                    return NotFound($"Ресурс с ID {id} не найден");
                }
                var resourceDto = new ResourceDto
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    IsActive = resource.IsActive
                };
                return Ok(resourceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Создать новый ресурс
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ResourceDto>> CreateResource([FromBody] CreateResourceDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var resource = new Resource
                {
                    Name = createDto.Name,
                    IsActive = createDto.IsActive
                };

                var createdResource = await _resourceService.CreateResourceAsync(resource);
                var resourceDto = new ResourceDto
                {
                    Id = createdResource.Id,
                    Name = createdResource.Name,
                    IsActive = createdResource.IsActive
                };
                return CreatedAtAction(nameof(GetResource), new { id = resourceDto.Id }, resourceDto);
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
        /// Обновить ресурс
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ResourceDto>> UpdateResource(int id, [FromBody] UpdateResourceDto updateDto)
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

                var resource = new Resource
                {
                    Id = updateDto.Id,
                    Name = updateDto.Name,
                    IsActive = updateDto.IsActive
                };

                var updatedResource = await _resourceService.UpdateResourceAsync(resource);
                var resourceDto = new ResourceDto
                {
                    Id = updatedResource.Id,
                    Name = updatedResource.Name,
                    IsActive = updatedResource.IsActive
                };
                return Ok(resourceDto);
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
        /// Перевести ресурс в архив
        /// </summary>
        [HttpPut("{id}/archive")]
        public async Task<ActionResult<ResourceDto>> ArchiveResource(int id)
        {
            try
            {
                var archivedResource = await _resourceService.ArchiveResourceAsync(id);
                var resourceDto = new ResourceDto
                {
                    Id = archivedResource.Id,
                    Name = archivedResource.Name,
                    IsActive = archivedResource.IsActive
                };
                return Ok(resourceDto);
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
        /// Удалить ресурс (только если он не используется)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteResource(int id)
        {
            try
            {
                var result = await _resourceService.DeleteResourceAsync(id);
                if (!result)
                {
                    return BadRequest("Невозможно удалить ресурс, так как он используется в документах");
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
