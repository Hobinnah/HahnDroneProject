using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Db.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Implementations
{
    public class ModelRepository : Repository<Model>, IModelRepository
    {
        
        public ModelRepository(HahnDroneDBContext context) : base(context)
        {
            
        }

        public IQueryable<Model> Models => _context.DroneModels.OrderByDescending(x => x.ModelID).AsQueryable();

    }
}
