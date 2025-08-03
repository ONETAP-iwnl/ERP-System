using System;

namespace WebAPIManagement.DTO
{
    /// <summary>
    /// DTO для отображения документа поступления
    /// </summary>
    public class ReceiptDocumentDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public DateOnly Date { get; set; }
        public List<ReceiptResourceDto> ReceiptResources { get; set; } = new List<ReceiptResourceDto>();
    }

    /// <summary>
    /// DTO для создания документа поступления
    /// </summary>
    public class CreateReceiptDocumentDto
    {
        public string Number { get; set; } = null!;
        public DateOnly Date { get; set; }
    }

    /// <summary>
    /// DTO для обновления документа поступления
    /// </summary>
    public class UpdateReceiptDocumentDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public DateOnly Date { get; set; }
    }

    /// <summary>
    /// DTO для отображения документа поступления с краткой информацией
    /// </summary>
    public class ReceiptDocumentSummaryDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public DateOnly Date { get; set; }
        public int ResourcesCount { get; set; }
        public decimal TotalQuantity { get; set; }
    }
} 