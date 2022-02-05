using HahnDroneAPI.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Interfaces
{
    public interface IModelRepository : IRepository<Model>
    {
        IQueryable<Model> Models { get; }

        void SeedData();
    }
}
