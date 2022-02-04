using HahnDroneAPI.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Interfaces
{
    public interface IMedicationRepository : IRepository<Medication>
    {
        IQueryable<Medication> Medications { get; }

        void SeedData();
    }
}
