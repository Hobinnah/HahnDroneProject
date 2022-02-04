using HahnDroneAPI.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Interfaces
{
    public interface IDroneMedicationMasterRepository : IRepository<DroneMedicationMaster>
    {
        public IEnumerable<DroneMedicationMaster> DroneMedicationMasters(int droneID);

        public IEnumerable<DroneMedicationMaster> DroneWithMedication(int droneID);
        public IEnumerable<DroneMedicationMaster> DroneWithMedication_Loading(int droneID);

        void SeedData();
    }
}
