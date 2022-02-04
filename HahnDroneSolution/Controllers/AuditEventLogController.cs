using Microsoft.AspNetCore.Mvc;
using HahnDroneAPI.Models;
using HahnDroneAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace HahnDroneAPI.Controllers
{
    #region ====================================Version 1======================================
    [ApiVersion("1.0")]
    [Route("api/AuditEventLog")]
    [ApiController]
    public class AuditEventLogV1_Controller : ControllerBase
    {
        private readonly IAuditEventLogService _auditEventLogService;
        public AuditEventLogV1_Controller(IAuditEventLogService auditEventLogService)
        {
            this._auditEventLogService = auditEventLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryParameters queryParameters)
        {
            var result = await this._auditEventLogService.GetAuditEventLogsAsync(queryParameters);

            return Ok(result);
        }

        [HttpPost]
        [Route("CreateAuditEvent")]
        public async Task<IActionResult> Create()
        {
            await this._auditEventLogService.CreateAuditEventLogAsync();

            return Ok();
        }
    }
    #endregion
}
