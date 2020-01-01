using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VideoHosting.Services.Entities;

namespace VideoHosting.Services.Repositories
{
    public interface IUserRepository
    {       
        Task<UserProfile> GetUserProfileById(string Id);
        Task<IEnumerable<Country>> GetCountries();
        Task<IEnumerable<UserProfile>> GetUserProfiles();
        Task<IEnumerable<UserProfile>> GetUserBysubName(string str);
        
    }
}
