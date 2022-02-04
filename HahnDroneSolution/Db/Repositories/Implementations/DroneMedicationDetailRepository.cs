using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Db.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Implementations
{
    public class DroneMedicationDetailRepository : Repository<DroneMedicationDetail>, IDroneMedicationDetailRepository
    {
        private readonly HahnDroneDBContext _context;

        public DroneMedicationDetailRepository(HahnDroneDBContext context) : base(context)
        {
            _context = context;
        }

    }
}
