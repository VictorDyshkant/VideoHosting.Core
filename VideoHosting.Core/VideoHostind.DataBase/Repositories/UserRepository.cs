using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using VideoHosting.Services.Repositories;
using VideoHostind.DataBase;
using VideoHosting.Services.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace VideoHostind.DataBase.Repositories
{
    public class UserRepository : IUserRepository
    {
        DataBaseContext context;
        public UserRepository(DataBaseContext context)
        {
            this.context = context;
        }

        public async Task<UserProfile> GetUserProfileById(string Id)
        {
            return await context.UserProfiles.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<Country>> GetCountries()
        {
            return await context.Countries.ToListAsync();
        }
        public async Task<IEnumerable<UserProfile>> GetUserProfiles()
        {
            return await context.UserProfiles.ToListAsync();
        }
        public async Task<IEnumerable<UserProfile>> GetUserBysubName(string str)
        {
            str = str.ToLower();
            return await context.UserProfiles
                .Where(x => x.Name.ToLower().Contains(str) || x.Surname.ToLower().Contains(str) || (x.Name + x.Surname)
                .ToLower()
                .Contains(str))
                .ToListAsync();
        }
    }
}
