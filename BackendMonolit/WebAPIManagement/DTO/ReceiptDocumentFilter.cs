namespace WebAPIManagement.DTO
{
    /// <summary>
    /// DTO-класс для передачи данных между слоями
    /// </summary>
    public class ReceiptDocumentFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<string>? DocumentNumbers { get; set; }

        public List<int>? ResourceIds { get; set; }

        public List<int>? UnitIds { get; set; }
    }

}
