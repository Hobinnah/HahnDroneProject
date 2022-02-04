using HahnDroneAPI.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Interfaces
{
    public interface IDroneRepository : IRepository<Drone>
    {
        IQueryable<Drone> Drones { get; }

        void SeedData(int droneUpperWeightLimit);
    }
}
