namespace WebAPIManagement.DTO
{
    /// <summary>
    /// DTO для справочных данных
    /// </summary>
    public class ReferenceDataDto
    {
        public List<ResourceDto> Resources { get; set; } = new List<ResourceDto>();
        public List<UnitDto> Units { get; set; } = new List<UnitDto>();
    }
} 