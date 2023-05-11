using CityInfo.Shared.Models;

namespace CItyInfo.Application
{
    public interface ICitiesFacade
    {
        Task<IEnumerable<CityWithoutPointsOfInterestDto>> GetCitiesWithoutPointsOfInterestAsync(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10);

        Task<CityWithoutPointsOfInterestDto> GetCityByIdWithoutPointsOfInterestAsync(int id);

        Task<CityDto> GetCityByIdWithPointsOfInterest(int id);
    }
}