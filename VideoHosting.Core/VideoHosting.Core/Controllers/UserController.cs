using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net.Mail;
using System;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using VideoHosting.Services.DTO;
using AutoMapper;
using VideoHosting.Core.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using VideoHosting.Core;
using Microsoft.IdentityModel.Tokens;
using VideoHosting.Core.Filters;

namespace MyWebProject.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService UserService;
        private readonly IMapper _mapper;
        public UserController(IUserService service, IMapper mapper)
        {
            _mapper = mapper;
            UserService = service;
        }


        [HttpPost]
        [Route("Exist")]
        public async Task<ActionResult> Exist(LoginUserModel model)
        {
            if (model.Email == null || model.PhoneNumber == null)
            {
                return BadRequest("Invalid data");
            }
            else
            {
                return Ok(UserService.Exist(model.Email, model.PhoneNumber));
            }

        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<ActionResult> Authenticate(UserEnterModel model)
        {
            if (ModelState.IsValid)
            {
                var identity = await UserService.Authenticate(model.Email,model.Password);
                if (identity == null)
                {
                    return BadRequest(new { errorText = "Invalid username or password." });
                }

                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name
                };
                return Ok(response);
            }
            return BadRequest("Invalid login");
        }



        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Bearer");
            return Ok();
        }

        [HttpPost]
        [Route("Registrate")]
        public async Task<ActionResult> Registrate(UserRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                UserLoginDTO userDTO = _mapper.Map<UserLoginDTO>(model);
                UserDTO userLogin = _mapper.Map<UserDTO>(model);

                await UserService.AddUser(userLogin, userDTO);
                return Ok("You was registrated");
            }
            else
            {
                return BadRequest("Invalid data," + ModelState.Values);
            }

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("AddPhoto")]
        public async Task<ActionResult> LoadPhoto()
        {
            var files = HttpContext.Request.Form.Files;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "UsersContent/UsersPhotos");

            if (files[0].FileName.Contains(".JPG") || files[0].FileName.Contains(".png") || files[0].FileName.Contains(".jpg"))
            {
                string storePath = files[0].FileName.Contains(".JPG") || files[0].FileName.Contains(".jpg") ? User.Identity.Name + ".JPG" : User.Identity.Name + ".png";
                string fullPath = Path.Combine(path, storePath);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                using (var stream = System.IO.File.Create(fullPath))
                {
                    await files[0].CopyToAsync(stream);
                }

                UserDTO user = await UserService.GetUserProfileById(User.Identity.Name, User.Identity.Name);
                user.PhotoPath = storePath;
                await UserService.UpdateProfile(user);
            }
            else
            {
                throw new Exception("Image should be .jpg or .png");
            }
            return Ok();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Subscribe/{Id}")]
        public async Task<ActionResult> Subscribe(string Id)
        {
            await UserService.Subscribe(User.Identity.Name, Id);
            return Ok("You subscribed");
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("UpdateUser")]
        public async Task<ActionResult> UpdateUserProfile(UserDTO model)
        {
            if (ModelState.IsValid)
            {
                model.PhotoPath = null;
                model.Id = User.Identity.Name;

                await UserService.UpdateProfile(model);
                return Ok("You changed data");
            }
            return BadRequest("Invalid data");
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("UpdateUserLogin")]
        public async Task<ActionResult> UpdateUserLogin(LoginUserModel model)
        {
            if (ModelState.IsValid)
            {
                UserLoginDTO userLogin = _mapper.Map<UserLoginDTO>(model);
                userLogin.Id = User.Identity.Name;
                await UserService.UpdateLogin(userLogin);
                return Ok("You changed data");
            }
            return BadRequest("Invalid data");
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("ResertPassword")]
        public async Task<ActionResult> ResertPassword(ResertPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                UserLoginDTO userLogin = await UserService.GetUserLoginDTO(User.Identity.Name);
                await UserService.ResertPassword(User.Identity.Name, model.Password, model.OldPassword);
                return Ok("You have new password");
            }
            else
            {
                return BadRequest("Invalid data");
            }
        }

        [HttpPut]
        [Route("RecoverByEmail")]
        public async Task<ActionResult> ResertPasswordByEmail(ResertPasswordModelByEmail model)
        {
            if (ModelState.IsValid)
            {
                await UserService.ResertPassword(model.TempPassword, model.Email, model.Password);

                return Ok("You have new password");
            }
            return BadRequest("Invalid data");
        }

        [HttpPut]
        [Route("DropByEmail")]
        public async Task<ActionResult> DropPassword(string email)
        {
            int pass = await UserService.DropPassword(email);

            string myEmail = "dyshkant2804@gmail.com";
            string password = "Qwerty280400";

            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            var loginInfo = new NetworkCredential(email, password);

            msg.From = new MailAddress(myEmail);
            msg.To.Add(new MailAddress(email));
            msg.Subject = "Recreation password";
            msg.Body = "Here is your temporary password " + pass.ToString();
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;


            smtpClient.Send(msg);
            return Ok("We sent you temporary password");
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [RoleFilter("TsarBatushka")]
        [Route("addAdmin/{Id}")]
        public async Task<ActionResult> AddAdmin(string Id)
        {
            return Ok(await UserService.AddAdmin(Id));
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("profileUser/{Id}")]
        public async Task<ActionResult> GetUser(string Id)
        {
            UserDTO user = await UserService.GetUserProfileById(Id, User.Identity.Name);
            return Ok(user);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("loginUser")]
        public async Task<ActionResult> GetLoginUser()
        {
            UserLoginDTO user = await UserService.GetUserLoginDTO(User.Identity.Name);
            return Ok(_mapper.Map<LoginUserModel>(user));
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("subscribers")]
        public async Task<ActionResult> GetSubscribers()
        {
            IEnumerable<UserDTO> users = await UserService.GetSubscribers(User.Identity.Name, User.Identity.Name);
            return Ok(users);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("subscriptions")]
        public async Task<ActionResult> GetSubscriptions()
        {
            IEnumerable<UserDTO> users = await UserService.GetSubscriptions(User.Identity.Name, User.Identity.Name);
            return Ok(users);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("findbyname/{str}")]
        public async Task<ActionResult> GetUserByName(string str)
        {
            IEnumerable<UserDTO> users = await UserService.GetUserBysubName(str, User.Identity.Name);
            return Ok(users);
        }
    }
}
