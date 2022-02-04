using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HahnDroneAPI.Services.Interfaces
{
    public interface IDroneService
    {
        Task<DroneResponse> GetDronesAsync(QueryParameters queryParameters);
        Task<DroneDto> GetDroneAsync(int droneID);
        Task<DroneDto> CreateDroneAsync(DroneDto drone);
        Task<DroneDto> UpdateDroneAsync(DroneDto drone, int droneID);
        Task<DroneDto> DeleteDroneAsync(int droneID);
        Task<DroneResponse> GetAvaliableDronesAsync();
        Task<BatteryLevel> GetDroneBatteryLevelAsync(int droneID);
        Task<DroneMedicationResponse> GetDroneMedicationsAsync(int droneID);
        bool IsValidDroneWeight(decimal weight);
    }
}
