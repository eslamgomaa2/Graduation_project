using Domins.Model;
using Firebase.Database;
using Firebase.Database.Query;
using System.Reactive.Linq;

public class FirebaseSensorService
{
    private readonly FirebaseClient _firebaseClient;
    private IDisposable _subscription;

    public event EventHandler<SensorData> OnSensorDataReceived;

    public FirebaseSensorService(string firebaseUrl, string authSecret)
    {
        _firebaseClient = new FirebaseClient(firebaseUrl,
            new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(authSecret)
            });
    }

    public async Task<SensorData> GetLatestDataAndListenForUpdatesAsync()
    {
        // First, get the latest data
        var latestData = await GetLatestSensorDataAsync();

        // If there's latest data, invoke the event
        if (latestData != null)
        {
            OnSensorDataReceived?.Invoke(this, latestData);
        }

        // Then, start listening for updates
        StartListeningForSensorData();

        return latestData;
    }

    private async Task<SensorData> GetLatestSensorDataAsync()
    {
        var sensorData = await _firebaseClient
            .Child("SensorData")
            .OrderByKey()
            .LimitToLast(1)
            .OnceAsync<SensorData>();

        return sensorData.FirstOrDefault()?.Object;
    }

    private void StartListeningForSensorData()
    {
        _subscription = _firebaseClient
            .Child("SensorData")
            .AsObservable<SensorData>()
            .Subscribe(sensorData =>
            {
                if (sensorData.Object != null)
                {
                    OnSensorDataReceived?.Invoke(this, sensorData.Object);
                }
            }, ex =>
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            });
    }

    public void StopListening()
    {
        _subscription?.Dispose();
    }
}