using AutoMapper;
using CityInfo.API.Profiles;
using CityInfo.Infrastructue.DataAccess;
using CityInfo.Shared.Models;
using CItyInfo.Application;
using NSubstitute;

namespace CityInfo.Test
{
    public class PointsOfInterestFacadeTest
    {
        [Fact]
        public void CreatePointOfInterestAsync_Fails_WithInvalidCityId()
        {
            //arrange
            ICityInfoRepository _cityInfoRepository = Substitute.For<ICityInfoRepository>();

            IPointsOfInterestFacade pointsOfInterestFacade = new PointsOfInterestFacade(null, null, _cityInfoRepository, null);

            var cityId = 10;

            //act
            var result = Assert.ThrowsAsync<Exception>(async () => await pointsOfInterestFacade.CreatePointOfInterestAsync(cityId, null));

            //assert
            Assert.Equal("City does not exist", result.Result.Message);
        }

        [Fact]
        public void CreatePointOfInterestAsync_Succeeds_WithValidCityId()
        {
            //arrange
            var cityId = 10;

            ICityInfoRepository _cityInfoRepository = Substitute.For<ICityInfoRepository>();

            _cityInfoRepository.CityExistsAsync(cityId).Returns(true);

            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new CityProfile());
                configuration.AddProfile(new PointOfInterestProfile());
            });

            var mapper = mapperConfiguration.CreateMapper();

            IPointsOfInterestFacade pointsOfInterestFacade = new PointsOfInterestFacade(mapper, null, _cityInfoRepository, null);

            var pointOfInterestDto = new PointOfInterestForCreationDto
            {
                Name = "Test",
                Description = "Test",
            };

            //act
            var result = Assert.ThrowsAsync<Exception>(async () => await pointsOfInterestFacade.CreatePointOfInterestAsync(cityId, pointOfInterestDto));

            //assert
            Assert.Equal("City does not exist", result.Result.Message);
        }
    }
}