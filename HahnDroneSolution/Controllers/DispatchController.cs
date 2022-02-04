using Microsoft.AspNetCore.Mvc;
using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HahnDroneAPI.Controllers
{

    #region ====================================Version 1======================================
    [ApiVersion("1.0")]
    [Route("api/Dispatch")]
    [ApiController]
    public class DispatchV1_Controller : ControllerBase
    {

        private readonly IDroneService _droneService;
        private readonly IMedicationService _medicationService;
        private readonly IDroneMedicationService _droneMedicationService;
        private readonly IAuditEventLogService _auditEventLogService;
        public DispatchV1_Controller(IDroneService droneService, IMedicationService medicationService, IDroneMedicationService droneMedicationService, IAuditEventLogService auditEventLogService)
        {
            this._droneService = droneService;
            this._medicationService = medicationService;
            this._droneMedicationService = droneMedicationService;
            this._auditEventLogService = auditEventLogService;
        }

        [HttpPost]
        [Route("CreateDrone")]
        public async Task<IActionResult> Create([FromBody] DroneDto drone)
        {
            var result = await this._droneService.CreateDroneAsync(drone);

            return Created($"api/drone/{ result.DroneID }", result);
        }

        [HttpPost]
        [Route("LoadDroneWithMedication")]
        public async Task<IActionResult> LoadDrone([FromBody] DroneMedicationRequest droneMedicationRequest)
        {
            var result = await this._droneMedicationService.LoadDroneWithMedicationAsync(droneMedicationRequest);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetDroneMedications")]
        public async Task<IActionResult> Get([FromQuery] QueryParameters queryParameters)
        {
            var result = this._droneMedicationService.GetDroneMedicationsAsync(queryParameters);

            return Ok(result);
        }

        [HttpGet]
        [Route("{droneID}/Medications")]
        public async Task<IActionResult> GetDroneMedications(int droneID)
        {
            var result = await this._droneMedicationService.GetDroneMedicationsAsync(droneID);

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
        [Route("GetAuditEventLog")]
        public async Task<IActionResult> GetAuditEventLog([FromQuery] QueryParameters queryParameters)
        {
            var result = await this._auditEventLogService.GetAuditEventLogsAsync(queryParameters);

            return Ok(result);
        }

    }
    #endregion
}
