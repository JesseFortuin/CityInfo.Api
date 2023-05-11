using AutoMapper;
using CityInfo.Infrastructue.Services;
using CityInfo.Infrastructue.DataAccess;
using CityInfo.Shared.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CityInfo.Entities;

namespace CItyInfo.Application
{
    public class PointsOfInterestFacade : IPointsOfInterestFacade
    {
        private readonly IMapper _mapper;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMailService _mailService;
        private readonly ILogger<PointsOfInterestFacade> _logger;

        public PointsOfInterestFacade(
            IMapper mapper,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            ILogger <PointsOfInterestFacade> logger)
        {
            _logger = logger; //??
                              //throw new ArgumentNullException(nameof(logger));

            _cityInfoRepository = cityInfoRepository;
            //??
            //throw new ArgumentNullException(nameof(cityInfoRepository));

            _mailService = mailService;
            //??
            //throw new ArgumentNullException(nameof(mailService));

            _mapper = mapper;
                //??
                //throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> CreatePointOfInterestAsync(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                throw new Exception("City does not exist");

                //return false;
            }

            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId
                , finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            //var createdpointofInterestToReturn =
                //_mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);


            //return CreatedAtRoute("GetPointOfInterest",
            //    new
            //    {
            //        cityId = cityId,
            //        pointOfInterestId = createdpointofInterestToReturn.Id
            //    },
            //    createdpointofInterestToReturn);

            return true;
        }

        public async Task<bool> DeletePointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return false;
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync
                (cityId, pointOfInterestId);

            if (pointOfInterestEntity == null) 
            {
                return false;
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send("Point of interest deleted.",
            $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted");

            return true;
        }

        public async Task<bool> GetAllPointsOfInterestForCityAsync(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found when accessing point of interest");

                return false;
            }

            var pointsOfInterestForCity = await _cityInfoRepository
                .GetPointsOfInterestForCityAsync(cityId);

            _mapper.Map<ActionResult<IEnumerable<PointOfInterestDto>>>(pointsOfInterestForCity);

            return true;
        }

        public async Task<bool> GetSinglePointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return false;
            }

            var pointOfInterest = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            
            if (pointOfInterest == null)
            {
                return false;
            }

            _mapper.Map<PointOfInterestDto>(pointOfInterest);

            return true;
        }

        public async Task<bool> PartiallyUpdatePointOfInterestAsync(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return false;
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync
                (cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> UpdatePointOfInterestAsync(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return false;
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync
                (cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return false;
            }

            _mapper.Map(pointOfInterest, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return true;
        }
    }
}
