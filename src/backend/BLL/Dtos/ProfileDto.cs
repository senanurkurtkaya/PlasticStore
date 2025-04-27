﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos
{
    public class ProfileDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }  
        public string UserName { get; set; }  
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
