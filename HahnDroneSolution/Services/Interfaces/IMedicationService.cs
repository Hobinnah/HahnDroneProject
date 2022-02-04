using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.ViewModel;
using System.Threading.Tasks;

namespace HahnDroneAPI.Services.Interfaces
{
    public interface IMedicationService
    {
        Task<MedicationResponse> GetMedicationsAsync(QueryParameters queryParameters);
        Task<MedicationDto> GetMedicationAsync(int medicationID);
        Task<MedicationDto> CreateMedicationAsync(MedicationDto medication);
        Task<MedicationDto> UpdateMedicationAsync(MedicationDto medication, int medicationID);
        Task<MedicationDto> DeleteMedicationAsync(int medicationID);
    }
}
