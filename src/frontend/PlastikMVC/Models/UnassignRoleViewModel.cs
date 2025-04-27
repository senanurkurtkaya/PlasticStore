using Microsoft.AspNetCore.Mvc.Rendering;

namespace PlastikMVC.Models
{
    public class UnassignRoleViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }

    public class UnassignRolePostViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
