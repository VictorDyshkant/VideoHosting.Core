
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.Entities
{
    public class UserLogin : IdentityUser
    {
        //public override string Id { get => base.Id; set => base.Id = value; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
