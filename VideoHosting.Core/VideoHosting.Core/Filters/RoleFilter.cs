using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoHosting.Core.Filters
{
    public class RoleFilter :Attribute, IAuthorizationFilter
    {
        string _role;
        public RoleFilter(string role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if(!user.IsInRole(_role))
            {
                throw new UnauthorizedAccessException("You do not have such permission");
            }
        }
    }
}
