using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.DTO
{
    public interface IUserService
    {
        Task<ClaimsIdentity> Authenticate(string email, string password);
        Task AddUser(UserDTO longUserDTO, UserLoginDTO userLoginDTO);
        Task UpdateProfile(UserDTO longUserDTO);
        Task UpdateLogin(UserLoginDTO userLoginDTO);
        string Exist(string email, string phonenumber);
        Task Subscribe(string subscriberId, string subscriptionId);
        Task<bool> AddAdmin(string Id);
        Task<int> DropPassword(string Email);
        Task ResertPassword(int tempPassword,string email,string newPassword);
        Task ResertPassword(string userId, string newpassword, string oldpassword);

        Task<UserDTO> GetUserProfileById(string id, string userId);
        Task<UserLoginDTO> GetUserLoginDTO(string id);

        Task<IEnumerable<UserDTO>> GetSubscribers(string id, string userId);
        Task<IEnumerable<UserDTO>> GetSubscriptions(string id, string userId);
        Task<IEnumerable<UserDTO>> Get(string userId);
        Task<IEnumerable<UserDTO>> GetUserBysubName(string str, string userId);
    }
}
