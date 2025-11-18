using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Charity.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public int? CityId { get; set; }
        public City? City { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserType { get; set; }
        public bool IsApproved { get; set; } = true;
    }
}
