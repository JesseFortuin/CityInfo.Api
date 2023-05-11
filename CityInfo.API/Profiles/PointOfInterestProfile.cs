using AutoMapper;
using CityInfo.Entities;
using CityInfo.Shared.Models;
using System.ComponentModel.DataAnnotations;
namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile//automapper namespace
    {
        public PointOfInterestProfile()
        {
            CreateMap<PointOfInterest, PointOfInterestDto>();
       
            //automapper ignores extra properties
            CreateMap<PointOfInterestForCreationDto, PointOfInterest>();

            CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();

            CreateMap<PointOfInterest, PointOfInterestForUpdateDto>();
        }
    }
}
