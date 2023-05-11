using AutoMapper;
using CityInfo.Infrastructue.DataAccess;
using CityInfo.Shared.Models;
using CItyInfo.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
    [Authorize(Policy = "MustBeFromAntwerp")]
    [ApiVersion("2.0")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase//exposes user object(with claims id)
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        private readonly IPointsOfInterestFacade _pointsOfInterestFacade;

        //contructor injection for Ilogger of T (poiController)
        //requesting dependencies
        public PointsOfInterestController(
            ICityInfoRepository cityInfoRepository,
            IMapper mapper,
            IPointsOfInterestFacade pointsOfInterestFacade)
        {
            //if type injected cannot be found
            //can request service in container directly 
            //e.g HttpContext.RequestServices.GetService

            _cityInfoRepository = cityInfoRepository ??
               throw new ArgumentNullException(nameof(cityInfoRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _pointsOfInterestFacade = pointsOfInterestFacade ??
                throw new ArgumentNullException(nameof(pointsOfInterestFacade));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest
            (int cityId)
        {
            //accessing city claim and using it to restrict points of interest to that value
            var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            if (!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
            {
                //403 authenticated but not allowed access
                return Forbid();
            }

            var cityExists = await _pointsOfInterestFacade.GetAllPointsOfInterestForCityAsync(cityId);

            if (cityExists == false)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointofInterest
            (int cityId, int pointOfInterestId)
        {
            var pointOfInterestExists = await _pointsOfInterestFacade.GetSinglePointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterestExists == false)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest
            (int cityId,
             PointOfInterestForCreationDto pointOfInterest)
        {
            var cityExists = await _pointsOfInterestFacade.CreatePointOfInterestAsync(cityId, pointOfInterest);

            if (cityExists == false)
            {
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<CityInfo.Entities.PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId
                , finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var createdpointofInterestToReturn =
                _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            //allows to return response with location header
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = createdpointofInterestToReturn.Id
                },
                createdpointofInterestToReturn);
        }

        [HttpPut("{pointofinterestid}")]

        //not action result of T as it returns a no content
        public async Task<ActionResult> UpdatePointofInterest(int cityId,
            int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            var pointOfInterestExists = await _pointsOfInterestFacade.UpdatePointOfInterestAsync(cityId,
                pointOfInterestId,
                pointOfInterest);

            if (pointOfInterestExists == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]

        public async Task<ActionResult> PartiallyUpdatePointOfInterest
            (int cityId, int pointOfInterestId, JsonPatchDocument
            <PointOfInterestForUpdateDto> patchDocument)
        {
            var pointOfInterestEntity = await _pointsOfInterestFacade.PartiallyUpdatePointOfInterestAsync(cityId, pointOfInterestId, patchDocument);

            if (pointOfInterestEntity == false)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]

        public async Task<ActionResult> DeletePointofInterest(int cityId,
            int pointOfInterestId)
        {
            var pointOfinterestToDelete = await _pointsOfInterestFacade.DeletePointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfinterestToDelete == false)
            {
                return NotFound();
            };

            return NoContent();
        }
    }
}
