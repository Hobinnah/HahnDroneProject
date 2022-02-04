using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Interfaces
{
    public interface IUnitOfWorkRepository
    {
        Task<int> SaveDroneMedications(DroneMedicationMaster master, DroneMedicationRequest droneMedicationRequest);
    }
}
