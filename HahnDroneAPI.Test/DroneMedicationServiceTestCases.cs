using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HahnDroneAPI.Configurations;
using HahnDroneAPI.Db;
using HahnDroneAPI.Models;
using HahnDroneAPI.Models.Enums;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.Profiles.Profiles;
using HahnDroneAPI.Services.Implementations;
using HahnDroneAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HahnDroneAPI.Db.Repositories.Implementations;

namespace HahnDroneAPI.Test
{
    [TestClass]
    public class DroneMedicationServiceTestCases
    {
        IMapper _mapper;
        Mock<ICustomConfiguration> config = new Mock<ICustomConfiguration>();
        IDroneMedicationService droneMedicationService;
        DroneMedicationRequest droneMedicationRequest;
        IDroneService droneService;
        IMedicationService medicationService;

        public DroneMedicationServiceTestCases()
        {
            var options = new DbContextOptionsBuilder<HahnDroneDBContext>().UseInMemoryDatabase(databaseName: "MockDB").Options;
            var context = new HahnDroneDBContext(options);
            var droneRepo = new DroneRepository(context);
            var droneMedicationMasterRepository = new DroneMedicationMasterRepository(context);
            var droneMedicationDetailRepository = new DroneMedicationDetailRepository(context);
            var medRepo = new MedicationRepository(context);
            var unitOfWorkRepo = new UnitOfWorkRepository(droneRepo, droneMedicationMasterRepository, droneMedicationDetailRepository);

            config.CallBase = true;
            config.Setup(x => x.GetDroneUpperWeightLimit()).Returns(500);
            this.config.Setup(x => x.BatteryLowerLimit()).Returns(25);

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new DroneMedicationMasterProfile());
                    mc.AddProfile(new MedicationProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            droneMedicationService = new DroneMedicationService(_mapper, config.Object, droneMedicationMasterRepository, droneRepo, droneMedicationDetailRepository, medRepo, unitOfWorkRepo);
            droneService = new DroneService(_mapper, config.Object, droneRepo, droneMedicationMasterRepository);
            medicationService = new MedicationService(_mapper, medRepo);
        }

        [TestInitialize]
        public void Initalize()
        {
            droneMedicationRequest = new DroneMedicationRequest() { CapturedBy = "System", CapturedDate = DateTime.Now, DroneID = 3, MedicationIDs = new List<int> { 1 } };
        }

        [TestMethod]
        public async Task GetDroneMedicationsAsync_ShouldReturnSomeRecords()
        {
            QueryParameters queryParameters = new QueryParameters();
            queryParameters.ID = 1;
            var result = this.droneMedicationService.GetDroneMedicationsAsync(queryParameters);

            Assert.IsNotNull(result.Medications);
        }

        [TestMethod]
        public async Task DroneBatteryCapacity_ShouldReturnTrueIfBatteryLevelIsGreaterThanLimit()
        {
            int droneID = 1;
            var isBatteryLevelValid = await this.droneMedicationService.DroneBatteryCapacity(droneID);

            Assert.IsTrue(isBatteryLevelValid);
        }


        [TestMethod]
        public async Task IsDroneAvailableForLoading_ShouldReturnTrueIfAvaliableForLoading()
        {
            var isDroneAvailableForLoading = await this.droneMedicationService.IsDroneAvailableForLoading(2);

            Assert.IsTrue(isDroneAvailableForLoading);
        }
    }
}
