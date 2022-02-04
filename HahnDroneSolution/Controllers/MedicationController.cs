using Microsoft.AspNetCore.Mvc;
using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace HahnDroneAPI.Controllers
{

    #region ====================================Version 1======================================
    [ApiVersion("1.0")]
    [Route("api/Medication")]
    [ApiController]
    public class MedicationV1_Controller : ControllerBase
    {
        private readonly IMedicationService _medicationService;
        public MedicationV1_Controller(IMedicationService medicationService)
        {
            this._medicationService = medicationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryParameters queryParameters)
        {
            var result = await this._medicationService.GetMedicationsAsync(queryParameters);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetMedication/{medicationID}")]
        public async Task<IActionResult> GetMedication(int medicationID)
        {
            var result = await this._medicationService.GetMedicationAsync(medicationID);

            return Ok(result);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] MedicationDto medication)
        {
            var result = await this._medicationService.CreateMedicationAsync(medication);

            return Created($"api/medication/{ result.MedicationID }", result);
        }

        [HttpPut]
        [Route("{medicationID}")]
        public async Task<ActionResult> Put(int medicationID, [FromBody] MedicationDto medication)
        {
            var result = await this._medicationService.UpdateMedicationAsync(medication, medicationID);

            return Ok(result);

        }

        [HttpDelete]
        [Route("{medicationID}")]
        public async Task<ActionResult> Delete(int medicationID)
        {
            var result = await this._medicationService.DeleteMedicationAsync(medicationID);

            return Ok(result);

        }
    }

    #endregion
}
