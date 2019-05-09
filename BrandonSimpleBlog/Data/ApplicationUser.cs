﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrandonSimpleBlog.Data
{
    public class ApplicationUser: IdentityUser
    {
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        public bool HasAvatarImage { get; set; }
    }
}
