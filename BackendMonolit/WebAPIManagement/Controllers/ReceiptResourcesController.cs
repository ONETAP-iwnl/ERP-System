using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIManagement.Interface.Service;
using WebAPIManagement.Models;
using WebAPIManagement.DTO;

namespace WebAPIManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptResourcesController : ControllerBase
    {
        private readonly IReceiptResourceService _receiptResourceService;

        public ReceiptResourcesController(IReceiptResourceService receiptResourceService)
        {
            _receiptResourceService = receiptResourceService;
        }

        /// <summary>
        /// Получить все ресурсы поступления
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptResourceDto>>> GetReceiptResources()
        {
            try
            {
                var resources = await _receiptResourceService.GetAllReceiptResource();
                var resourceDtos = resources.Select(rr => new ReceiptResourceDto
                {
                    Id = rr.Id,
                    ResourceId = rr.ResourceId,
                    ResourceName = rr.Resource?.Name ?? "",
                    UnitId = rr.UnitId,
                    UnitName = rr.Unit?.Name ?? "",
                    Quantity = rr.Quantity,
                    ReceiptDocumentId = rr.ReceiptDocumentId
                });
                return Ok(resourceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить ресурс поступления по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptResourceDto>> GetReceiptResource(int id)
        {
            try
            {
                var resource = await _receiptResourceService.GetByIdAsync(id);
                if (resource == null)
                {
                    return NotFound($"Ресурс поступления с ID {id} не найден");
                }
                var resourceDto = new ReceiptResourceDto
                {
                    Id = resource.Id,
                    ResourceId = resource.ResourceId,
                    ResourceName = resource.Resource?.Name ?? "",
                    UnitId = resource.UnitId,
                    UnitName = resource.Unit?.Name ?? "",
                    Quantity = resource.Quantity,
                    ReceiptDocumentId = resource.ReceiptDocumentId
                };
                return Ok(resourceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить ресурсы поступления по ID документа
        /// </summary>
        [HttpGet("document/{documentId}")]
        public async Task<ActionResult<IEnumerable<ReceiptResourceDto>>> GetReceiptResourcesByDocument(int documentId)
        {
            try
            {
                var resources = await _receiptResourceService.GetByDocumentIdAsync(documentId);
                var resourceDtos = resources.Select(rr => new ReceiptResourceDto
                {
                    Id = rr.Id,
                    ResourceId = rr.ResourceId,
                    ResourceName = rr.Resource?.Name ?? "",
                    UnitId = rr.UnitId,
                    UnitName = rr.Unit?.Name ?? "",
                    Quantity = rr.Quantity,
                    ReceiptDocumentId = rr.ReceiptDocumentId
                });
                return Ok(resourceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Добавить ресурс в документ поступления
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ReceiptResourceDto>> AddReceiptResource([FromBody] CreateReceiptResourceDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var resource = new ReceiptResource
                {
                    ResourceId = createDto.ResourceId,
                    UnitId = createDto.UnitId,
                    Quantity = createDto.Quantity,
                    ReceiptDocumentId = createDto.ReceiptDocumentId
                };

                var addedResource = await _receiptResourceService.AddResourceAsync(resource);
                var resourceDto = new ReceiptResourceDto
                {
                    Id = addedResource.Id,
                    ResourceId = addedResource.ResourceId,
                    ResourceName = addedResource.Resource?.Name ?? "",
                    UnitId = addedResource.UnitId,
                    UnitName = addedResource.Unit?.Name ?? "",
                    Quantity = addedResource.Quantity,
                    ReceiptDocumentId = addedResource.ReceiptDocumentId
                };
                return CreatedAtAction(nameof(GetReceiptResource), new { id = resourceDto.Id }, resourceDto);
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
        /// Обновить ресурс поступления
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ReceiptResourceDto>> UpdateReceiptResource(int id, [FromBody] UpdateReceiptResourceDto updateDto)
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

                var resource = new ReceiptResource
                {
                    Id = updateDto.Id,
                    ResourceId = updateDto.ResourceId,
                    UnitId = updateDto.UnitId,
                    Quantity = updateDto.Quantity,
                    ReceiptDocumentId = updateDto.ReceiptDocumentId
                };

                var updatedResource = await _receiptResourceService.UpdateResourceAsync(resource);
                var resourceDto = new ReceiptResourceDto
                {
                    Id = updatedResource.Id,
                    ResourceId = updatedResource.ResourceId,
                    ResourceName = updatedResource.Resource?.Name ?? "",
                    UnitId = updatedResource.UnitId,
                    UnitName = updatedResource.Unit?.Name ?? "",
                    Quantity = updatedResource.Quantity,
                    ReceiptDocumentId = updatedResource.ReceiptDocumentId
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
        /// Удалить ресурс из документа поступления
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReceiptResource(int id)
        {
            try
            {
                var result = await _receiptResourceService.DeleteResourceAsync(id);
                if (!result)
                {
                    return BadRequest("Невозможно удалить ресурс поступления");
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