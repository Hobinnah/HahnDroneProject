using Microsoft.AspNetCore.Mvc;
using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace HahnDroneAPI.Controllers
{

    #region ====================================Version 1======================================
    [ApiVersion("1.0")]
    [Route("api/Drone")]
    [ApiController]
    public class DroneV1_Controller : ControllerBase
    {
        private readonly IDroneService _droneService;
        private readonly IDroneMedicationService _droneMedicationService;
        public DroneV1_Controller(IDroneService droneService, IDroneMedicationService droneMedicationService)
        {
            this._droneService = droneService;
            this._droneMedicationService = droneMedicationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryParameters queryParameters)
        {
            var result = await this._droneService.GetDronesAsync(queryParameters);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetDrone/{droneID}")]
        public async Task<IActionResult> Get([FromRoute] int droneID)
        {
            var result = await this._droneService.GetDroneAsync(droneID);

            return Ok(result);
        }

        [HttpPost]
        [Route("LoadDroneWithMedication")]
        public async Task<IActionResult> LoadDrone([FromBody] DroneMedicationRequest droneMedicationRequest)
        {
            var result = await this._droneMedicationService.LoadDroneWithMedicationAsync(droneMedicationRequest);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAvaliableDronesForLoading")]
        public async Task<IActionResult> GetAvaliableDrones()
        {
            var result = await this._droneService.GetAvaliableDronesAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetDroneBatteryLevel/{droneID}")]
        public async Task<IActionResult> GetDroneBatteryLevel(int droneID)
        {
            var result = await this._droneService.GetDroneBatteryLevelAsync(droneID);

            return Ok(result);
        }

        [HttpGet]
        [Route("{droneID}/Medications")]
        public async Task<IActionResult> GetDroneMedications(int droneID)
        {
            var result = await this._droneMedicationService.GetDroneMedicationsAsync(droneID);

            return Ok(result);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] DroneDto drone)
        {
            var result = await this._droneService.CreateDroneAsync(drone);

            return Created($"api/drone/{ result.DroneID }", result);
        }

        [HttpPut]
        [Route("Update/{droneID}")]
        public async Task<ActionResult> Put(int droneID, [FromBody] DroneDto drone)
        {
            var result = await this._droneService.UpdateDroneAsync(drone, droneID);

            return Ok(result);

        }

        [HttpDelete]
        [Route("Delete/{droneID}")]
        public async Task<ActionResult> Delete(int droneID)
        {
            var result = await this._droneService.DeleteDroneAsync(droneID);

            return Ok(result);

        }
    }

    #endregion
}

