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
    public class ModelService : IModelService
    {
        private readonly IMapper _mapper;
        private readonly HahnDroneDBContext _context;
        private readonly IModelRepository _modelRepository;

        public ModelService(IMapper mapper, IModelRepository medicationRepository)
        {
            this._mapper = mapper;
            this._modelRepository = medicationRepository;

        }

        public async Task<ModelResponse> GetModelsAsync(QueryParameters queryParameters)
        {
            
            IQueryable<Model> models = this._modelRepository.Models;

            var allModels = models;

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                models = models?.Where(p => p.Description.ToLower().Contains(queryParameters.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy) && typeof(ModelDto).GetProperty(queryParameters.SortBy) != null)
            {
                models = models?.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
            }

            if (models == null || !models.Any())
            {
                throw new MessageException("Model was not found", HttpStatusCode.NotFound);
            }

            var filteredModels = models.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);

            var result = _mapper.Map<IEnumerable<Model>, IEnumerable<ModelDto>>(await filteredModels.ToListAsync());
            ModelResponse medicationResponse = new ModelResponse();

            medicationResponse.Models = result;
            medicationResponse.Count = models.Count();

            return medicationResponse;

            
        }

    }
}
