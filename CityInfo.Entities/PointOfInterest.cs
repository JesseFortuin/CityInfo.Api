using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        //manual
        [ForeignKey("CityId")]
        public City? City { get; set; }//navigation property(to parent relationship)

        public int CityId { get; set; }//foreign key

        public PointOfInterest(string name)
        {
            Name = name;
        }
    }
}
