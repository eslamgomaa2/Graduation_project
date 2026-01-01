using Domins.Model;
using Repository.Interfaces;

namespace Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbcontext _context;

        public UnitOfWork(ApplicationDbcontext context, IBaseRepository<Patient> patients, IAdminService adminService)
        {
            _context = context;
            this.patients = patients;
            this.adminService = adminService;
        }

        public IBaseRepository<Patient> patients { get; private set; }
        public IAdminService adminService { get; internal set; }

        


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
