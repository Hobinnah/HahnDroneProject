using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HahnDroneAPI.Configurations;
using HahnDroneAPI.Controllers;
using HahnDroneAPI.Db;
using HahnDroneAPI.Models;
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
    public class DispatchControllerTestCases
    {
        IMapper _mapper;
        IDroneService _droneService;
        IMedicationService _medicationService;
        IDroneMedicationService _droneMedicationService;
        IAuditEventLogService _auditEventLogService;
        Mock<ICustomConfiguration> config = new Mock<ICustomConfiguration>();

        DispatchV1_Controller dispatchController;

        DroneDto drone;
        MedicationDto medication;
        DroneMedicationRequest droneMedicationRequest;

        public DispatchControllerTestCases()
        {
            var options = new DbContextOptionsBuilder<HahnDroneDBContext>().UseInMemoryDatabase(databaseName: "MockDB").Options;
            var context = new HahnDroneDBContext(options);
            var droneRepo = new DroneRepository(context);
            var droneMedicationMasterRepository = new DroneMedicationMasterRepository(context);
            var medRepo = new MedicationRepository(context);
            var auditRepo = new AuditEventLogRepository(context);
            var droneMedicationDetailRepository = new DroneMedicationDetailRepository(context);
            var unitOfWorkRepo = new UnitOfWorkRepository(droneRepo, droneMedicationMasterRepository, droneMedicationDetailRepository);

            config.CallBase = true;
            config.Setup(x => x.GetDroneUpperWeightLimit()).Returns(500);
            this.config.Setup(x => x.BatteryLowerLimit()).Returns(25);
            this.config.Setup(x => x.DroneCount()).Returns(6);

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new DroneProfile());
                    mc.AddProfile(new MedicationProfile());
                    mc.AddProfile(new DroneMedicationMasterProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _droneMedicationService = new DroneMedicationService(_mapper, config.Object, droneMedicationMasterRepository, droneRepo, droneMedicationDetailRepository, medRepo, unitOfWorkRepo);
            _droneService = new DroneService(_mapper, config.Object, droneRepo, droneMedicationMasterRepository);
            _medicationService = new MedicationService(_mapper, medRepo);
            _auditEventLogService = new AuditEventLogService(_mapper, config.Object, auditRepo, droneRepo);

            dispatchController = new DispatchV1_Controller(_droneService, _medicationService, _droneMedicationService, _auditEventLogService);
        }

        [TestInitialize]
        public void Initalize()
        {
            medication = new MedicationDto() { Name = "Hydrocodone1230", Code = "HYD12340", Weight = 300, Image = "https://c8.alamy.com/comp/2A32WJP/hydrocodone-concept-image-with-molecule-and-chemical-formula-2A32WJP.jpg" };
            drone = new DroneDto() { BatteryCapacity = 100, ModelID = 0, SerialNumber = "09876054321", State = 0, Weight = 500 };
            droneMedicationRequest = new DroneMedicationRequest() { CapturedBy = "System", CapturedDate = DateTime.Now, DroneID = 3, MedicationIDs = new List<int> { 1 } };
        }

        [TestMethod]
        public async Task CreateDroneAsync_ShouldReturnCreatedObject()
        {
            var response = await this.dispatchController.Create(drone);

            Assert.IsInstanceOfType(response, typeof(CreatedResult));
        }

        [TestMethod]
        public async Task LoadDrone_ShouldReturnOkObject()
        {
            var response = await this.dispatchController.LoadDrone(droneMedicationRequest);

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Get_ShouldReturnOkObject()
        {
            QueryParameters queryParameters = new QueryParameters();
            queryParameters.ID = 1;
            var response = await this.dispatchController.Get(queryParameters);

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetDroneMedications_ShouldReturnOkObject()
        {
            var response = await this.dispatchController.GetDroneMedications(1);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetAvaliableDrones_ShouldReturnOkObject()
        {
            var response = await this.dispatchController.GetAvaliableDrones();
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetDroneBatteryLevel_ShouldReturnOkObject()
        {
            var response = await this.dispatchController.GetDroneBatteryLevel(1);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetAuditEventLog_ShouldReturnOkObject()
        {
            QueryParameters queryParameters = new QueryParameters();
            var response = await this.dispatchController.GetAuditEventLog(queryParameters);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
        }

    }
}
