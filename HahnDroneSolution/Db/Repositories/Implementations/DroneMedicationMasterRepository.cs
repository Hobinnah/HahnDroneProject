using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Db.Repositories.Interfaces;
using HahnDroneAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HahnDroneAPI.Db.Repositories.Implementations
{
    public class DroneMedicationMasterRepository : Repository<DroneMedicationMaster>, IDroneMedicationMasterRepository
    {

        public DroneMedicationMasterRepository(HahnDroneDBContext context) : base(context)
        {

        }

        public IEnumerable<DroneMedicationMaster>  DroneMedicationMasters(int droneID) => _context.DroneMedicationMaster.Where(x => x.DroneID == droneID).OrderByDescending(x => x.DroneMedicationMasterID).Include(x => x.DroneMedicationDetails);

        public IEnumerable<DroneMedicationMaster> DroneWithMedication(int droneID) => _context.DroneMedicationMaster.Where(x => x.DroneID == droneID).OrderByDescending(x => x.DroneMedicationMasterID).Include(x => x.DroneMedicationDetails).ThenInclude(x => x.Medication);

        public IEnumerable<DroneMedicationMaster> DroneWithMedication_Loading(int droneID) => _context.DroneMedicationMaster.Where(x => x.DroneID == droneID && x.Status == StateEnum.LOADING).OrderByDescending(x => x.DroneMedicationMasterID).Include(x => x.DroneMedicationDetails).ThenInclude(x => x.Medication).ToList();
        public void SeedData()
        {
            //Seed drone Medications
            if (!_context.DroneMedicationMaster.Any())
            {
                _context.DroneMedicationMaster.Add(new DroneMedicationMaster() { CapturedBy = "System", CapturedDate = DateTime.Now, DroneID = 1, Status = StateEnum.LOADING });
                _context.DroneMedicationDetails.Add(new DroneMedicationDetail() { DroneMedicationMasterID = 1, MedicationID = 2 });
                _context.DroneMedicationDetails.Add(new DroneMedicationDetail() { DroneMedicationMasterID = 1, MedicationID = 3 });
                _context.DroneMedicationDetails.Add(new DroneMedicationDetail() { DroneMedicationMasterID = 1, MedicationID = 4 });

                _context.SaveChanges();
            }

        }
    }
}
