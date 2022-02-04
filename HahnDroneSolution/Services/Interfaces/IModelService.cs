using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.ViewModel;
using System.Threading.Tasks;

namespace HahnDroneAPI.Services.Interfaces
{
    public interface IModelService
    {
        Task<ModelResponse> GetModelsAsync(QueryParameters queryParameters);
       
    }
}
