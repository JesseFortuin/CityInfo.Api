using CityInfo.Shared.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CItyInfo.Application
{
    public interface IPointsOfInterestFacade
    {
        Task<bool> GetAllPointsOfInterestForCityAsync (int cityId);

        Task<bool> GetSinglePointOfInterestAsync(int cityId, int pointOfInterestId);

        Task<bool> CreatePointOfInterestAsync(int cityId, PointOfInterestForCreationDto pointOfInterest);

        Task<bool> UpdatePointOfInterestAsync(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest);

        Task<bool> PartiallyUpdatePointOfInterestAsync(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument);

        Task<bool> DeletePointOfInterestAsync(int cityId, int pointOfInterestId);
    }
}
