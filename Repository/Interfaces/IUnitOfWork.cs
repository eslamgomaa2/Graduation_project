using Domins.Model;

namespace Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IBaseRepository<Patient> patients { get; }
        IAdminService adminService { get; }
        int Complete();

    }
}
