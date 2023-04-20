using AutoMapper;
using System.ComponentModel.DataAnnotations;
namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile//automapper namespace
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
       
            //automapper ignores extra properties
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();

            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();

            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
        }
    }
}
