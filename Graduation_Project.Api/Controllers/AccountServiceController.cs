using Domins.Dtos.Auth_dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OA.Domain.Auth;
using Repository.Interfaces;

namespace Graduation_Project.Api.Controllers
{
    [Route("api/[Controller]/")]
    [ApiController]
    public class AccountServiceController : Controller
    {
        private readonly IAccountServices _accountServices;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountServiceController(IAccountServices accountServices, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _accountServices = accountServices;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest model)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _accountServices.Login(model);
            if (!result.IsAuthenticated )
            {
                return BadRequest(result.Message); 
            }
            return Ok(result);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterAsPatientRequest model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var res = await _accountServices.RegisterAsPatientAsync(model);
            if (!res.IsAuthenticated)
                return BadRequest(res.Message);

            return Ok(res);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgotPasswordRequest model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            await _accountServices.ForgotPassword(model);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
           var res= await _accountServices.ResetPassword(model);
            return Ok(res);
        }



        /* [HttpPost("externallogin")]
         public async Task<IActionResult> ExternalLogin(ExternalLoginModel model)
         {
             // Authenticate the user with the external provider
             var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync(model.Providerkey);

             if (externalLoginInfo == null)
             {
                 // Handle error: External login information not found
                 return BadRequest(new { error = "External login information not found." });
             }

             // Sign in the user with the external provider
             var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, isPersistent: false, bypassTwoFactor: true);

             if (!signInResult.Succeeded)
             {
                 var email = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
                 if (email!=null)
                 {
                     var user = await _userManager.FindByEmailAsync(email);
                     if (user == null)
                     {
                         var newUser = new ApplicationUser
                         {
                             UserName = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                             Email = email
                         };

                         var createUserResult = await _userManager.CreateAsync(newUser);

                         if (!createUserResult.Succeeded)
                         {
                             // Handle user creation failure
                             return BadRequest(new { error = "Failed to create user." });
                         }

                         user = newUser;
                     }

                     var addLoginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);

                     if (!addLoginResult.Succeeded)
                     {
                         // Handle adding external login failure
                         return BadRequest(new { error = "Failed to add external login to user." });
                     }

                     await _signInManager.SignInAsync(user, isPersistent: false);
                     return Ok("Account created or authenticated successfully.");
                 }
             }

             // Authentication successful, return success response
             return Ok("Authentication successful!");
         }*/


        /* [HttpPost("External Register")]
         public async Task<IActionResult> ExternalRegister(ExternalRegisterModel model)
         {
             // Check if the user already exists
             var existUser = await _userManager.FindByEmailAsync(model.Email);
             if (existUser != null)
             {
                 return BadRequest("This email already exists.");
             }

             // Create a new ApplicationUser instance
             var newUser = new ApplicationUser
             {
                 FirstName = model.FirstName,
                 LastName = model.LastName,
                 UserName = model.Username,
                 Email = model.Email
             };

             // Create the user in the database
             var createUserResult = await _userManager.CreateAsync(newUser);
             if (!createUserResult.Succeeded)
             {
                 return BadRequest("User creation failed.");
             }

             // Add user to the role
             var addToRoleResult = await _userManager.AddToRoleAsync(newUser, Roles.Patient.ToString());
             if (!addToRoleResult.Succeeded)
             {
                 return BadRequest("Failed to add user to role.");
             }
             var patient=new Patient
             {
                 Name =model.FirstName,

             }

             // Prepare external login info and add it
             var externalLoginInfo = new ExternalLoginInfo(null, model.Provider, model.ProviderKey, model.Provider);
             var addLoginResult = await _userManager.AddLoginAsync(newUser, externalLoginInfo);
             if (!addLoginResult.Succeeded)
             {
                 // Optionally, consider a transaction or compensatory action if critical to business logic
                 return BadRequest("Adding external login failed.");
             }

             // User creation and external login linking succeeded
             return Ok("User created and linked with external provider!");
         }
 */


        /*  public void SetRefreshTokenInCookies(string refreshToken, DateTime expiration)
          {
              var cookieOptions = new CookieOptions
              {
                  Expires = expiration,
                  HttpOnly = true
              };

              Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
          }*/

    }

}
