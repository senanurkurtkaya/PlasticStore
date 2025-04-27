using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Role : IdentityRole
    {
     
        [Display(Name = "Rol Açıklaması")]
        [MaxLength(150)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
