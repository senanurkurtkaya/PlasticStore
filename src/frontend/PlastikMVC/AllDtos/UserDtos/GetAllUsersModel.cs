using PlastikMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.AllDtos.UserDtos   // DTO CREATED ONR
{
    public class GetAllUsersModel
    {
        public string UserName { get; set; }
        public string Lastname { get; set; }   
        public string Email { get; set; }
        public List<Role> Roles { get; set; }
    }
}
