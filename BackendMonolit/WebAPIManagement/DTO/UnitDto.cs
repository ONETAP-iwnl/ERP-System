namespace WebAPIManagement.DTO
{
    /// <summary>
    /// DTO для отображения единицы измерения
    /// </summary>
    public class UnitDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO для создания единицы измерения
    /// </summary>
    public class CreateUnitDto
    {
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO для обновления единицы измерения
    /// </summary>
    public class UpdateUnitDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
} 