using grpcCrud;
using library.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly UserManager<IdentityUser> um;
        private readonly SignInManager<IdentityUser> sim;
        private readonly ILogger<UserInfoController> log;
        public UserInfoController(UserManager<IdentityUser> umm, SignInManager<IdentityUser> simm, ILogger<UserInfoController> loger)
        {
            um = umm;
            sim = simm;
            log = loger;
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegMsg reg)
        {
            var res = await um.CreateAsync(new IdentityUser { UserName = reg.Uname }, reg.Pass);
            if (res.Succeeded)
            {
                await um.AddToRoleAsync(await um.FindByNameAsync(reg.Uname), "Admin");
                log.LogInformation("Succesfully registered");
                return Ok();
            }
            log.LogError("There was mistake trying to register user");
            return BadRequest("Not registered");
        }

        [HttpGet]
        public async void LogOut()
        {
            await sim.SignOutAsync();
        }
        [HttpGet]
        public User Check()
        {
            var u = new User
            {
                Name = User.Identity.Name,
                LoggedIn = User.Identity.IsAuthenticated,
                Claims = User.Claims.ToDictionary(u => u.Type, u => u.Value)
            };
            log.LogInformation(User.Identity.IsAuthenticated.ToString());
            string res = "-------------" + Environment.NewLine;
            u.Claims.ToList().ForEach(cl => res += $"{cl.Key} -- {cl.Value}" + Environment.NewLine);
            res += "---------";
            log.LogInformation(res);
            return u;
        }
        [HttpPost]
        public async Task<IActionResult> Login(RegMsg reg)
        {
            log.LogInformation("log in");
            var kor = await um.FindByNameAsync(reg.Uname);
            if (kor is null)
            {
                log.LogError("User doesnt exist");
                return BadRequest("User null");
            }
            var res = await sim.PasswordSignInAsync(kor, reg.Pass, false, false);
            if (res.Succeeded)
            {
                return Ok();
            }
            else return BadRequest("Error");
        }
    }
}