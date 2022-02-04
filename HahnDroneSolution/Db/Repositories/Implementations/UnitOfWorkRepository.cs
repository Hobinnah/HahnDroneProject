using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Db.Repositories.Interfaces;
using HahnDroneAPI.Models;
using HahnDroneAPI.Models.Enums;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Implementations
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly IDroneMedicationMasterRepository _droneMedicationMasterRepository;
        private readonly IDroneMedicationDetailRepository _droneMedicationDetailRepository;
        private readonly IDroneRepository _droneRepository;

        public UnitOfWorkRepository(IDroneRepository droneRepository, IDroneMedicationMasterRepository droneMedicationMasterRepository, IDroneMedicationDetailRepository droneMedicationDetailRepository)
        {
            _droneRepository = droneRepository;
            _droneMedicationDetailRepository = droneMedicationDetailRepository;
            _droneMedicationMasterRepository = droneMedicationMasterRepository;
        }

        public async Task<int> SaveDroneMedications(DroneMedicationMaster master, DroneMedicationRequest droneMedicationRequest)
        {
            await this._droneMedicationMasterRepository.Create(master);
            await this._droneMedicationMasterRepository.Save();
            int ID = master.DroneMedicationMasterID;

            foreach (var medID in droneMedicationRequest.MedicationIDs)
            {
                await this._droneMedicationDetailRepository.Create(new DroneMedicationDetail() { DroneMedicationMasterID = ID, MedicationID = medID });
            }
            await this._droneMedicationDetailRepository.Save();

            if (ID > 0)
            {
                var drone = await this._droneRepository.GetByID(master.DroneID);
                drone.State = master.Status;
                await _droneRepository.Update(drone);
                await _droneRepository.Save();
            }

            return ID;

            //transaction.Commit(); Microsoft doesn't support transactions when using inMemory database
        }

    }
}
