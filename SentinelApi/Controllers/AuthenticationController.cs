using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SentinelApi.Custom;
using SentinelApi.Data;
using SentinelApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SentinelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole>  roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole>  roleManager , IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] Registermodel model)
        {
            var userExist = await userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError,new Response { Status="Error",Message="User Already Exist"});
            ApplicationUser user = new ApplicationUser()
            {
                RollNo = model.RollNo,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                SchoolCode = model.SchoolCode,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                
                var error = result.Errors.ToList(); //convert to list

                foreach (var err in error) //iterate through individual error
                {
                    // this.ModelState.AddModelError("Password", err.Description);
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Creation Faild " + err.Description });
                }
                
            }
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            if(await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "User Created Successfuly" });

        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Loginmodel loginmodel)
        {
            //var user1 = loginmodel.RollNo;
           // 
            var user = await userManager.FindByNameAsync(loginmodel.UserName);
            
            if (user !=null&& await userManager.CheckPasswordAsync(user,loginmodel.Password))
            {
                var RollNo = userManager.Users.Where(p => p.UserName == loginmodel.UserName).Select(p => p.RollNo).FirstOrDefault();
               // if (RollNo)
                    var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };
                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var toekn = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(toekn)
                });

            }
            return Unauthorized();

        }

    }
}
