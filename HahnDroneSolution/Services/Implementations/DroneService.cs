using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HahnDroneAPI.Configurations;
using HahnDroneAPI.CustomExceptions;
using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Extensions;
using HahnDroneAPI.Models;
using HahnDroneAPI.Models.Enums;
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
    public class DroneService : IDroneService
    {
        private readonly IMapper _mapper;
        private readonly ICustomConfiguration _config;
        private readonly IDroneRepository _droneRepository;
        private readonly IDroneMedicationMasterRepository _droneMedicationMasterRepository;
        
        public DroneService(IMapper mapper, ICustomConfiguration config, IDroneRepository droneRepository, IDroneMedicationMasterRepository droneMedicationMasterRepository)
        {
            this._mapper = mapper;
            this._config = config;
            this._droneRepository = droneRepository;
            this._droneMedicationMasterRepository = droneMedicationMasterRepository;

            this._droneRepository.SeedData(this._config.GetDroneUpperWeightLimit());
        }
        /// <summary>
        /// Fetches the drones, pages and orders the records if the user specifies it should be.
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <returns></returns>

        public async Task<DroneResponse> GetDronesAsync(QueryParameters queryParameters)
        {
            IQueryable<Drone> drones = this._droneRepository.Drones;
            
            var allDrones = drones.OrderByDescending(x => x.DroneID);

            if (drones == null)
            {
                throw new MessageException("The drone table is empty.", HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                drones = drones?.Where(p => p.SerialNumber.ToLower().Contains(queryParameters.SearchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy) && typeof(DroneDto).GetProperty(queryParameters.SortBy) != null)
            {
                drones = drones?.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
            }            

            var filteredDrones = drones.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);

            var pagedDrones = _mapper.Map<IEnumerable<DroneDto>>(await filteredDrones.ToListAsync());
            DroneResponse droneResponse = new DroneResponse();

            droneResponse.Drones = pagedDrones;
            droneResponse.Count = drones.Count();

            return droneResponse;

        }

        public async Task<DroneDto> GetDroneAsync(int droneID)
        {

            Drone drone = await this._droneRepository.GetByID(droneID);

            if (drone == null || drone.DroneID == 0)
            {
                throw new MessageException("The required drone does not exists.", HttpStatusCode.NotFound);
            }

            var result = _mapper.Map<DroneDto>(drone);
            
            return result;          
        }

        public async Task<DroneDto> CreateDroneAsync(DroneDto drone)
        {

            Drone newDrone = _mapper.Map<Drone>(drone);

            if (_droneRepository.Count() > _config.DroneCount())
            {
                throw new MessageException("Total drone limit reached.", HttpStatusCode.NotFound);
            }

            if(!IsValidDroneWeight(drone.Weight))
            {
                throw new MessageException("Drone weight is above specified limit.", HttpStatusCode.BadRequest);
            }

            if (newDrone == null || string.IsNullOrEmpty(newDrone.SerialNumber))
            {
                throw new MessageException("Bad request.", HttpStatusCode.BadRequest);
            }

            if (_droneRepository.FindWhere(x => x.SerialNumber.ToLower().Contains(newDrone.SerialNumber.ToLower())).Any())
            {
                throw new MessageException("Drone already exits.", HttpStatusCode.Ambiguous);
            }

            await _droneRepository.Create(newDrone);
            await _droneRepository.Save();

            var result = _mapper.Map<Drone, DroneDto>(newDrone);

            return result;

        }

        public async Task<DroneDto> UpdateDroneAsync(DroneDto drone, int droneID)
        {
            
            Drone newDrone = _mapper.Map<DroneDto, Drone>(drone);
            var myDrone = await _droneRepository.GetByID(droneID);

            if (newDrone == null || string.IsNullOrEmpty(newDrone.SerialNumber) || newDrone.DroneID != droneID || myDrone == null)
            {
                throw new MessageException("Drone does not exist", HttpStatusCode.NotFound);
            }

            myDrone.BatteryCapacity = newDrone.BatteryCapacity;
            myDrone.ModelID = newDrone.ModelID;
            myDrone.SerialNumber = newDrone.SerialNumber;
            myDrone.State = newDrone.State;
            myDrone.Weight = newDrone.Weight;

            await _droneRepository.Update(myDrone);
            await _droneRepository.Save();

            var result = _mapper.Map<Drone, DroneDto>(myDrone);

            return result;

        }

        public async Task<DroneDto> DeleteDroneAsync(int droneID)
        {
           
            var newDrone = await _droneRepository.GetByID(droneID);
            if (newDrone == null)
            {
                throw new MessageException("Drone does not exist", HttpStatusCode.NotFound);
            }

            if (newDrone.State != StateEnum.IDLE)
            {
                throw new MessageException("This drone cant be deleted because it is not in the IDLE state", HttpStatusCode.Forbidden);
            }

            await _droneRepository.Delete(newDrone);
            await _droneRepository.Save();

            var result = _mapper.Map<Drone, DroneDto>(newDrone);

            return result;

        }

        public async Task<DroneResponse> GetAvaliableDronesAsync()
        {

            IEnumerable<Drone> drones;
            drones = await _droneRepository.FindWhereAsync(x => (x.State == (int)StateEnum.IDLE || x.State == StateEnum.LOADING));

            if (drones == null)
            {
                throw new MessageException("There are no available drones.", HttpStatusCode.NotFound);
            }

            var result = _mapper.Map<IEnumerable<Drone>, IEnumerable<DroneDto>>(drones);
            DroneResponse droneResponse = new DroneResponse();
            droneResponse.Drones = result;
            droneResponse.Count = result.Count();

            return droneResponse;
        }

        public async Task<BatteryLevel> GetDroneBatteryLevelAsync(int droneID)
        {
            Drone drone = await _droneRepository.GetByID(droneID);

            if (drone == null)
            {
                throw new MessageException("drone does not exist.", HttpStatusCode.NotFound);
            }

            BatteryLevel batteryLevel = new BatteryLevel() { DroneID = droneID, Level = drone.BatteryCapacity };
           
            return batteryLevel;
        }

        public async Task<DroneMedicationResponse> GetDroneMedicationsAsync(int droneID)
        {

            List<Medication> medications = new List<Medication>();
            IEnumerable<DroneMedicationMaster> droneMedicationMaster;

            if (droneID == 0)
            {
                throw new MessageException("Bad request", HttpStatusCode.BadRequest);
            }

            var drone = await _droneRepository.GetByID(droneID);
            if (drone.DroneID == 0)
            {
                throw new MessageException("Not Found", HttpStatusCode.NotFound);
            }

            droneMedicationMaster = _droneMedicationMasterRepository.DroneMedicationMasters(droneID);
            foreach (var med in droneMedicationMaster.FirstOrDefault().DroneMedicationDetails)
            {
                medications.Add(med.Medication);
            }
            IEnumerable<MedicationDto> medicationList = _mapper.Map<IEnumerable<Medication>, IEnumerable<MedicationDto>>(medications);

            DroneMedicationResponse droneMedicationResponse = new DroneMedicationResponse();
            droneMedicationResponse.Medications = medicationList;
            droneMedicationResponse.Count = droneMedicationMaster.Count();

            return droneMedicationResponse;

        }

        public bool IsValidDroneWeight(decimal weight) => weight <= _config.GetDroneUpperWeightLimit();
        
    }
}
