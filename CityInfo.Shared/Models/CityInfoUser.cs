using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.Shared.Models
{
    public class CityInfoUser
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public CityInfoUser(
            int userId,
            string userName,
            string firstName,
            string lastName,
            string city)
        {
            UserId = userId;

            UserName = userName;

            FirstName = firstName;

            LastName = lastName;

            City = city;
        }
    }
}
