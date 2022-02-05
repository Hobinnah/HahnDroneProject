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
            SeedData();
        }

        public IQueryable<Model> Models => _context.Models.OrderByDescending(x => x.ModelID).AsQueryable();

        public void SeedData()
        {

            //Seed Drone models
            if (!_context.Models.Any())
            {
                    _context.Models.Add(new Model() { Description = "Lightweight" });
                    _context.Models.Add(new Model() { Description = "Middleweight" });
                    _context.Models.Add(new Model() { Description = "Cruiserweight" });
                    _context.Models.Add(new Model() { Description = "Heavyweight" });

                _context.SaveChanges();
            }
        }

    }
}
