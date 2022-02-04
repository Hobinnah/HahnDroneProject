using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class MedicationServiceTestCases
    {
        IMapper _mapper;
        IMedicationService medicationService;
        MedicationDto medication;

        public MedicationServiceTestCases()
        {
            var options = new DbContextOptionsBuilder<HahnDroneDBContext>().UseInMemoryDatabase(databaseName: "MockDB").Options;
            var context = new HahnDroneDBContext(options);
            var medRepo = new MedicationRepository(context);

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MedicationProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            medicationService = new MedicationService(_mapper, medRepo);
        }

        [TestInitialize]
        public void Initalize()
        {
            medication = new MedicationDto() { Name = "Hydrocodone123", Code = "HYD1234", Weight = 300, Image = "https://c8.alamy.com/comp/2A32WJP/hydrocodone-concept-image-with-molecule-and-chemical-formula-2A32WJP.jpg" };

        }

        [TestMethod]
        public async Task GetMedicationsAsync_ShouldReturnSomeRecords()
        {
            QueryParameters queryParameters = new QueryParameters();
            var result = await this.medicationService.GetMedicationsAsync(queryParameters);

            Assert.IsNotNull(result.Medications);
        }

        [TestMethod]
        public async Task GetMedicationsAsync_ShouldReturnOneRecord()
        {
            int ID = 1;
            var drone = await this.medicationService.GetMedicationAsync(ID);

            Assert.IsNotNull(drone);
        }

        [TestMethod]
        public async Task CreateMedicationAsync_ShouldReturnObjectWithMedicationID()
        {
            var result = await this.medicationService.CreateMedicationAsync(medication);
            Assert.AreEqual(6, result.MedicationID);
        }

        [TestMethod]
        public async Task UpdateMedicationAsync_ShouldReturnObjectWithUpdatedStateField()
        {

            var newMedication = await this.medicationService.GetMedicationAsync(1);
            newMedication.Code = "HYD1238";
            var result = await this.medicationService.UpdateMedicationAsync(newMedication, newMedication.MedicationID);

            Assert.AreEqual("HYD1238", result.Code);
        }

        [TestMethod]
        public async Task DeleteMedicationAsync_ShouldThrowsMedicationNotExistException()
        {
            await this.medicationService.DeleteMedicationAsync(5);
            await Assert.ThrowsExceptionAsync<MessageException>(() => this.medicationService.GetMedicationAsync(5));
        }


    }
}
