using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HahnDroneAPI.Configurations;
using HahnDroneAPI.CustomExceptions;
using HahnDroneAPI.Db;
using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.Profiles.Profiles;
using HahnDroneAPI.Services.Implementations;
using HahnDroneAPI.Services.Interfaces;
using System.Threading.Tasks;
using HahnDroneAPI.Db.Repositories.Implementations;

namespace HahnDroneAPI.Test
{
    [TestClass]
    public class DroneServiceTestCases
    {
        IMapper _mapper;
        Mock<ICustomConfiguration> config = new Mock<ICustomConfiguration>();
        IDroneService droneService;
        DroneDto drone;
       
        public DroneServiceTestCases()
        {
            var options = new DbContextOptionsBuilder<HahnDroneDBContext>().UseInMemoryDatabase(databaseName: "MockDB").Options;
            var context = new HahnDroneDBContext(options);
            var droneRepo = new DroneRepository(context);
            var droneMedicationMasterRepository = new DroneMedicationMasterRepository(context);

            config.CallBase = true;
            config.Setup(x => x.GetDroneUpperWeightLimit()).Returns(500);
            this.config.Setup(x => x.DroneCount()).Returns(6);

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new DroneProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            droneService = new DroneService(_mapper, config.Object, droneRepo, droneMedicationMasterRepository);
        }

        [TestInitialize]
        public void Initalize()
        {
            drone = new DroneDto() { BatteryCapacity = 100, ModelID = 0, SerialNumber = "0987654321", State = 0, Weight = 500 };

        }

        [TestMethod]
        public void IsValidDroneWeight_ShouldReturnFalse()
        {

            drone.Weight = 501;
            bool result = droneService.IsValidDroneWeight(drone.Weight);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidDroneWeight_ShouldReturnTrue()
        {
            drone.Weight = 500;
            bool result = this.droneService.IsValidDroneWeight(drone.Weight);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetDronesAsync_ShouldReturnSomeRecords()
        {
            QueryParameters queryParameters = new QueryParameters();
            var result = await this.droneService.GetDronesAsync(queryParameters);

            Assert.IsNotNull(result.Drones);
        }

        [TestMethod]
        public async Task GetDronesAsync_ShouldReturnOneRecord()
        {
            int droneID = 1;
            var drone = await this.droneService.GetDroneAsync(droneID);

            Assert.IsNotNull(drone);
        }

        [TestMethod]
        public async Task CreateDroneAsync_ShouldReturnObjectWithDroneID()
        {
            var result = await this.droneService.CreateDroneAsync(drone);
            Assert.IsTrue(result.DroneID > 0);
        }

        [TestMethod]
        public async Task CreateDroneAsync_ShouldThrowsDroneLimitException()
        {
            await Assert.ThrowsExceptionAsync<MessageException>(() => this.droneService.CreateDroneAsync(drone));
        }

        [TestMethod]
        public async Task UpdateDroneAsync_ShouldReturnObjectWithUpdatedStateField()
        {
            
            var newDrone = await this.droneService.GetDroneAsync(1);
            newDrone.State = Models.Enums.StateEnum.RETURNING;
            var result = await this.droneService.UpdateDroneAsync(newDrone, newDrone.DroneID);

            Assert.AreEqual(Models.Enums.StateEnum.RETURNING, result.State);
        }

        [TestMethod]
        public async Task DeleteDroneAsync_ShouldThrowsDroneNotExistException()
        {
            await this.droneService.DeleteDroneAsync(2);
            await Assert.ThrowsExceptionAsync<MessageException>(() => this.droneService.GetDroneAsync(2));
        }

        [TestMethod]
        public async Task GetAvaliableDronesAsync_ShouldNotReturnNull()
        {
            var result = await this.droneService.GetAvaliableDronesAsync();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetDroneBatteryLevelAsync_ShouldReturnBatteryLevel()
        {
            var result = await this.droneService.GetDroneBatteryLevelAsync(1);
            Assert.AreEqual(100, result.Level);
        }
    }
}
