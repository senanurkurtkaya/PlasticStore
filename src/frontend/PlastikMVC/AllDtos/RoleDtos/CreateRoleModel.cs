namespace PlastikMVC.AllDtos.RoleDtos // DTO CREATED ONR
{
    public class CreateRoleModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
