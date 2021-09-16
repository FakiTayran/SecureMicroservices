using AspNetCore.Identity.Dapper.Models;
using IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthController(SignInManager<ApplicationUser> _signInManager)
        {
            signInManager = _signInManager;
        }
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginDto loginDto)
        //{
        //    var result = await signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password,false,false);

        //    if (result.Succeeded)
        //    {
                
        //    }
        //    else if (result.IsLockedOut)
        //    { 
            
        //    }


        //}
    }
}
