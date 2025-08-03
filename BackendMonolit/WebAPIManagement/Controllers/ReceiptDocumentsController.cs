using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIManagement.DTO;
using WebAPIManagement.Interface.Service;
using WebAPIManagement.Models;

namespace WebAPIManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptDocumentsController : ControllerBase
    {
        private readonly IReceipDocumentService _receiptDocumentService;

        public ReceiptDocumentsController(IReceipDocumentService receiptDocumentService)
        {
            _receiptDocumentService = receiptDocumentService;
        }

        /// <summary>
        /// Получить все документы поступления с ресурсами
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptDocumentDto>>> GetReceiptDocuments()
        {
            try
            {
                var documents = await _receiptDocumentService.GetAllDocumentsWithResourcesAsync();
                var documentDtos = documents.Select(doc => new ReceiptDocumentDto
                {
                    Id = doc.Id,
                    Number = doc.Number,
                    Date = doc.Date,
                    ReceiptResources = doc.ReceiptResources.Select(rr => new ReceiptResourceDto
                    {
                        Id = rr.Id,
                        ResourceId = rr.ResourceId,
                        ResourceName = rr.Resource?.Name ?? "",
                        UnitId = rr.UnitId,
                        UnitName = rr.Unit?.Name ?? "",
                        Quantity = rr.Quantity,
                        ReceiptDocumentId = rr.ReceiptDocumentId
                    }).ToList()
                });
                return Ok(documentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить документ поступления по ID с ресурсами
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptDocumentDto>> GetReceiptDocument(int id)
        {
            try
            {
                var document = await _receiptDocumentService.GetDocumentByIdWithResourcesAsync(id);
                if (document == null)
                {
                    return NotFound($"Документ поступления с ID {id} не найден");
                }
                var documentDto = new ReceiptDocumentDto
                {
                    Id = document.Id,
                    Number = document.Number,
                    Date = document.Date,
                    ReceiptResources = document.ReceiptResources.Select(rr => new ReceiptResourceDto
                    {
                        Id = rr.Id,
                        ResourceId = rr.ResourceId,
                        ResourceName = rr.Resource?.Name ?? "",
                        UnitId = rr.UnitId,
                        UnitName = rr.Unit?.Name ?? "",
                        Quantity = rr.Quantity,
                        ReceiptDocumentId = rr.ReceiptDocumentId
                    }).ToList()
                };
                return Ok(documentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Создать новый документ поступления
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ReceiptDocumentDto>> CreateReceiptDocument([FromBody] CreateReceiptDocumentDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var document = new ReceiptDocument
                {
                    Number = createDto.Number,
                    Date = createDto.Date
                };

                var createdDocument = await _receiptDocumentService.CreateDocumentAsync(document);
                var documentDto = new ReceiptDocumentDto
                {
                    Id = createdDocument.Id,
                    Number = createdDocument.Number,
                    Date = createdDocument.Date,
                    ReceiptResources = new List<ReceiptResourceDto>()
                };
                return CreatedAtAction(nameof(GetReceiptDocument), new { id = documentDto.Id }, documentDto);
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
        /// Обновить документ поступления
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ReceiptDocumentDto>> UpdateReceiptDocument(int id, [FromBody] UpdateReceiptDocumentDto updateDto)
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

                var document = new ReceiptDocument
                {
                    Id = updateDto.Id,
                    Number = updateDto.Number,
                    Date = updateDto.Date
                };

                var updatedDocument = await _receiptDocumentService.UpdateDocumentAsync(document);
                var documentDto = new ReceiptDocumentDto
                {
                    Id = updatedDocument.Id,
                    Number = updatedDocument.Number,
                    Date = updatedDocument.Date,
                    ReceiptResources = new List<ReceiptResourceDto>()
                };
                return Ok(documentDto);
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
        /// Удалить документ поступления
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReceiptDocument(int id)
        {
            try
            {
                var result = await _receiptDocumentService.DeleteDocumentAsync(id);
                if (!result)
                {
                    return BadRequest("Невозможно удалить документ поступления");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Фильтрация документов поступления
        /// </summary>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<ReceiptDocumentDto>>> FilterReceiptDocuments([FromBody] ReceiptDocumentFilter filter)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var filteredDocuments = await _receiptDocumentService.FilterDocumentsAsync(filter);
                var documentDtos = filteredDocuments.Select(doc => new ReceiptDocumentDto
                {
                    Id = doc.Id,
                    Number = doc.Number,
                    Date = doc.Date,
                    ReceiptResources = doc.ReceiptResources.Select(rr => new ReceiptResourceDto
                    {
                        Id = rr.Id,
                        ResourceId = rr.ResourceId,
                        ResourceName = rr.Resource?.Name ?? "",
                        UnitId = rr.UnitId,
                        UnitName = rr.Unit?.Name ?? "",
                        Quantity = rr.Quantity,
                        ReceiptDocumentId = rr.ReceiptDocumentId
                    }).ToList()
                });
                return Ok(documentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить отфильтрованные документы поступления (GET запрос для удобства)
        /// </summary>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ReceiptDocumentDto>>> GetFilteredReceiptDocuments(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] List<string>? documentNumbers,
            [FromQuery] List<int>? resourceIds,
            [FromQuery] List<int>? unitIds)
        {
            try
            {
                var filter = new ReceiptDocumentFilter
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    DocumentNumbers = documentNumbers,
                    ResourceIds = resourceIds,
                    UnitIds = unitIds
                };

                var filteredDocuments = await _receiptDocumentService.FilterDocumentsAsync(filter);
                var documentDtos = filteredDocuments.Select(doc => new ReceiptDocumentDto
                {
                    Id = doc.Id,
                    Number = doc.Number,
                    Date = doc.Date,
                    ReceiptResources = doc.ReceiptResources.Select(rr => new ReceiptResourceDto
                    {
                        Id = rr.Id,
                        ResourceId = rr.ResourceId,
                        ResourceName = rr.Resource?.Name ?? "",
                        UnitId = rr.UnitId,
                        UnitName = rr.Unit?.Name ?? "",
                        Quantity = rr.Quantity,
                        ReceiptDocumentId = rr.ReceiptDocumentId
                    }).ToList()
                });
                return Ok(documentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
} 