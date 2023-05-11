using CityInfo.API.Entities;
using CityInfo.API.Services;

namespace CityInfo.Infrastructure
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize);

        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

        Task<bool> CityExistsAsync(int cityId);


        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync
            (int cityId);

        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId
            , int pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        
        //in-memory operation. async if it calls into another async method, like add method
        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);

        Task<bool> SaveChangesAsync();
    }
}
