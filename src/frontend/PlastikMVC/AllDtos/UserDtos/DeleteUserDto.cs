namespace PlastikMVC.AllDtos.UserDtos  // DTO CREATED ONR
{
    public class DeleteUserDto
    {
        public Guid Id { get; set; }
        public int UserName { get; set; }
        public int Email { get; set; }
        public string DeletedBy { get; set; }
        public string Reason { get; set; }    
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow; 
    }
}
