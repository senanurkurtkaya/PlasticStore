﻿using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.AllDtos.UserDtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public List<string> Roles { get; set; }
    }
}
