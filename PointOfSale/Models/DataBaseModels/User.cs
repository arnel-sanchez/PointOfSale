using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSale.Models.DataBaseModels
{
    public class User : IdentityUser
    {
        [Required]
        [DataType(DataType.Text), StringLength(20)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text), StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text), StringLength(10)]
        public string Role { get; set; }
    }
}
