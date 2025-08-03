namespace WebAPIManagement.DTO
{
    /// <summary>
    /// DTO для отображения ресурса
    /// </summary>
    public class ResourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO для создания ресурса
    /// </summary>
    public class CreateResourceDto
    {
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO для обновления ресурса
    /// </summary>
    public class UpdateResourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
} 