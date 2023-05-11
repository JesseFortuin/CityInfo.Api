using AutoMapper;
using CityInfo.Shared.Models;
using CItyInfo.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    //controller level
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities")]
    //base contains functionality controllers need
    //access to model state, current user, methods to return responses
    public class CitiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICitiesFacade _citiesFacade;
        

        public CitiesController(
            IMapper mapper,
            ICitiesFacade citiesFacade)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _citiesFacade = citiesFacade;
        }

        //action level
        [HttpGet()]
        //returns any data in contructor is Represented in Json form
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            var cityEntities = _citiesFacade.GetCitiesWithoutPointsOfInterestAsync(name, searchQuery, pageNumber, pageSize);

            //Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(cityEntities);
        }

        /// <summary>
        /// Get a city by id
        /// </summary>
        /// <param name="id">The id of the city to get</param>
        /// <param name="includePointsOfInterest">whether or not to include points of interest</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested city</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCityById(
            int id, bool includePointsOfInterest = false)
        {
            //if (city == null)
            //{
            //    return NotFound();
            //}

            if (includePointsOfInterest)
            {
                var cityWithPointsOfInterest = await _citiesFacade.GetCityByIdWithPointsOfInterest(id);

                if (cityWithPointsOfInterest == null)
                {
                    return NotFound();
                }

                return Ok(cityWithPointsOfInterest);
            }

            var cityWithoutPointsOfInterest = await _citiesFacade.GetCityByIdWithoutPointsOfInterestAsync(id);

            if (cityWithoutPointsOfInterest == null)
            {
                return NotFound();
            }

            return Ok(cityWithoutPointsOfInterest);
            //if an exception is not handled now, a 500 server error appears
        }
    }
}
