using Domins.Dtos.Dto;
using Microsoft.AspNetCore.Identity;

using OA.Domain.Auth;
using OA.Service.Contract;
using Repository.Interfaces;

namespace Repository.Implementation
{
    public class WarningService : IWarningService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _mailService;

        public WarningService(UserManager<ApplicationUser> userManager, IEmailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }

        public async Task SendWarning(Warningdto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new Exception("Invalid email");
            }

            var email = new MailRequestdto
            {
                Body = dto.Message,
                ToEmail = dto.Email,
                Subject = "Warning"
            };

            await _mailService.SendEmailAsync(email.ToEmail, email.Subject, email.Body);
        }
    }
}
