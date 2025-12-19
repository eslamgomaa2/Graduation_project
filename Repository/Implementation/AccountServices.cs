using Domins.Dtos.Auth_dto;
using Domins.Dtos.Dto;
using Domins.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OA.Domain.Auth;
using OA.Domain.Enum;
using OA.Domain.Settings;
using OA.Service.Contract;
using Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Repository.Implementation
{
    public class AccountServices : IAccountServices

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly JWT _jwt;
        private readonly MailSettings? _mailsetting;
        private readonly ApplicationDbcontext _dbcontext;

        public AccountServices(IOptions<JWT> jwt, UserManager<ApplicationUser> userManager, IEmailService emailService, ApplicationDbcontext dbcontext)
        {
            _jwt = jwt.Value;
            _userManager = userManager;
            _emailService = emailService;
            _dbcontext = dbcontext;
        }



        public async Task<AuthenticationResponse> Login(AuthenticationRequest request)
        {

            AuthenticationResponse response = new AuthenticationResponse();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                response.Message = $"PassWord or Email is Incorrect";

                return response;
            }

            
            /*if (request.Role != await _userManager.GetRolesAsync(user))
            {
                response.IsAuthorized = false;
                response.Message = $"UnAuthorized Role";
                return response;
            }*/


            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user);
            response.Roles = rolesList.ToList();
            response.IsAuthenticated = true;
            
            /*if (useremail.RefreshTokens.Any(p => p.IsActive))
            {
                var ActivRefreshToken = useremail.RefreshTokens.SingleOrDefault(p => p.IsActive);
                response.RefreshToken = ActivRefreshToken.Token;

            }
            else
            {
                var refreshtoken = GenerateRefreshToken();
                response.RefreshToken = refreshtoken.Token;
                useremail.RefreshTokens.Add(refreshtoken);
                await _userManager.UpdateAsync(useremail);

            }*/
            return response;



        }



        public async Task<AuthenticationResponse> RegisterAsPatientAsync(RegisterAsPatientRequest model)
        {
            AuthenticationResponse response = new AuthenticationResponse();

            var user = await _userManager.FindByEmailAsync(model.Email);
            var username = await _userManager.FindByNameAsync(model.UserName);

            if (username != null || user != null)
            {

                response.Message = $"Username '{model.UserName}'or  Email  {model.Email} is already Exist.";

                return response;
            }
            var newuser = new ApplicationUser
            {

                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.phoneNumber

            };


            var result = await _userManager.CreateAsync(newuser, model.Password!);
            if (!result.Succeeded)
            {

                return new AuthenticationResponse { Message = $"{result.Errors.ToList()[0].Description}" };
            }


            await _userManager.AddToRoleAsync(newuser, Roles.Patient.ToString());
            var Patient = new Patient
            {
                FName = model.FirstName,
                LName = model.LastName,
                patientEmail = model.Email,
                UserName = model.UserName,
                DoctorId = model.DoctorId,
                UserId = newuser.Id
            };

            await _dbcontext.Patients.AddAsync(Patient);
            _dbcontext.SaveChanges();

            var jwtSecurityToken = await GenerateJWToken(newuser);
            response.Email = newuser.Email;
            response.UserName = newuser.UserName;
            response.IsAuthenticated = true;
            response.Id = newuser.Id;
            response.Roles = new List<string>{Roles.Patient.ToString()};



            return response;

        }
        public async Task<AuthenticationResponse> AddDoctor(RegisterAsDoctorRequest request)
        {
            AuthenticationResponse response = new AuthenticationResponse();

            var username = await _userManager.FindByNameAsync(request.UserName);
            var useremail = await _userManager.FindByEmailAsync(request.Email);

            if (username != null || useremail != null)
            {
                return new AuthenticationResponse { Message = $"Username '{request.UserName}'or{request.Email} is already taken." };

            }
            var newuser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.phoneNumber

            };


            var result = await _userManager.CreateAsync(newuser, request.Password!);
            if (!result.Succeeded)
            {

                return new AuthenticationResponse { Message = $"{result.Errors.ToList()[0].Description}" };
            }

            await _userManager.AddToRoleAsync(newuser, Roles.Doctor.ToString());
            var doctor = new Doctor
            {
                UserName = request.UserName,
                UserId = newuser.Id,
                FName = request.FirstName,
                LName = request.LastName,
                Email = request.Email,
                Password = request.Password,
              
            };
            await _dbcontext.Doctors.AddAsync(doctor);

            _dbcontext.SaveChanges();

            var jwtSecurityToken = await GenerateJWToken(newuser);
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = newuser.Email;
            response.UserName = newuser.UserName;
            response.IsAuthenticated = true;
            response.Id = newuser.Id;
            response.Roles = new List<string>{Roles.Doctor.ToString() };

            return response;



        }




        public async Task ForgotPassword(ForgotPasswordRequest model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);


            if (user == null)
            {
                throw new Exception("Invalid email");
            }


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var email = new MailRequestdto
            {
                Body = $"Your reset token is: {token}",
                ToEmail = model.Email,
                Subject = "Reset Password"
            };


            await _emailService.SendEmailAsync(email.ToEmail, email.Subject, email.Body);
        }






        public async Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest model)
        {
            var response = new AuthenticationResponse();
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) {
                response.Message = $"No Accounts Registered with {model.Email}.";
                return response;
            }
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                response.Message = $"Password Resetted.";
                return response;
            }
            else
            {

                response.Message = $"Error occured while reseting the password.";
                return response;
            };
        }




        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(role => new Claim("roles", role));

            var claims = new[]
            {

        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),

            }
            .Union(userClaims)
            .Union(roleClaims);


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            using var randomumbergenerator = RandomNumberGenerator.Create();
            var randomnumber = new byte[40];
            randomumbergenerator.GetBytes(randomnumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomnumber),
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                CreatedOn = DateTime.UtcNow,

            };
        }

        private async Task<AuthenticationResponse> RefreshTokenasync(string refreshToken)
        {
            AuthenticationResponse Response = new AuthenticationResponse();
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
            {
                Response.Message = "in not valid";
                return Response;
            }
            var tokenuser = user.RefreshTokens.Single(t => t.Token == refreshToken);
            if (!tokenuser.IsActive)
            {


                Response.Message = "is not valid";
                return Response;
            }

            tokenuser.RevokedOn = DateTime.UtcNow;
            var newRefreshtoken = GenerateRefreshToken();
            Response.IsAuthenticated = true;
            Response.RefreshToken = newRefreshtoken.Token;
            var roles = await _userManager.GetRolesAsync(user);
            Response.Roles = roles.ToList();
            Response.Email = user.Email;
            Response.RefreshTokenExpiration = newRefreshtoken.ExpiresOn;
            var jwttoken = await GenerateJWToken(user);
            Response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwttoken);


            return Response;

        }
        private async Task<bool> revokedtoken(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(p => p.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                return false;
            }
            var refreshToken = user.RefreshTokens.Single(p => p.Token == token);
            if (!refreshToken.IsActive)
            {
                return false;
            }
            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;


        }


    }


}   



