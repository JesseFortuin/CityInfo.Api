using AutoMapper;
using CityInfo.Shared.Models;
using System.ComponentModel.DataAnnotations;
namespace CityInfo.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Entities.City, CityWithoutPointsOfInterestDto>();
            
            CreateMap<Entities.City, CityDto>();
        }
    }
}
