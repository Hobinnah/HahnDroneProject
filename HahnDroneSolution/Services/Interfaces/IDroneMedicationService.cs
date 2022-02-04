using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HahnDroneAPI.Services.Interfaces
{
    public interface IDroneMedicationService
    {
        DroneMedicationResponse GetDroneMedicationsAsync(QueryParameters queryParameters);
        Task<DroneMedicationResponse> GetDroneMedicationsAsync(int droneID);
        Task<IEnumerable<DroneMedicationMasterDto>> LoadDroneWithMedicationAsync(DroneMedicationRequest droneMedicationRequest);
        Task<bool> DroneBatteryCapacity(int droneID);
        Task<bool> IsDroneAvailableForLoading(int droneID);
        Task<decimal> DroneMedicationWeight(DroneMedicationRequest droneMedicationRequest);
    }
}
