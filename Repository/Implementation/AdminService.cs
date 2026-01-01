using Domins.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OA.Domain.Auth;
using Repository.Interfaces;

namespace Repository.Implementation
{
    public class AdminService : BaseRepositry<Doctor>, IAdminService
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;


        public AdminService(ApplicationDbcontext dbcontext, UserManager<ApplicationUser> userManager) : base(dbcontext)
        {
            _userManager = userManager;
            _dbcontext = dbcontext;
        }

        public async Task<bool> Deletedoctor(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;

        }



        public async Task<RegisterAsDoctorRequest> UpdateDoctor(RegisterAsDoctorRequest model, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var doctor = await _dbcontext.Doctors.SingleOrDefaultAsync(d => d.UserId == id);

            if (user == null || doctor == null)
            {
                throw new ArgumentException("User or doctor not found");
            }

            await UpdateUserAsync(user, model);
            UpdateDoctor(doctor, model);

            await _dbcontext.SaveChangesAsync();

            return model;
        }

        private async Task UpdateUserAsync(ApplicationUser user, RegisterAsDoctorRequest model)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.phoneNumber;

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            }

            await _userManager.UpdateAsync(user);
        }

        private void UpdateDoctor(Doctor doctor, RegisterAsDoctorRequest model)
        {
            doctor.FName = model.FirstName;
            doctor.LName = model.LastName;
            doctor.UserName = model.UserName;
            doctor.Email = model.Email;
            doctor.PhoneNumber = model.phoneNumber;
            doctor.Password = model.Password;

            _dbcontext.Doctors.Update(doctor);
        }


    }
}

