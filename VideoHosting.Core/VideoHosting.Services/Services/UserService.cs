using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VideoHosting.Services.DTO;
using VideoHosting.Services.Entities;
using VideoHosting.Services.Unitofwork;

namespace VideoHosting.Services.Services
{
    public class UserService : IUserService
    {
        protected IUnitofwork _unit;
        protected readonly IMapper _mapper;
       
        public UserService(IUnitofwork unit,IMapper mapper) 
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<ClaimsIdentity> Authenticate(string email, string password)
        {
            UserLogin userLogin = await _unit.UserManager.FindByEmailAsync(email);
            if (await _unit.UserManager.CheckPasswordAsync(userLogin, password) == false)
            {
                throw new InvalidDataException("Incorect password");
            }

            var claims = new List<Claim>();
            var roles = await _unit.UserManager.GetRolesAsync(userLogin);

            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, userLogin.Id));
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            }               
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }

        public string Exist(string email, string phonenumber)
        {
            UserLogin user = _unit.UserManager.Users.FirstOrDefault(x => x.Email == email || x.PhoneNumber == phonenumber);
            if(user != null)
            {
                return user.Id;
            }
            else
            {
                return null;
            }
        }
        public async Task AddUser(UserDTO longUserDTO, UserLoginDTO userLoginDTO)
        {
            UserLogin user = new UserLogin()
            {
                Email = userLoginDTO.Email,
                PhoneNumber = userLoginDTO.PhoneNumber,
                UserName = longUserDTO.Name + " " + longUserDTO.Surname
            };
            UserProfile userProfile = _mapper.Map<UserProfile>(longUserDTO);
            IEnumerable<Country> countries = await _unit.UserRepository.GetCountries();
            
            user.UserProfile = userProfile;
            await _unit.UserManager.CreateAsync( user, userLoginDTO.Password);
            user.UserName = user.Id;
            await _unit.UserManager.UpdateAsync(user);
            userProfile.Country = countries.FirstOrDefault(x => x.CountryName == longUserDTO.Country);
            await _unit.UserManager.AddToRoleAsync(user, "User");
            await _unit.SaveAsync();
        }
 
        public async Task<UserDTO> GetUserProfileById(string id,string userId)
        {
            UserProfile user = await _unit.UserRepository.GetUserProfileById(id);
            UserProfile userSub = await _unit.UserRepository.GetUserProfileById(userId);

            UserDTO userDTO = _mapper.Map<UserDTO>(user);
            userDTO.DoSubscribed = user.Subscribers.FirstOrDefault(x=>x.Subscripter == userSub) == null ? false : true;

            return  userDTO;
        }
        public async Task<UserLoginDTO> GetUserLoginDTO(string id)
        {
            UserLogin userLogin = await _unit.UserManager.FindByIdAsync(id);

            UserLoginDTO userLoginDTO = _mapper.Map<UserLoginDTO>(userLogin);
            return userLoginDTO;
        }

        public async Task<IEnumerable<UserDTO>> GetSubscribers(string id,string userId)
        {
            List<UserDTO> list = new List<UserDTO>();

            UserProfile user = await _unit.UserRepository.GetUserProfileById(id);
            IEnumerable<UserUser> userProfiles = user.Subscribers;
            foreach (var u in userProfiles)
            {
                list.Add(await GetUserProfileById(u.SubscriberId, userId));
            }
            return list;
        }
        public async Task<IEnumerable<UserDTO>> GetSubscriptions(string id, string userId)
        {
            List<UserDTO> list = new List<UserDTO>();
            UserProfile user = await _unit.UserRepository.GetUserProfileById(id);
            IEnumerable<UserUser> userProfiles = user.Subscriptions;
            foreach (var u in userProfiles)
            {
                list.Add(await GetUserProfileById(u.SubscripterId, userId));
            }
            return list;
        }
        public async Task<IEnumerable<UserDTO>> Get(string userId)
        {
            IEnumerable<UserProfile> user = await _unit.UserRepository.GetUserProfiles();
            List<UserDTO> list = new List<UserDTO>();

            foreach (var u in user)
            {
                list.Add(await GetUserProfileById(u.Id, userId));
            }
            return list;
        }
        public async Task<IEnumerable<UserDTO>> GetUserBysubName(string str,string userId)
        {
            IEnumerable<UserProfile> users = await _unit.UserRepository.GetUserBysubName(str);
            List<UserDTO> list = new List<UserDTO>();

            foreach (var u in users)
            {
                list.Add(await GetUserProfileById(u.Id, userId));
            }
            return list;
        }

        public async Task Subscribe(string subscriberId, string subscriptionId)
        {
            UserProfile subscriber = await _unit.UserRepository.GetUserProfileById(subscriberId);
            UserProfile subscription = await _unit.UserRepository.GetUserProfileById(subscriptionId);

            if (subscriber.Subscribers.FirstOrDefault(x => x.Subscripter == subscription) == null)
            {
                subscriber.Subscribe(subscription);
            }
            else
            { 
                subscriber.Unsubscribe(subscription);
            }
           
            await _unit.SaveAsync();
        }
        public async Task UpdateLogin(UserLoginDTO userLoginDTO)
        {
            UserLogin userLogin = await _unit.UserManager.FindByIdAsync(userLoginDTO.Id);

            userLogin.Email = userLoginDTO.Email != null ? userLoginDTO.Email : userLogin.Email;
            userLogin.PhoneNumber = userLoginDTO.PhoneNumber != null ? userLoginDTO.PhoneNumber : userLogin.PhoneNumber;

            await _unit.SaveAsync();
        }
        public async Task UpdateProfile(UserDTO longUserDTO)
        {
            IEnumerable<Country> countries = await _unit.UserRepository.GetCountries();
            UserProfile userProfile = await _unit.UserRepository.GetUserProfileById(longUserDTO.Id);

            userProfile.Name = longUserDTO.Name != null ? longUserDTO.Name : userProfile.Name;
            userProfile.Surname = longUserDTO.Surname != null ? longUserDTO.Surname : userProfile.Surname;

            userProfile.Birthday = longUserDTO.Birthday != null ? longUserDTO.Birthday : userProfile.Birthday;
            userProfile.Country = longUserDTO.Country != null ? countries.FirstOrDefault(x => x.CountryName == longUserDTO.Country) : userProfile.Country;
            userProfile.PhotoPath = longUserDTO.PhotoPath != null ? longUserDTO.PhotoPath : userProfile.PhotoPath;
            userProfile.Sex = longUserDTO.Sex;

            await _unit.SaveAsync();
        }
        
        public async Task<bool> AddAdmin(string Id)
        {
            UserLogin user = await _unit.UserManager.FindByIdAsync(Id);
            
            if (await _unit.UserManager.IsInRoleAsync(user, "Admin") == false)
            {
                await _unit.UserManager.AddToRoleAsync(user, "Admin");
                return true;
            }
            else
            {
                await _unit.UserManager.RemoveFromRoleAsync(user, "Admin");
                return false;
            }

        }
        public async Task<int> DropPassword(string email)
        {
            UserLogin user = await _unit.UserManager.FindByEmailAsync(email);
            if (user == null)
                throw new InvalidDataException("Invalid email");

            int password = new Random().Next(1000, 1000000);

            user.UserProfile.TempPassword = password;
            await _unit.SaveAsync();
            return password;
        }
        public async Task ResertPassword(int tempPassword, string email, string newPassword)
        {
            UserLogin user = await _unit.UserManager.FindByEmailAsync(email);
            if(user.UserProfile.TempPassword == tempPassword && user.UserProfile.TempPassword !=0)
            {
                user.UserProfile.TempPassword = 0;
                await _unit.UserManager.RemovePasswordAsync(user);
                await _unit.UserManager.AddPasswordAsync(user, newPassword);
                await _unit.SaveAsync();
            }
            throw new InvalidDataException("Invalid password or login");
            
        }
        public async Task ResertPassword(string userId, string newpassword,string oldpassword)
        {
            UserLogin user = await _unit.UserManager.FindByIdAsync(userId);

            await _unit.UserManager.ChangePasswordAsync(user,oldpassword,newpassword);
        }
    }
}
