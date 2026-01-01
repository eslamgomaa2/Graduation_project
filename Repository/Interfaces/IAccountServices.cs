using Domins.Dtos.Auth_dto;
using OA.Domain.Auth;
using OA.Service.Exceptions;
using AuthenticationResponse = OA.Domain.Auth.AuthenticationResponse;

namespace Repository.Interfaces
{
    public interface IAccountServices
    {
        Task<AuthenticationResponse> Login(AuthenticationRequest request);
        Task<AuthenticationResponse> AddDoctor(RegisterAsDoctorRequest request);
        Task<AuthenticationResponse> RegisterAsPatientAsync(RegisterAsPatientRequest request);
       
        Task ForgotPassword(ForgotPasswordRequest model);
        Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest model);
    }
}
