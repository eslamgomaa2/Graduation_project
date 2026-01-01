using Domins.Dtos.Dto;
using Domins.Model;

namespace Repository.Interfaces
{
    public interface IAlarmServices:IBaseRepository<Alarm>
    {
        public Task<Alarm> Create(Alarm model);
      
        public Task<IEnumerable<Alarmdto>> GetPtientAlarms();
        public Task<Alarmdto> Update(Alarmdto model,int id);
        public Task<bool> Delete(int id);
       
        
    }
}