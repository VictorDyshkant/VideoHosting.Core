using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VideoHostind.DataBase;
using VideoHosting.Services.Entities;
using VideoHosting.Services.Unitofwork;

namespace VideoHosting.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        DataBaseContext _context;
        UserManager<UserLogin> _userManager;
        RoleManager<UserRole> _roleManager;
       
        public ValuesController(UserManager<UserLogin> userManager, RoleManager<UserRole> roleManager, DataBaseContext dataBaseContext)
        {
            _userManager = userManager;
            _context = dataBaseContext;
            _roleManager = roleManager;

        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
