using Domins.Model;

namespace Repository.Interfaces
{
    public interface ISensorDataServices
    {
        public Task<SensorData> GetSensorDataAsync();
    }
}
