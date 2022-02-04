using HahnDroneAPI.Models;
using HahnDroneAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HahnDroneAPI.Controllers
{
    #region ====================================Version 1======================================
    [ApiVersion("1.0")]
    [Route("api/Model")]
    [ApiController]
    public class ModelV1_Controller : ControllerBase
    {
        private readonly IModelService _modelService;
        public ModelV1_Controller(IModelService modelService)
        {
            this._modelService = modelService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryParameters queryParameters)
        {
            var result = await this._modelService.GetModelsAsync(queryParameters);

            return Ok(result);
        }
    }

    #endregion ================================================================================
    
}
