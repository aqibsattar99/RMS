﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Active { get; set; }
    }
}