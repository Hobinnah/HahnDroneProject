using AutoMapper;
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
    public class DroneMedicationService : IDroneMedicationService
    {
        private readonly IMapper _mapper;
        private readonly ICustomConfiguration _config;
        private readonly IDroneRepository _droneRepository;
        private readonly IMedicationRepository _medicationRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IDroneMedicationMasterRepository _droneMedicationMasterRepository;
        private readonly IDroneMedicationDetailRepository _droneMedicationDetailRepository;

        public DroneMedicationService(IMapper mapper, ICustomConfiguration config, IDroneMedicationMasterRepository droneMedicationMasterRepository, IDroneRepository droneRepository, IDroneMedicationDetailRepository droneMedicationDetailRepository, IMedicationRepository medicationRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            this._mapper = mapper;
            this._config = config;
            this._droneRepository = droneRepository;
            this._medicationRepository = medicationRepository;
            this._unitOfWorkRepository = unitOfWorkRepository;
            this._droneMedicationDetailRepository = droneMedicationDetailRepository;
            this._droneMedicationMasterRepository = droneMedicationMasterRepository;

            this._droneMedicationMasterRepository.SeedData();
        }

        public DroneMedicationResponse GetDroneMedicationsAsync(QueryParameters queryParameters)
        {

            List<Medication> medications = new List<Medication>();
            IQueryable<DroneMedicationMaster> droneMedicationMaster;

            
            if (queryParameters.ID == 0)
            {
                throw new MessageException("Bad request", HttpStatusCode.BadRequest);
            }

            droneMedicationMaster = this._droneMedicationMasterRepository.DroneMedicationMasters(queryParameters.ID).AsQueryable();
            if (!string.IsNullOrEmpty(queryParameters.SortBy) && typeof(DroneMedicationMasterDto).GetProperty(queryParameters.SortBy) != null)
            {
                droneMedicationMaster = droneMedicationMaster?.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
            }


            foreach (var med in droneMedicationMaster.FirstOrDefault().DroneMedicationDetails)
            {
                medications.Add(med.Medication);
            }

            var filtered = medications.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);
            IEnumerable<MedicationDto> medicationList = _mapper.Map<IEnumerable<Medication>, IEnumerable<MedicationDto>>(filtered);
                        
            DroneMedicationResponse droneMedicationResponse = new DroneMedicationResponse();
            droneMedicationResponse.Medications = medicationList;
            droneMedicationResponse.Count = medicationList.Count();

            return droneMedicationResponse;

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

            droneMedicationMaster = this._droneMedicationMasterRepository.DroneWithMedication(droneID);
            if (droneMedicationMaster == null || !droneMedicationMaster.Any())
            {
                throw new MessageException("The selected drone has no medications.", HttpStatusCode.NotFound);
            }

            foreach(var med in droneMedicationMaster.FirstOrDefault().DroneMedicationDetails)
            {
                medications.Add(med.Medication);
            }
            IEnumerable<MedicationDto> medicationList = _mapper.Map<IEnumerable<Medication>, IEnumerable<MedicationDto>>(medications);

            DroneMedicationResponse droneMedicationResponse = new DroneMedicationResponse();
            droneMedicationResponse.Medications = medicationList;
            droneMedicationResponse.Count = medicationList.Count();

            return droneMedicationResponse;

        }

        public async Task<IEnumerable<DroneMedicationMasterDto>> LoadDroneWithMedicationAsync(DroneMedicationRequest droneMedicationRequest)
        {

            if (droneMedicationRequest == null || droneMedicationRequest.DroneID == 0 || !droneMedicationRequest.MedicationIDs.Any())
            {
                throw new MessageException("Bad request", HttpStatusCode.BadRequest);
            }

            //Validate Drone's state.
            bool isAvailable = await IsDroneAvailableForLoading(droneMedicationRequest.DroneID);
            if (!isAvailable)
            {
                throw new MessageException("The selected drone can not be loaded because it is currently not available.", HttpStatusCode.Conflict);
            }

            bool isBatterySufficient = await DroneBatteryCapacity(droneMedicationRequest.DroneID); //Validates the drone's battery capacity
            if (!isBatterySufficient)
            {
                await UpdateDroneState(droneMedicationRequest.DroneID, StateEnum.IDLE);

                throw new MessageException($"The selected drone battrey is below { this._config.BatteryLowerLimit() }.", HttpStatusCode.Conflict);
            }

            var droneChecks = await this._droneMedicationMasterRepository.FirstOrDefaultAsync(x => x.DroneID == droneMedicationRequest.DroneID && (x.Status == StateEnum.LOADING || x.Status == (int)StateEnum.IDLE));
           
            if (droneChecks != null)
            {
                foreach (var medID in droneMedicationRequest.MedicationIDs)
                {
                    var medicationChecks = await _droneMedicationDetailRepository.FindWhereAsync(x => x.DroneMedicationMasterID == droneChecks.DroneMedicationMasterID && x.MedicationID == medID);
                    if (medicationChecks != null || medicationChecks.Any())
                    {
                        throw new MessageException($"Some or all of the selected medications have been previously loaded on this drone.", HttpStatusCode.Conflict);
                    }
                }
            }

            decimal weigth = await DroneMedicationWeight(droneMedicationRequest);
            if (weigth > this._config.GetDroneUpperWeightLimit()) //Verifies drone total weight isnt beyond the maximum allowable weight
            {
                throw new MessageException($"The total weight of medications on the selected drone is now over the { this._config.GetDroneUpperWeightLimit() }. limit", HttpStatusCode.Conflict);
            }

            int ID = 0;
            StateEnum state = weigth == this._config.GetDroneUpperWeightLimit() ? StateEnum.LOADED : StateEnum.LOADING;
            DroneMedicationMaster master = new DroneMedicationMaster() { CapturedBy = droneMedicationRequest.CapturedBy, DroneID = droneMedicationRequest.DroneID, Status = state };


            ID = await this._unitOfWorkRepository.SaveDroneMedications(master, droneMedicationRequest);
            //transaction.Commit(); Microsoft doesnt support transactions when using inMemory database


            var newDroneMedications =  this._droneMedicationMasterRepository.DroneWithMedication(ID);
            var res = _mapper.Map<IEnumerable<DroneMedicationMaster>, IEnumerable<DroneMedicationMasterDto>>(newDroneMedications);

            return res;
        }

        public async Task<bool> DroneBatteryCapacity(int droneID)
        {
            var droneMedications = await this._droneRepository.GetByID(droneID);

            if (droneMedications != null && droneMedications.BatteryCapacity > this._config.BatteryLowerLimit())
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsDroneAvailableForLoading(int droneID)
        {
            var droneMedications = await this._droneRepository.FirstOrDefaultAsync(x => x.DroneID == droneID && (x.State == StateEnum.IDLE || x.State == StateEnum.LOADING));

            if (droneMedications == null || droneMedications.DroneID == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<decimal> DroneMedicationWeight(DroneMedicationRequest droneMedicationRequest)
        {
            decimal totalDroneWeight = 0;
            int droneID = droneMedicationRequest.DroneID;
            var droneMedicationMaster = this._droneMedicationMasterRepository.DroneWithMedication_Loading(droneID);

            if (droneMedicationMaster != null && droneMedicationMaster.Any())
            {
                foreach (var medication in droneMedicationMaster.FirstOrDefault().DroneMedicationDetails)
                {
                    totalDroneWeight += medication.Medication.Weight;
                }
            }

            foreach (var medicationID in droneMedicationRequest.MedicationIDs)
            {
                var med = await this._medicationRepository.GetByID(medicationID);
                totalDroneWeight += med.Weight;
            }

            return totalDroneWeight;
        }

        private async Task UpdateDroneState(int droneID, StateEnum state)
        {
            var droneBelowBatteryLimit = await _droneRepository.GetByID(droneID);
            droneBelowBatteryLimit.State = state;

            await this._droneRepository.Update(droneBelowBatteryLimit);
            await this._droneRepository.Save();
        }
        
    }
}
