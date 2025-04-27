using DAL.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User: IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }       
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<CustomerRequest> CustomerRequests { get; set; }
        public ICollection<CustomerRequest> AssignedRequests { get; set; }
        public string RoleName { get; set; } = "User";


    }
}
