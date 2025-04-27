namespace PlastikMVC.AllDtos.UserDtos
{
    public class GetUserByIdModel : BaseDto  // DTO CREATED ONR
    {
        public string Name { get; set; }
        public string Lastname { get; set; }        
        public string EMail { get; set; }
    }
}
