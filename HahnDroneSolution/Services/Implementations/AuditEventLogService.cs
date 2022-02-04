using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HahnDroneAPI.Configurations;
using HahnDroneAPI.CustomExceptions;
using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Extensions;
using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.Services.Interfaces;
using HahnDroneAPI.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HahnDroneAPI.Db.Repositories.Interfaces;

namespace HahnDroneAPI.Services.Implementations
{
    public class AuditEventLogService : IAuditEventLogService
    {
        private readonly IMapper _mapper;
        private readonly ICustomConfiguration _config;
        private readonly IDroneRepository _droneRepository;
        private readonly IAuditEventLogRepository _auditEventLogRepository;


        public AuditEventLogService(IMapper mapper, ICustomConfiguration config, IAuditEventLogRepository auditEventLogRepository, IDroneRepository droneRepository)
        {
            this._mapper = mapper;
            this._config = config;
            this._droneRepository = droneRepository;
            this._auditEventLogRepository = auditEventLogRepository;
        }

        public async Task<AuditEventLogResponse> GetAuditEventLogsAsync(QueryParameters queryParameters)
        {
            
            IQueryable<AuditEventLog> auditEventLogs;
            auditEventLogs = this._auditEventLogRepository.AuditEventLogs;
            
            if (!string.IsNullOrEmpty(queryParameters.SortBy) && typeof(AuditEventLogDto).GetProperty(queryParameters.SortBy) != null)
            {
                 auditEventLogs = auditEventLogs?.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
            }

            var filteredAuditEventLogs = auditEventLogs.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);
            var result = _mapper.Map<IEnumerable<AuditEventLog>, IEnumerable<AuditEventLogDto>>(await filteredAuditEventLogs.ToListAsync());
            AuditEventLogResponse auditEventLogResponse = new AuditEventLogResponse();
            auditEventLogResponse.AuditEventLogs = result;
            auditEventLogResponse.Count = auditEventLogs.Count();

            return auditEventLogResponse;
            
        }

        public async Task CreateAuditEventLogAsync()
        {

            AuditEventLog newAuditEventLog = new AuditEventLog();
            var drones = await this._droneRepository.Drones.ToListAsync();

            foreach (var drone in drones)
            {
                newAuditEventLog.DroneID = drone.DroneID;
                newAuditEventLog.BatteryCapacity = drone.BatteryCapacity;
                newAuditEventLog.Message = drone.BatteryCapacity < this._config.BatteryLowerLimit() ? "Battery level is too low; Drone can't be put up for loading." : "Battery level is fine.";

                await this._auditEventLogRepository.Create(newAuditEventLog);
                newAuditEventLog = new AuditEventLog();
            }

            await this._auditEventLogRepository.Save();

        }

        public async Task<AuditEventLogDto> DeleteAuditEventLogAsync(int auditEventLogID)
        {
            
            var newAuditEventLog = await this._auditEventLogRepository.GetByID(auditEventLogID);
            if (newAuditEventLog == null)
            {
                throw new MessageException("Not Found", HttpStatusCode.NotFound);
            }

            await this._auditEventLogRepository.Delete(newAuditEventLog);
            await this._auditEventLogRepository.Save();

            var result = _mapper.Map<AuditEventLog, AuditEventLogDto>(newAuditEventLog);

            return result;

        }
    }
}
