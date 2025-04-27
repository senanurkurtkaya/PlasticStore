using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Serialization;

namespace PlastikMVC.Models
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }

    public class AssignRolePostViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
