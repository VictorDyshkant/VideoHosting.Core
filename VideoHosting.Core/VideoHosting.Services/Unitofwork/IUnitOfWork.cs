using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoHosting.Services.Entities;
using VideoHosting.Services.Repositories;

namespace VideoHosting.Services.Unitofwork
{
    public interface IUnitofwork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IVideoRepository VideoRepository { get; }
        IComentaryRepository ComentaryRepository { get; }
        UserManager<UserLogin> UserManager { get; }
        RoleManager<UserRole> RoleManager { get; }
        Task SaveAsync();
    }
}
