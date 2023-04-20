using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class City
    {
        //or set it manually
        [Key]
        //generating Id primary keys
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }//naming it Id sets it as primary key(convention)

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Province { get; set; }

        //list of children
        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
            = new List<PointOfInterest>();

        public City(string name)
        {
            Name = name;
        }
    }
}
