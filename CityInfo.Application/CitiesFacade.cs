using AutoMapper;
using CityInfo.Entities;
using CityInfo.Infrastructue.DataAccess;
using CityInfo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CItyInfo.Application
{
    public class CitiesFacade : ICitiesFacade
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesFacade(
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityWithoutPointsOfInterestDto>> GetCitiesWithoutPointsOfInterestAsync(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            var (cityEntities, paginationMetadata) = await _cityInfoRepository
                .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            var mapResult = _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

            return mapResult;
        }

        public async Task<CityWithoutPointsOfInterestDto> GetCityByIdWithoutPointsOfInterestAsync(int id)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, false);

            var mapResult = _mapper.Map<CityWithoutPointsOfInterestDto>(city);

            return mapResult;
        }

        public async Task<CityDto> GetCityByIdWithPointsOfInterest(int id)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, true);

            var mapResult = _mapper.Map<CityDto>(city);

            return mapResult;
        }
    }
}
