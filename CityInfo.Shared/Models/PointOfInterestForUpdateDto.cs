using System.ComponentModel.DataAnnotations;

namespace CityInfo.Shared.Models
{
    public class PointOfInterestForUpdateDto
    {
        [Required(ErrorMessage = "Please provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
