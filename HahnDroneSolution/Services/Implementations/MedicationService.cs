using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HahnDroneAPI.CustomExceptions;
using HahnDroneAPI.Db;
using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Extensions;
using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.Services.Interfaces;
using HahnDroneAPI.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HahnDroneAPI.Db.Repositories.Interfaces;

namespace HahnDroneAPI.Services.Implementations
{
    public class MedicationService : IMedicationService
    {
        private readonly IMapper _mapper;
        private readonly IMedicationRepository _medicationRepository;

        public MedicationService(IMapper mapper, IMedicationRepository medicationRepository)
        {
            this._mapper = mapper;
            this._medicationRepository = medicationRepository;

            this._medicationRepository.SeedData();
        }

        public async Task<MedicationResponse> GetMedicationsAsync(QueryParameters queryParameters)
        {
            
            IQueryable<Medication> medications = this._medicationRepository.Medications;

            var allMedications = medications;

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                medications = medications?.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy) && typeof(MedicationDto).GetProperty(queryParameters.SortBy) != null)
            {
                medications = medications?.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
            }

            if (medications == null || !medications.Any())
            {
                throw new MessageException("Medication was not found", HttpStatusCode.NotFound);
            }

            var filteredMedications = medications.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);

            var result = _mapper.Map<IEnumerable<Medication>, IEnumerable<MedicationDto>>(await filteredMedications.ToListAsync());
            MedicationResponse medicationResponse = new MedicationResponse();

            medicationResponse.Medications = result;
            medicationResponse.Count = medications.Count();

            return medicationResponse;

            
        }

        public async Task<MedicationDto> GetMedicationAsync(int medicationID)
        {

            Medication medication = await this._medicationRepository.GetByID(medicationID);

            if (medication == null || medication.MedicationID == 0)
            {
                throw new MessageException("Medication was not found", HttpStatusCode.NotFound);
            }

            var result = _mapper.Map<Medication, MedicationDto>(medication);
            return result;
        }

        public async Task<MedicationDto> CreateMedicationAsync(MedicationDto medication)
        {

            Medication newMedication = _mapper.Map<MedicationDto, Medication>(medication);

            if (newMedication == null || string.IsNullOrEmpty(newMedication.Name))
            {
                throw new MessageException("Bad request", HttpStatusCode.BadRequest);
            }

            if (this._medicationRepository.FindWhere(x => x.Name.ToLower().Contains(newMedication.Name.ToLower())).Any())
            {
                throw new MessageException("Medication not found", HttpStatusCode.NotFound);
            }

            await this._medicationRepository.Create(newMedication);
            await this._medicationRepository.Save();

            var result = _mapper.Map<Medication, MedicationDto>(newMedication);

            return result;

        }

        public async Task<MedicationDto> UpdateMedicationAsync(MedicationDto medication, int medicationID)
        {
            
            Medication newMedication = _mapper.Map<MedicationDto, Medication>(medication);

            if (newMedication == null || string.IsNullOrEmpty(newMedication.Name) || newMedication.MedicationID != medicationID)
            {
                throw new MessageException("Bad request", HttpStatusCode.BadRequest);
            }

            var med = await this._medicationRepository.GetByID(medicationID);
            if (med == null)
            {
                throw new MessageException("Medication not found", HttpStatusCode.NotFound);
            }

            med.Code = newMedication.Code;
            med.Image = newMedication.Image;
            med.Name = newMedication.Name;
            med.Weight = newMedication.Weight;

            await this._medicationRepository.Update(med);
            await this._medicationRepository.Save();

            var result = _mapper.Map<Medication, MedicationDto>(med);

            return result;

        }

        public async Task<MedicationDto> DeleteMedicationAsync(int medicationID)
        {
            
            var newMedication = await this._medicationRepository.GetByID(medicationID);
            if (newMedication == null)
            {
                throw new MessageException("Medication not found", HttpStatusCode.NotFound);
            }

            await this._medicationRepository.Delete(newMedication);
            await this._medicationRepository.Save();

            var result = _mapper.Map<Medication, MedicationDto>(newMedication);

            return result;

        }

    }
}
