using AutoMapper;
using Contracts;
using DataAccessContracts.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Models.AccountApiModels;

namespace WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseTurnoverController {

        private readonly UserManager<TurnoverUser> userManager;
        private readonly RoleManager<TurnoverRole> roleManager;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly AuthSettings authSettings;

        public AccountController(UserManager<TurnoverUser> userManager, RoleManager<TurnoverRole> roleManager, IMapper mapper, IEmailService emailService, IOptions<AuthSettings> authSettings) {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.emailService = emailService;
            this.authSettings = authSettings.Value;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model) {
            TurnoverUser user = await userManager.FindByNameAsync(model.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password)) {
                string token = await CreateToken(user);
                return Ok(new { token });

            } else
                return BadRequest(new { message = "Username or passowrd is incorrect." });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel model) {
            if (model.Password != model.ConfirmPassword)
                return BadRequest(new { message = "Password and Confirm Password do not match." });

            TurnoverUser user = mapper.Map<TurnoverUser>(model);

            IdentityResult res = await userManager.CreateAsync(user, model.Password);

            if (res.Succeeded) {
                foreach (string role in model.Roles)
                    await userManager.AddToRoleAsync(user, role);

                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                await emailService.SendEmailAsync(model.Email, "Confirm your email", $"Confirmation code: <b>{code}</b>");
            }

            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmEmail(string code) {
            var user = await userManager.FindByIdAsync(GetUserIdString());
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost]
        [Route("UpdateUserRoles")]
        [Authorize(Roles = RolesHelper.AdminRole)]
        public async Task<IActionResult> UpdateUserRoles(UpdateUserRolesModel model) {
            TurnoverUser user = await userManager.FindByIdAsync(model.UserId.ToString());

            await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user));
            foreach (var item in model.Roles) {
                await userManager.AddToRoleAsync(user, item);
            }

            return Ok();
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model) {
            if (model.NewPassword != model.ConfirmNewPassword)
                return BadRequest(new { message = "Password and Confirm Password do not match." });

            TurnoverUser user = await userManager.FindByIdAsync(GetUserIdString());

            IdentityResult res = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            return Ok(res);
        }

        private async Task<string> CreateToken(TurnoverUser user) {
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]{
                        new Claim("UserId", user.Id.ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(authSettings.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key)), SecurityAlgorithms.HmacSha256Signature)
            };

            IList<string> userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles) {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, userRole));

                TurnoverRole roleFound = await roleManager.FindByNameAsync(userRole);
                if (roleFound != null) {
                    IList<Claim> roleClaims = await roleManager.GetClaimsAsync(roleFound);
                    foreach (Claim roleClaim in roleClaims)
                        tokenDescriptor.Subject.AddClaim(roleClaim);
                }
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);
            return token;
        }

    }
}
