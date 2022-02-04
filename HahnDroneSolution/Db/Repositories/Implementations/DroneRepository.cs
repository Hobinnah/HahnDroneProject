using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Db.Repositories.Interfaces;
using HahnDroneAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HahnDroneAPI.Db.Repositories.Implementations
{
    public class DroneRepository : Repository<Drone>, IDroneRepository
    {

        public DroneRepository(HahnDroneDBContext context) : base(context)
        {

        }

        public IQueryable<Drone> Drones => _context.Drones.OrderByDescending(x => x.DroneID).Include(x => x.Model).AsQueryable();

        public void SeedData(int droneUpperWeightLimit)
        {

            //Seed Drone models
            if (!_context.DroneModels.Any())
            {
                _context.DroneModels.Add(new Model() { Description = "Lightweight" });
                _context.DroneModels.Add(new Model() { Description = "Middleweight" });
                _context.DroneModels.Add(new Model() { Description = "Cruiserweight" });
                _context.DroneModels.Add(new Model() { Description = "Heavyweight" });

                _context.SaveChanges();
            }

            //Seed drones
            if (!_context.Drones.Any())
            {
                _context.Drones.Add(new Drone() { BatteryCapacity = 100, ModelID = 1, SerialNumber = "1234567890", State = StateEnum.LOADED, Weight = droneUpperWeightLimit });
                _context.Drones.Add(new Drone() { BatteryCapacity = 80, ModelID = 2, SerialNumber = "2345678901", State = StateEnum.IDLE, Weight = droneUpperWeightLimit });
                _context.Drones.Add(new Drone() { BatteryCapacity = 60, ModelID = 3, SerialNumber = "3456789012", State = StateEnum.LOADING, Weight = droneUpperWeightLimit });
                _context.Drones.Add(new Drone() { BatteryCapacity = 40, ModelID = 4, SerialNumber = "4567890123", State = StateEnum.DELIVERING, Weight = droneUpperWeightLimit });
                _context.Drones.Add(new Drone() { BatteryCapacity = 20, ModelID = 1, SerialNumber = "5678901234", State = StateEnum.RETURNING, Weight = droneUpperWeightLimit });

                _context.SaveChanges();
            }

        }
    }
}
