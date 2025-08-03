namespace WebAPIManagement.DTO
{
    /// <summary>
    /// DTO для отображения ресурса поступления
    /// </summary>
    public class ReceiptResourceDto
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public string ResourceName { get; set; } = null!;
        public int UnitId { get; set; }
        public string UnitName { get; set; } = null!;
        public decimal Quantity { get; set; }
        public int ReceiptDocumentId { get; set; }
    }

    /// <summary>
    /// DTO для создания ресурса поступления
    /// </summary>
    public class CreateReceiptResourceDto
    {
        public int ResourceId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public int ReceiptDocumentId { get; set; }
    }

    /// <summary>
    /// DTO для обновления ресурса поступления
    /// </summary>
    public class UpdateReceiptResourceDto
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public int ReceiptDocumentId { get; set; }
    }
} 