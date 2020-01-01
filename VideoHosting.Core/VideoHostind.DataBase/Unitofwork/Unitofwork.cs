using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using VideoHostind.DataBase;
using VideoHostind.DataBase.Repositories;
using VideoHosting.Services.Entities;
using VideoHosting.Services.Repositories;
using VideoHosting.Services.Unitofwork;

namespace DVideoHostind.DataBase.Unitofwork
{
    public class Unitofwork : IUnitofwork
    {
        DataBaseContext _context;
        UserManager<UserLogin> _userManager;
        RoleManager<UserRole> _roleManager;
        IUserRepository _userRepository;
        IVideoRepository _videoRepository;
        IComentaryRepository _comentaryRepository;

        bool disposedValue = false;

        public Unitofwork(UserManager<UserLogin> userManager,RoleManager<UserRole> roleManager, DataBaseContext dataBaseContext)
        {
            _userManager = userManager;
            _context = dataBaseContext;
            _roleManager = roleManager;
            
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }
        public IVideoRepository VideoRepository 
        {
            get
            {
                if (_videoRepository == null)
                    _videoRepository = new VideoRepository(_context);
                return _videoRepository;
            }
        }
        public IComentaryRepository ComentaryRepository 
        {
            get
            {
                if (_comentaryRepository == null)
                    _comentaryRepository = new ComentaryRepository(_context);
                return _comentaryRepository;
            }
        }
        public UserManager<UserLogin> UserManager
        {
            get
            {
                return _userManager;
            }
        }
        public RoleManager<UserRole> RoleManager
        {
            get
            {
                return _roleManager;
            }
        }

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
        }   
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
    }
}
