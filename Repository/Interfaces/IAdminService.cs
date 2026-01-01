using Domins.Dtos.Dto;
using Domins.Model;
using OA.Domain.Auth;

namespace Repository.Interfaces
{
    public interface IAdminService :IBaseRepository<Doctor> 
    {
        public Task<RegisterAsDoctorRequest> UpdateDoctor(RegisterAsDoctorRequest model,string id);
        public Task<bool> Deletedoctor(string id);

    }
}
