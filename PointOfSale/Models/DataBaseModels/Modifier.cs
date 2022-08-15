﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.DataBaseModels
{
    public class Modifier
    {
        [Required]
        [DataType(DataType.Text), StringLength(36)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text), StringLength(20)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text), StringLength(255)]
        public string Description { get; set; }

        [Required]
        [Precision(2)]
        public decimal Price { get; set; }

        [Required]
        public bool Add { get; set; }
    }
}
