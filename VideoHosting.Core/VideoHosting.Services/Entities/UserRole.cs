
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.Entities
{
    public class UserRole : IdentityRole
    {
       public string Description { get; set; }
    }
}
